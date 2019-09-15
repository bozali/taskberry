namespace TaskBerry.Service.Controllers
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Collections.Generic;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class KanbanController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// </summary>
        public KanbanController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Task>> GetCurrentUserTasks()
        {
            return this.Ok();
        }
    }
}