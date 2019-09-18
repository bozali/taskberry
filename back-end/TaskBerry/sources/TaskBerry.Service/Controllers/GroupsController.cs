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
    using TaskBerry.Business.Services;


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("/api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;
        private readonly IGroupsService _groupsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupsController(ITaskBerryUnitOfWork taskBerry, IGroupsService groupsService)
        {
            this._groupsService = groupsService;
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

            IEnumerable<Group> groups = this._groupsService.GetGroups();

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
        [Authorize(Roles = Roles.Admin)]
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

            return this.Ok(entity.ToModel()); // TODO CreateResult?
        }

        /// <summary>
        /// </summary>
        /// <param name="users"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("/api/groups/assign")]
        [Produces("application/json")]
        public ActionResult<Group> AssignUsersToGroup(int[] users, Guid groupId)
        {
            Group group = this._groupsService.AssignUsersToGroup(users, groupId);

            if (group == null)
            {
                return this.NotFound($"Could not find {groupId}.");
            }

            return this.Ok(group);
        }

        /// <summary>
        /// </summary>
        /// <param name="users"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/groups/remove-user-from-group")]
        [Produces("application/json")]
        public ActionResult<Group> RemoveUsersFromGroup(int[] users, Guid groupId)
        {
            GroupEntity groupEntity = this._taskBerry.GroupsRepository.GetGroups().FirstOrDefault(g => g.Id == groupId);

            if (groupEntity == null)
            {
                return this.NotFound($"Group {groupId} not found.");
            }

            // TODO Make the member assignment bitiful
            Group group = groupEntity.ToModel();
            group.Members = new List<int>();

            IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.Context.GroupAssignments;

            foreach (int userId in users)
            {
                // TODO Make this bitiful
                // Get assignments with the userId and the groupId
                GroupAssignmentEntity toRemove = assignments.FirstOrDefault(a => a.GroupId == groupId && a.UserId == userId);

                this._taskBerry.Context.GroupAssignments.Remove(toRemove);
            }

            foreach (GroupAssignmentEntity assignment in this._taskBerry.Context.GroupAssignments)
            {
                group.Members.Add(assignment.UserId);
            }

            // TODO Check if saved successfully
            this._taskBerry.Context.SaveChanges();

            return this.Ok(group);
        }

        /// <summary>
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/groups/delete")]
        [Produces("application/json")]
        public IActionResult DeleteGroup(Guid groupId)
        {
            GroupEntity groupEntity = this._taskBerry.Context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (groupEntity == null)
            {
                return this.NotFound($"Could not find {groupId}.");
            }

            IEnumerable<GroupAssignmentEntity> assignments = this._taskBerry.Context.GroupAssignments.Where(a => a.GroupId == groupId);
            this._taskBerry.Context.GroupAssignments.RemoveRange(assignments);
            this._taskBerry.Context.Groups.Remove(groupEntity);

            this._taskBerry.Context.SaveChanges();

            return this.Ok($"Successfully deleted {groupId}");
        }

        /// <summary>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("/api/groups/edit")]
        [Produces("application/json")]
        public ActionResult<Group> EditGroup([FromBody] Group group)
        {
            GroupEntity entity = this._taskBerry.Context.Groups.FirstOrDefault(g => g.Id == group.Id);

            if (entity == null)
            {
                return this.NotFound($"Could not find {group.Id}.");
            }

            entity.Name = group.Name;
            entity.Description = group.Description ?? "";

            this._taskBerry.Context.SaveChanges();

            return this.Ok(entity.ToModel());
        }
    }
}