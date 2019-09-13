namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpGet]
        //[Authorize]
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
        [HttpGet("/id/{userid}")]
        //[Authorize]
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
    }
}