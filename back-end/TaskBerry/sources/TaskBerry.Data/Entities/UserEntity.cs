﻿namespace TaskBerry.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    using TaskBerry.Data.Models;


    /// <summary>
    /// User entity.
    /// </summary>
    [Table("User")]
    public class UserEntity
    {
        /// <inheritdoc cref="User"/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <inheritdoc cref="User"/>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <inheritdoc cref="User"/>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        /// <inheritdoc cref="User"/>
        [Required]
        [MaxLength(62)]
        public string Email { get; set; }
    }
}