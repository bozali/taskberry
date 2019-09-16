namespace TaskBerry.Service.Controllers
{
    using Swashbuckle.AspNetCore.Annotations;

    using TaskBerry.Service.Constants;
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupsController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            if (!this.User.IsInRole(Roles.Admin))
            {
                return this.Forbid();
            }

            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroups();
            
            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// Gets all groups of the current logged in user.
        /// </summary>
        /// <returns>Returns all groups of the current logged in user.</returns>
        [Authorize]
        [HttpGet("/currentUserGroups")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "No user is logged in.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returns all groups of the current user")]
        public ActionResult<IEnumerable<Group>> GetCurrentUserGroups()
        {
            if (this.User == null)
            {
                return this.BadRequest("No user is logged in.");
            }

            int currentUserId = int.Parse(this.User.Identity.Name);

            IEnumerable<GroupEntity> entities = this._taskBerry.GroupsRepository.GetGroupsByUserId(currentUserId);

            return this.Ok(entities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/new")]
        [Produces("application/json")]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            // TODO Create services or repositories to create this

            GroupEntity entity = new GroupEntity
            {
                Id = Guid.NewGuid(),
                Name = group.Name,
                Description = group.Description
            };

            this._taskBerry.GroupsRepository.CreateGroup(entity);

            foreach (int member in group.Members)
            {
                this._taskBerry.Context.GroupAssignments.Add(new GroupAssignmentEntity { GroupId = entity.Id, UserId = member });
            }

            this._taskBerry.Context.SaveChanges();

            return this.Ok(entity.ToModel()); // TODO CreateResult?
        }
    }
}