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

        public int UserId { get; set; }

        public Guid GroupId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [ForeignKey("GroupId")]
        public GroupEntity Group { get; set; }
    }
}