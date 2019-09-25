namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;


    /// <summary>
    /// Represents a database entity for the group assignments.
    /// </summary>
    [Table("GroupAssignment")]
    public class GroupAssignmentEntity
    {
        /// <inheritdoc cref="Models.Group"/>
        [Key]
        public Guid Id { get; set; }

        /// <inheritdoc cref="Models.Group"/>
        public int UserId { get; set; }

        /// <inheritdoc cref="Models.Group"/>
        [MaxLength(36)]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }

        /// <inheritdoc cref="Models.Group"/>
        public GroupEntity Group { get; set; }
    }
}