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
    using System.Security.Cryptography;
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
        public ActionResult<User> Login(string email, string password)
        {
            UserEntity entity = this._taskBerry.UsersRepository.GetUsers().FirstOrDefault(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null)
            {
                return this.NotFound(email);
            }

            UserInfoEntity userInfoEntity = this._taskBerry.Context.UserInfos.FirstOrDefault(info => info.UserId == entity.Id);

            if (userInfoEntity == null)
            {
                return this.BadRequest($"User with the email {email} is not registered.");
            }

            using (MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes(password));
                string hashedPassword = Encoding.ASCII.GetString(bytes);

                if (hashedPassword != userInfoEntity.Password)
                {
                    return this.BadRequest($"Wrong password.");
                }
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
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("/api/authentication/register")]
        [Produces("application/json")]
        public IActionResult Register(string email, string password)
        {

            UserEntity userEntity = this._taskBerry.Context.Users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (userEntity == null)
            {
                return this.NotFound($"User with the email {email} does not exist.");
            }

            if (this._taskBerry.Context.UserInfos.Any(info => info.UserId == userEntity.Id))
            {
                return this.BadRequest($"User with the email {email} already exists.");
            }

            using (MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes(password));
                string hashedPassword = Encoding.ASCII.GetString(bytes);

                UserInfoEntity entity = new UserInfoEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userEntity.Id,
                    Password = hashedPassword
                };

                this._taskBerry.Context.UserInfos.Add(entity);
                this._taskBerry.Context.SaveChanges();
            }

            return this.Ok($"Successfully registered {email}.");
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("/api/authentication/logout")]
        public IActionResult Logout()
        {
            return this.Ok();
        }
    }
}