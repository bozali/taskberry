namespace TaskBerry.Shared.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System;


    /// <summary>
    /// Represents a database entity for the tasks.
    /// </summary>
    [Table("Task")]
    public class TaskEntity
    {
        /// <summary>
        /// Id of the task.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the task.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Status of the task.
        /// </summary>
        [DefaultValue(TaskStatus.Open)]
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Type of the task.
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// The owner of a task can be user or a group.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// The id of the user that is assigned to this task.
        /// </summary>
        public Guid? AssigneeId { get; set; }
    }
}