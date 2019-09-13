namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    using TaskBerry.Data.Models;


    /// <summary>
    /// User entity.
    /// </summary>
    public class UserEntity : EntityBase<User>
    {
        /// <inheritdoc cref="User"/>
        [Key]
        public int Id { get; set; }

        /// <inheritdoc cref="User"/>
        public string FirstName { get; set; }

        /// <inheritdoc cref="User"/>
        public string LastName { get; set; }

        /// <inheritdoc cref="User"/>
        public string Email { get; set; }
    }
}