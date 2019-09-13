namespace TaskBerry.Service.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;


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
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsersByFirstName(string firstName)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsersByLastName(string lastName)
        {
            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUserById(int id)
        {
            return this.Ok();
        }
    }
}