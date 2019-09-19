namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;


    /// <summary>
    /// </summary>
    [Table("UserInfo")]
    public class UserInfoEntity
    {
        /// <summary>
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        public UserEntity User { get; set; }
    }
}