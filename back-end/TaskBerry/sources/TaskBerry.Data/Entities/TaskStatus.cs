namespace TaskBerry.Data.Entities
{
    /// <summary>
    /// Representing three status(es) of the task.
    /// The status is representing the three columns in the Kanban-Board.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// Task is open.
        /// </summary>
        Open,

        /// <summary>
        /// Task is in progress.
        /// </summary>
        Progress,

        /// <summary>
        /// Task is done.
        /// </summary>
        Done
    }
}