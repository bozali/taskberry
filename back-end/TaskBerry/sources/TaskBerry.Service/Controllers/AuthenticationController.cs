namespace TaskBerry.Service.Controllers
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using Swashbuckle.AspNetCore.Annotations;

    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Mvc;

    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
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
        [HttpPost("/login")]
        [SwaggerResponse(404, "User with the email not found")]
        [SwaggerResponse(200, "User email found and user is logged in")]
        public ActionResult<User> Login(string email)
        {
            UserEntity entity = this._taskBerry.UsersRepository.GetUsers().FirstOrDefault(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null)
            {
                return this.NotFound(email);
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes("test1234567890andmore");

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10.0),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            // TODO Change this with AutoMapper
            User user = entity.ToModel();
            user.Token = tokenHandler.WriteToken(token);

            return this.Ok(user);
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