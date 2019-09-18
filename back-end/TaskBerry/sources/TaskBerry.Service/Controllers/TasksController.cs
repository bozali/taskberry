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


    /// <summary>
    /// </summary>
    [ApiController]
    [Route("/api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskBerryUnitOfWork _taskBerry;

        /// <summary>
        /// </summary>
        public TasksController(ITaskBerryUnitOfWork taskBerry)
        {
            this._taskBerry = taskBerry;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/api/tasks/move")]
        [Produces("application/json")]
        public IActionResult MoveTask(Guid taskId, TaskStatus status, int row)
        {
            // TODO Check if the user has access rights to edit the task

            TaskEntity taskEntity = this._taskBerry.Context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not fnd {taskId}.");
            }

            taskEntity.Row = row;
            this._taskBerry.Context.SaveChanges();

            return this.Ok();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/tasks/current-user")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Task>> GetCurrentUserTasks()
        {
            if (this.User == null)
            {
                return this.Forbid("No user is logged in.");
            }

            IEnumerable<TaskEntity> userTasks = this._taskBerry.Context.Tasks.Where(t => t.Type == TaskType.User);

            int userId = int.Parse(this.User.Identity.Name);
            IEnumerable<TaskEntity> tasks = userTasks.Where(t => t.AssigneeId == userId);

            return this.Ok(tasks.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/tasks/tasks-from-group")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Task>> GetTasksFromGroup(Guid groupId)
        {
            // Check if group exists
            if (!this._taskBerry.Context.Groups.Any(entity => entity.Id == groupId))
            {
                return this.NotFound($"Could not find {groupId}.");
            }

            // Check if user is member of the group
            int userId = int.Parse(this.User.Identity.Name);

            if (!this._taskBerry.Context.GroupAssignments.Any(assignment => assignment.GroupId == groupId && assignment.UserId == userId))
            {
                return this.Forbid($"User {userId} is not member of the group.");
            }

            // Get all group tasks and than the tasks where the OwnerId equals groupId
            IEnumerable<TaskEntity> taskEntities = this._taskBerry.Context.Tasks
                .Where(t => t.Type == TaskType.Group)
                .Where(gt => gt.OwnerId.Equals(groupId.ToString(), StringComparison.InvariantCultureIgnoreCase));

            return this.Ok(taskEntities.Select(entity => entity.ToModel()));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/api/tasks/new")]
        [Produces("application/json")]
        public ActionResult<Task> CreateTask([FromBody] Task newTask)
        {
            // TODO Use automapper
            TaskEntity taskEntity = new TaskEntity
            {
                Id = Guid.NewGuid(),
                OwnerId = newTask.OwnerId,
                Description = newTask.Description,
                Type = newTask.Type,
                AssigneeId = newTask.AssigneeId,
                Row = newTask.Row,
                Status = TaskStatus.Open,
                Title = newTask.Title
            };

            this._taskBerry.Context.Tasks.Add(taskEntity);

            this._taskBerry.Context.SaveChanges();

            return this.Ok(taskEntity);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/api/tasks/assign")]
        [Produces("application/json")]
        public ActionResult<Task> AssignTaskToUser(Guid taskId, int user)
        {
            TaskEntity taskEntity = this._taskBerry.Context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not find {taskId}.");
            }

            if (taskEntity.Type == TaskType.User)
            {
                return this.BadRequest("Cannot assign user task to another users.");
            }

            taskEntity.AssigneeId = user;

            this._taskBerry.Context.SaveChanges();

            return this.Ok(taskEntity.ToModel());
        }

        /// <summary>
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("/api/tasks/delete")]
        public IActionResult DeleteTask(Guid taskId)
        {
            TaskEntity taskEntity = this._taskBerry.Context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskEntity == null)
            {
                return this.NotFound($"Could not find {taskId}.");
            }

            this._taskBerry.Context.Tasks.Remove(taskEntity);
            this._taskBerry.Context.SaveChanges();

            return this.Ok($"Successfully deleted {taskId}");
        }
    }
}