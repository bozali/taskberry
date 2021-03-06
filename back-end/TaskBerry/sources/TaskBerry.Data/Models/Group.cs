﻿using System.Collections.Generic;

namespace TaskBerry.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System;


    /// <summary>
    /// Group model.
    /// </summary>
    [DataContract]
    public class Group
    {
        /// <summary>
        /// Id of the group.
        /// </summary>
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the group.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [DataMember(Name = "members")]
        public List<int> Members { get; set; }
    }
}