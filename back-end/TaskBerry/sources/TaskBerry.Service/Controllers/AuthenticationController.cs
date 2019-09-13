namespace TaskBerry.Service.Controllers
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using Swashbuckle.AspNetCore.Annotations;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;
    using System;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// </summary>
        /// <param name="taskBerry"></param>
        public AuthenticationController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// Login the user.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">User with the email not found</response>
        /// <response code="200">User email found and user is logged in</response>
        [HttpPost("/login/")]
        [SwaggerResponse(404, "User with the email not found")]
        [SwaggerResponse(200, "User email found and user is logged in")]
        public IActionResult Login([FromBody] string email)
        {
            UserEntity entity = this._taskBerry.UsersRepository.GetUsers().FirstOrDefault(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null)
            {
                return this.NotFound(email);
            }

            // TODO Create JWT and return it

            return this.Ok(entity);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            return this.Ok();
        }
    }
}