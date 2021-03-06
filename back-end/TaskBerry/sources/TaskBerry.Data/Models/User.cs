﻿namespace TaskBerry.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;


    /// <summary>
    /// User model.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// Id of the user.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        [Required]
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        [Required]
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        [Required]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Jwt token.
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// Is Teacher?
        /// </summary>
        [DataMember(Name = "isTeacher")]
        public bool IsTeacher { get; set; }
    }
}