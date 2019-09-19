namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Service.Constants;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Linq;
    using System;


    /// <summary>
    /// Controller for user functions.
    /// </summary>
    [ApiController]
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskBerry"></param>
        public UsersController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Returns all users.</returns>
        /// <response code="200">Successfully returned all users.</response>
        [Authorize]
        [HttpGet("/api/users")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsers();

            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        /// <param name="userid">The user id.</param>
        /// <returns>Returns a user.</returns>
        /// <response code="404">User was not found.</response>
        /// <response code="200">Successfully returned the user.</response>
        [Authorize]
        [HttpGet("/api/users/{userid:int}")]
        [Produces("application/json")]
        [SwaggerResponse(404, "User by id not found.")]
        [SwaggerResponse(200, "User by id successfully returned.")]
        public ActionResult<User> GetUserById(int userid)
        {
            UserEntity entity = this._taskBerry.UsersRepository.GetUserById(userid);

            if (entity == null)
            {
                return this.NotFound(userid);
            }

            return this.Ok(entity.ToModel());
        }

        /// <summary>
        /// Gets users by the group id.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <returns>Returns all users in a group.</returns>
        /// <response code="200">Successfully returned the users.</response>
        [Authorize]
        [HttpGet("/api/users/users-by-group/{groupId:guid}")]
        [Produces("application/json")]
        public ActionResult<User[]> GetUsersByGroupId(Guid groupId)
        {
            IEnumerable<User> entities = this._taskBerry.UsersRepository.GetUsersByGroupId(groupId);

            return this.Ok(entities.ToArray());
        }

        /// <summary>
        /// Returns the current user.
        /// </summary>
        /// <returns>Returns the current user.</returns>
        /// <response code="200">Successfully returned the current user.</response>
        [Authorize]
        [HttpGet("/api/users/current-user")]
        [Produces("application/json")]
        public ActionResult<User> GetCurrentUser()
        {
            if (this.User == null)
            {
                return this.NotFound("No user is not logged in.");
            }

            // TODO Query current user
            User user = new User();

            return this.Ok(user);
        }

        /// <summary>
        /// Gets all school classes.
        /// </summary>
        /// <returns>Returns all school classes.</returns>
        /// <response code="200">Successfully returned all school classes.</response>
        [Authorize]
        [HttpGet("/api/users/get-classes")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<string>> GetClasses()
        {
            return this.Ok();
        }

        /// <summary>
        /// Gets all users in a school class.
        /// </summary>
        /// <param name="schoolClass">The school class name.</param>
        /// <returns>Returns all users in a school class.</returns>
        /// <response code="200">Successfully returned the users in a school class.</response>
        [Authorize]
        [HttpGet("/api/users/get-users-in-class")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<User>> GetUsersInClass(string schoolClass)
        {
            return this.Ok();
        }

        /// <summary>
        /// Checks if the user exists by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Returns true if the user exists.</returns>
        /// <response code="200">A boolean value as result.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("/api/users/user-exists")]
        [Produces("application/json")]
        public ActionResult<bool> UserExists(string email)
        {
            return this._taskBerry.Context.Users.Any(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Checks if the user is registered.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Returns true if the user is registered.</returns>
        /// <response code="200">Successfully returned the value.</response>
        /// <response code="404">User with the email does not exist.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("/api/users/is-user-registered")]
        [Produces("application/json")]
        public ActionResult<bool> IsUserRegistered(string email)
        {
            UserEntity userEntity = this._taskBerry.Context.Users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (userEntity == null)
            {
                return this.NotFound($"User with the email {email} does not exist.");
            }

            return this._taskBerry.Context.UserInfos.Any(info => info.UserId == userEntity.Id);
        }

        /// <summary>
        /// Changes a password of a user that is registered.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>Returns the result of the request.</returns>
        /// <response code="200">Successfully changed the password of the user.</response>
        /// <response code="404">The user was not found or the user was not registered.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("/api/users/change-password")]
        [Produces("application/json")]
        public IActionResult ChangeUserPassword(string email, string newPassword)
        {
            using (MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes(newPassword));
                string hashedPassword = Encoding.ASCII.GetString(bytes);

                UserEntity userEntity = this._taskBerry.Context.Users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

                if (userEntity == null)
                {
                    return this.NotFound($"Could not find user {email}.");
                }

                UserInfoEntity infoEntity = this._taskBerry.Context.UserInfos.FirstOrDefault(info => info.UserId == userEntity.Id);

                if (infoEntity == null)
                {
                    return this.NotFound($"User {email} was not registered");
                }

                infoEntity.Password = hashedPassword;
                this._taskBerry.Context.SaveChanges();
            }

            return this.Ok($"Successfully changed password of {email}.");
        }
    }
}