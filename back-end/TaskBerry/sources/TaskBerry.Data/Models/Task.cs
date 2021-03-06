﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TaskBerry.Data.Entities;

namespace TaskBerry.Data.Models
{
    /// <summary>
    /// </summary>
    public class Task
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
        public string OwnerId { get; set; }

        /// <summary>
        /// The id of the user that is assigned to this task.
        /// </summary>
        public int? AssigneeId { get; set; }

        /// <summary>
        /// </summary>
        public int Row { get; set; }
    }
}