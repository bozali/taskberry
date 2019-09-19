namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;

    using TaskBerry.Data.Models;


    /// <summary>
    /// Represents a database entity for the groups.
    /// </summary>
    [Table("Group")]
    public class GroupEntity
    {

        /// <inheritdoc cref="Group"/>
        [Key]
        public Guid Id { get; set; }

        /// <inheritdoc cref="Group"/>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <inheritdoc cref="Group"/>
        public string Description { get; set; }
    }
}