namespace TaskBerry.Service.Controllers
{
    using TaskBerry.Service.Configuration;
    using TaskBerry.Service.Constants;
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;


    using Swashbuckle.AspNetCore.Annotations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Linq;
    using System;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("/api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// </summary>
        /// <param name="taskBerry"></param>
        /// <param name="configurationProvider"></param>
        public AuthenticationController(ITaskBerryUnitOfWork taskBerry, IConfigurationProvider configurationProvider)
        {
            this._configurationProvider = configurationProvider;
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// Login the user.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">User with the email not found</response>
        /// <response code="200">User email found and user is logged in</response>
        [HttpPost("/api/authentication/login")]
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
            byte[] key = Encoding.ASCII.GetBytes(this._configurationProvider.GetTokenConfiguration().Secret);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, entity.Id.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, Roles.Admin), // TODO Check if user is admin
                }),
                Expires = DateTime.UtcNow.AddMinutes(15.0),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            // TODO Change this with AutoMapper
            User user = entity.ToModel();
            user.Token = "Bearer " + tokenHandler.WriteToken(token);

            return this.Ok(user);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/authentication/logout")]
        public IActionResult Logout()
        {
            return this.Ok();
        }
    }
}