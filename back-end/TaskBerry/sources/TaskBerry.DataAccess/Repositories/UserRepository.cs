namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System.Linq;
    using System;


    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(ITaskBerryUnitOfWork taskBerry) : base(taskBerry)
        {
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            return this.TaskBerry.Context.Users;
        }

        public UserEntity GetUserById(int id)
        {
            return this.TaskBerry.Context.Users.FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<UserEntity> GetUsersByGroupId(Guid groupId)
        {
            IEnumerable<GroupAssignmentEntity> assignments = this.TaskBerry.Context.GroupAssignments.Where(assignment => assignment.GroupId == groupId);
            return assignments.Select(entity => this.TaskBerry.Context.Users.FirstOrDefault())
        }
    }
}