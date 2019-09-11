namespace TaskBerry.Shared.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;


    /// <summary>
    /// Represents a database entity for the groups.
    /// </summary>
    [Table("Group")]
    public class GroupEntity
    {
        /// <summary>
        /// Id of the group.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of the group.
        /// </summary>
        public string Description { get; set; }
    }
}