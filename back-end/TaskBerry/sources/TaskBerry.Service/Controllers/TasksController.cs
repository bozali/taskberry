namespace TaskBerry.Service.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("/api/tasks")]
    public class TasksController : ControllerBase
    {
        /// <summary>
        /// </summary>
        public TasksController()
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        // [Authorize]
        // [HttpPost("/api/tasks/move")]
        // [Produces("application/json")]
        // public IActionResult MoveTask(Guid taskId, int column, int row)
        // {
        //     return this.Ok();
        // }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        // [Authorize]
        // [HttpPost]
        // [Produces("application/json")]
        // public IActionResult AssignTaskToUser()
        // {
        //     return this.Ok();
        // }
    }
}