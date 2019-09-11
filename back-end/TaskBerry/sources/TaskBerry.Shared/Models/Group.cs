namespace TaskBerry.Shared.Models
{
    using TaskBerry.Shared.Entities;

    using System;


    /// <summary>
    /// Group model.
    /// </summary>
    public class Group
    {
        /// <inheritdoc cref="GroupEntity"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="GroupEntity"/>
        public string Name { get; set; }

        /// <inheritdoc cref="GroupEntity"/>
        public string Description { get; set; }
    }
}