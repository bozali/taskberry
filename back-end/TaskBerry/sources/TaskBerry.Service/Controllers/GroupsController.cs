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
    /// Controller for group functions.
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
        /// Returns all groups in database.
        /// </summary>
        /// <returns>Returns all groups in database.</returns>
        /// <response code="200">Successfully returned groups.</response>
        /// <response code="403">User is not in the <see cref="Roles.Admin"/> to access the request.</response>
        [HttpGet("/api/groups")]
        [Authorize(Roles = Roles.Admin)]
        [Produces("application/json")]
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
        /// <response code="400">No user is logged in.</response>
        /// <response code="200">Returns all groups of the current user.</response>
        [Authorize]
        [HttpGet("/api/groups/current-user-groups")]
        [Produces("application/json")]
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
        /// <response code="200">Created successfully the group and returned the result.</response>
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
        /// Assigns a users to a specific group.
        /// </summary>
        /// <param name="users">List of the user ids.</param>
        /// <param name="groupId">The group where the users will be assigned to.</param>
        /// <returns>Returns the group with the assigned users.</returns>
        /// <response code="404">The group with the id could not be found.</response>
        /// <response code="200">Successfully assigned the users to the group.</response>
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
        /// Removes users from a group.
        /// </summary>
        /// <param name="users">List of user ids that will be removed from the group.</param>
        /// <param name="groupId">The group id where the users will be removed.</param>
        /// <returns>Returns the group with the removed users.</returns>
        /// <response code="404">The group could not be found.</response>
        /// <response code="200">Users were successfully removed from the group.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("/api/groups/remove-user-from-group")]
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
        /// Deletes a group.
        /// </summary>
        /// <param name="groupId">The group to delete.</param>
        /// <returns>Returns the result of the request.</returns>
        /// <response code="404">Could not find the group.</response>
        /// <response code="200">Successfully deleted the group.</response>
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
        /// Edits a group with its properties.
        /// </summary>
        /// <remarks>Members will be just ignored.</remarks>
        /// <param name="group">The edited group.</param>
        /// <returns>Returns the edited group.</returns>
        /// <response code="404">Group could not be found.</response>
        /// <response code="200">Successfully edited the group.</response>
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