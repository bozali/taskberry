namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;


    [Table("GroupAssignment")]
    public class GroupAssignmentEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [MaxLength(36)]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }

        public UserEntity User { get; set; }

        public GroupEntity Group { get; set; }
    }
}