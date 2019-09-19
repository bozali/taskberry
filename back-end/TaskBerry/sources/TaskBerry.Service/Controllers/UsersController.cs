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
    /// Users controller.
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
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/users")]
        [Produces("application/json")]
        [SwaggerResponse(200, "All users successfully returned.")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsers();

            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
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
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/users/users-by-group/{groupId:guid}")]
        [Produces("application/json")]
        public ActionResult<User[]> GetUsersByGroupId(Guid groupId)
        {
            IEnumerable<User> entities = this._taskBerry.UsersRepository.GetUsersByGroupId(groupId);

            return this.Ok(entities.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/users/current-user")]
        [Produces("application/json")]
        public ActionResult<User> GetCurrentUser()
        {
            if (this.User == null)
            {
                return this.NotFound("No user is not logged in.");
            }

            // Query current user
            User user = new User();


            return this.Ok(user);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/users/get-classes")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<string>> GetClasses()
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="schoolClass"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/users/get-users-in-class")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<User>> GetUsersInClass(string schoolClass)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("/api/users/user-exists")]
        [Produces("application/json")]
        public ActionResult<bool> UserExists(string email)
        {
            return this._taskBerry.Context.Users.Any(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        /// </summary>
        /// <returns></returns>
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