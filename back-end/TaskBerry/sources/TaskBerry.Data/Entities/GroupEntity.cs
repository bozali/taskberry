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
    public class GroupEntity : EntityBase<Group>
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }
    }
}