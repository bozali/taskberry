namespace TaskBerry.Service.Controllers
{
    using TaskBerry.DataAccess.Domain;

    using Microsoft.AspNetCore.Mvc;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private ITaskBerryUnitOfWork _taskBerry;

        public AuthenticationController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IActionResult Login(string email)
        {


            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            return this.Ok();
        }
    }
}