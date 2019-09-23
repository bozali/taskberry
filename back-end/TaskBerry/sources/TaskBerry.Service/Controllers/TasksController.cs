namespace TaskBerry.Service.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TaskBerry.Service.Constants;
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System;

    using AutoMapper;


    /// <summary>
    /// Controller for task functions.
    /// </summary>
    [ApiController]
    [Route("/api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TasksController(ITaskBerryUnitOfWork taskBerry, IMapper mapper)
        {
            this._taskBerry = taskBerry;
            this._mapper = mapper;
        }

        /// <summary>
        /// Moves a task to a new status or/and row.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status">The new status of the task. Can be null.</param>
        /// <param name="row">The new row of the task. Can be null.</param>
        /// <returns>Returns the result of the request.</returns>
        /// <response code="404">Task could not be found.</response>
        /// <response code="200">Successfully moved the task.</response>
        [Authorize]
        [HttpPost("/api/tasks/move")]
        [Produces("application/json")]
        public IActionResult MoveTask(Guid taskId, TaskStatus? status, int? row)
        {
            // TODO Check if the user has access rights to edit the task

            TaskEntity taskEntity = this._taskBerry.TaskBerryContext.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not fnd {taskId}.");
            }

            taskEntity.Status = status ?? taskEntity.Status;
            taskEntity.Row = row ?? taskEntity.Row;
            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok("Successfully moved the task.");
        }

        /// <summary>
        /// Updates the Task title
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="newTaskTitle">The newTaskTitle of the task. Can be null.</param>
        /// <returns>Returns the result of the request.</returns>
        /// <response code="404">Task could not be updated.</response>
        /// <response code="200">Successfully updated the task.</response>
        [Authorize]
        [HttpPost("/api/tasks/update/title")]
        [Produces("application/json")]
        public IActionResult EditTaskTitle(Guid taskId, string newTaskTitle)
        {
            // TODO Check if the user has access rights to edit the task

            TaskEntity taskEntity = this._taskBerry.TaskBerryContext.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not fnd {taskId}.");
            }

            taskEntity.Title = newTaskTitle ?? taskEntity.Title;
        
            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok("Successfully edited the task.");
        }

        /// <summary>
        /// Updates the Task description
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="newTaskDescription">The new TaskDescription of the task. Can not be null.</param>
        /// <returns>Returns the result of the request.</returns>
        /// <response code="404">Task could not be updated.</response>
        /// <response code="200">Successfully updated the task.</response>
        [Authorize]
        [HttpPost("/api/tasks/update/description")]
        [Produces("application/json")]
        public IActionResult EditTaskDescription(Guid taskId, string newTaskDescription)
        {
            // TODO Check if the user has access rights to edit the task

            TaskEntity taskEntity = this._taskBerry.TaskBerryContext.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not fnd {taskId}.");
            }

            taskEntity.Description = newTaskDescription == "" ? taskEntity.Description : newTaskDescription;
            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok("Successfully edited the task.");
        }

        /// <summary>
        /// Gets all the users tasks of the current user.
        /// </summary>
        /// <remarks>This does not return the tasks that are assigned to the user. This methods
        /// returns the tasks where the user is the owner of the task.
        /// </remarks>
        /// <returns>Returns user tasks of the current user.</returns>
        /// <response code="200">Successfully got the user tasks of the current user.</response>
        [Authorize]
        [HttpGet("/api/tasks/current-user")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Task>> GetCurrentUserTasks()
        {
            int userId = int.Parse(this.User.Identity.Name);
            IEnumerable<TaskEntity> userTasks = this._taskBerry.TaskBerryContext.Tasks.Where(t => t.Type == TaskType.User && t.OwnerId == userId.ToString());
     
            //IEnumerable<TaskEntity> tasks = userTasks.Where(t => t.AssigneeId == userId);

            return this.Ok(userTasks.Select(this._mapper.Map<Task>));
        }



        /// <summary>
        /// Gets all tasks from the group.
        /// </summary>
        /// <param name="groupId">The group id to get the tasks.</param>
        /// <returns>Returns the tasks of the group.</returns>
        /// <response code="404">Could not find the group.</response>
        /// <response code="403">User is not member of the group.</response>
        /// <response code="200">Successfully returned all the tasks from the group.</response>
        [Authorize]
        [HttpGet("/api/tasks/tasks-from-group")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Task>> GetTasksFromGroup(Guid groupId)
        {
            // Check if group exists
            if (!this._taskBerry.TaskBerryContext.Groups.Any(entity => entity.Id == groupId))
            {
                return this.NotFound($"Could not find {groupId}.");
            }

            // Check if user is member of the group
            int userId = int.Parse(this.User.Identity.Name);

            if (!this._taskBerry.TaskBerryContext.GroupAssignments.Any(assignment => assignment.GroupId == groupId && assignment.UserId == userId))
            {
                return this.Forbid($"User {userId} is not member of the group.");
            }

            // Get all group tasks and than the tasks where the OwnerId equals groupId
            IEnumerable<TaskEntity> taskEntities = this._taskBerry.TaskBerryContext.Tasks
                .Where(t => t.Type == TaskType.Group)
                .Where(gt => gt.OwnerId.Equals(groupId.ToString(), StringComparison.InvariantCultureIgnoreCase));

            return this.Ok(taskEntities.Select(this._mapper.Map<Task>));
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="newTask">The new task.</param>
        /// <returns>Returns the new created task.</returns>
        /// <response code="200">Successfully created the task.</response>
        [Authorize]
        [HttpPost("/api/tasks/new")]
        [Produces("application/json")]
        public ActionResult<Task> CreateTask([FromBody] Task newTask)
        {
            // TODO Check if the user is allowed to create the task at newTask.GroupId (OwnerId)
            // TODO Check if the row with the same status and the same group is already in use by another task

            TaskEntity taskEntity = this._mapper.Map<TaskEntity>(newTask);
            taskEntity.Id = Guid.NewGuid();

            this._taskBerry.TaskBerryContext.Tasks.Add(taskEntity);

            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok(taskEntity);
        }

        /// <summary>
        /// Assigns a task to a user.
        /// </summary>
        /// <param name="taskId">The task id.</param>
        /// <param name="user">The user id.</param>
        /// <returns>Returns the assigned task.</returns>
        /// <response code="404">Could not find the task.</response>
        /// <response code="200">Successfully assigned task to user.</response>
        /// <response code="400">User is not part of the group or could not assign a user task to another user <see cref="TaskType.User"/>.</response>
        [Authorize]
        [HttpPost("/api/tasks/assign")]
        [Produces("application/json")]
        public ActionResult<Task> AssignTaskToUser(Guid taskId, int user)
        {
            TaskEntity taskEntity = this._taskBerry.TaskBerryContext.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not find {taskId}.");
            }

            if (taskEntity.Type == TaskType.User)
            {
                return this.BadRequest("Cannot assign user task to another users.");
            }

            // Check if user is in the group where the task is located at.
            IEnumerable<GroupAssignmentEntity> assignmentEntities = this._taskBerry.TaskBerryContext.GroupAssignments;

            Guid groupId = Guid.Parse(taskEntity.OwnerId);
            if (!assignmentEntities.Any(assignment => assignment.GroupId == groupId && assignment.UserId == user))
            {
                return this.BadRequest("User is not part of the group.");
            }

            taskEntity.AssigneeId = user;

            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok(this._mapper.Map<Task>(taskEntity));
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="taskId">The task id.</param>
        /// <returns>Returns the result of the response.</returns>
        /// <response code="404">Could not find the task.</response>
        /// <response code="200">Successfully deleted the task.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/tasks/delete")]
        public IActionResult DeleteTask(Guid taskId)
        {
            TaskEntity taskEntity = this._taskBerry.TaskBerryContext.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not find {taskId}.");
            }

            this._taskBerry.TaskBerryContext.Tasks.Remove(taskEntity);
            this._taskBerry.TaskBerryContext.SaveChanges();

            return this.Ok( new { result = string.Format("Die Aufgabe wurde erfolgreich entfernt.") });
        }
    }
}