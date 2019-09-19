namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;

    using System.Collections.Generic;
    using System.Linq;
    using System;
    using TaskBerry.Data.Models;

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

        public IEnumerable<User> GetUsersByGroupId(Guid groupId)
        {
            List<GroupAssignmentEntity> assignments = this.TaskBerry.Context.GroupAssignments.Where(assignment => assignment.GroupId == groupId).Distinct().ToList();
            List<User> usersToAdd = new List<User>();
            foreach (var assignment in assignments)
                if(this.TaskBerry.Context.Users.Any(w=> w.Id == assignment.UserId))
                {
                    var assignedUser = this.TaskBerry.Context.Users.SingleOrDefault(w => w.Id == assignment.UserId);
                    usersToAdd.Add(assignedUser.ToModel());
                }
            return usersToAdd;
        }
    }
}