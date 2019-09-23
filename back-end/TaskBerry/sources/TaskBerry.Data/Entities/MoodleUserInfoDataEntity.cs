namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;


    /// <summary>
    /// </summary>
    [Table("user_info_data")]
    public class MoodleUserInfoDataEntity
    {
        /// <summary>
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// </summary>
        public UserEntity User { get; set; }
    }
}