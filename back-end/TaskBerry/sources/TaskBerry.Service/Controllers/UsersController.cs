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
    using System.Linq;
    using System;


    /// <summary>
    /// Users controller.
    /// </summary>
    [ApiController]
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
        public ActionResult<IEnumerable<User>> GetUsersByGroupId(Guid groupId)
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsersByGroupId(groupId);

            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/users/current-user")]
        [Authorize]
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
    }
}