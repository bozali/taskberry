using Microsoft.EntityFrameworkCore.Internal;

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
    [Route("/api/groups")]
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
        [HttpGet("/api/groups")]
        [Authorize(Roles = Roles.Admin)]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully returned models.")]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            if (!this.User.IsInRole(Roles.Admin))
            {
                return this.Forbid();
            }

            IEnumerable<GroupEntity> groupEntities = this._taskBerry.GroupsRepository.GetGroups();

            List<Group> groups = new List<Group>();

            foreach (GroupEntity groupEntity in groupEntities)
            {
                IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.Context.GroupAssignments.Where(a => a.GroupId == groupEntity.Id);

                Group group = groupEntity.ToModel();
                group.Members = new List<int>();

                foreach (GroupAssignmentEntity assignmentEntity in assignments)
                {
                    group.Members.Add(assignmentEntity.UserId);
                }

                groups.Add(group);
            }

            return this.Ok(groups);
        }

        /// <summary>
        /// Gets all groups of the current logged in user.
        /// </summary>
        /// <returns>Returns all groups of the current logged in user.</returns>
        [Authorize]
        [HttpGet("/api/groups/current-user-groups")]
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

            IEnumerable<GroupEntity> groupEntities = this._taskBerry.GroupsRepository.GetGroupsByUserId(currentUserId);

            List<Group> groups = new List<Group>();

            foreach (GroupEntity groupEntity in groupEntities)
            {
                IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.Context.GroupAssignments.Where(a => a.GroupId == groupEntity.Id);

                Group group = groupEntity.ToModel();
                group.Members = new List<int>();

                foreach (GroupAssignmentEntity assignmentEntity in assignments)
                {
                    group.Members.Add(assignmentEntity.UserId);
                }

                groups.Add(group);
            }

            return this.Ok(groups);
        }

        /// <summary>
        /// Creating a new group.
        /// </summary>
        /// <param name="group">New group to save in the database.</param>
        /// <returns>Returns the new created group.</returns>
        /// <remarks>For the property <see cref="Group.Members"/> only the ids of the user can be set.</remarks>
        [Authorize]
        [HttpPost("/api/groups/new")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returned successfully model.")]
        public ActionResult<Group> CreateGroup([FromBody] Group group)
        {
            // TODO Create services or repositories to create this

            GroupEntity entity = new GroupEntity
            {
                Id = Guid.NewGuid(),
                Name = group.Name,
                Description = group.Description ?? ""
            };

            this._taskBerry.GroupsRepository.CreateGroup(entity);

            if (group.Members != null)
            {
                foreach (int memberId in group.Members)
                {
                    this._taskBerry.Context.GroupAssignments.Add(new GroupAssignmentEntity { GroupId = entity.Id, UserId = memberId });
                }
            }

            this._taskBerry.Context.SaveChanges();

            return this.Ok(entity.ToModel()); // TODO CreateResult?
        }
    }
}