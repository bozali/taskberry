namespace TaskBerry.Service.Controllers
{
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
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsers();
            List<User> users = entities.Select(entity => entity.ToModel()).ToList();

            return this.Ok(users);
        }

        /// <summary>
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsersByFirstName(string firstName)
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsersByFirstName(firstName);
            List<User> users = entities.Select(entity => entity.ToModel()).ToList();

            return this.Ok(users);
        }

        /// <summary>
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsersByLastName(string lastName)
        {
            IEnumerable<UserEntity> entities = this._taskBerry.UsersRepository.GetUsersByLastName(lastName);
            List<User> users = entities.Select(entity => entity.ToModel()).ToList();

            return this.Ok(users);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<User> GetUserById(int id)
        {
            UserEntity entity = this._taskBerry.UsersRepository.GetUserById(id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            return this.Ok(entity.ToModel());
        }
    }
}