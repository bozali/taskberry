namespace TaskBerry.Service.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    /// <summary>
    /// </summary>
    [ApiController]
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
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        public IActionResult MoveTask()
        {
            return this.Ok();
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        public IActionResult AssignTaskToUser()
        {
            return this.Ok();
        }
    }
}