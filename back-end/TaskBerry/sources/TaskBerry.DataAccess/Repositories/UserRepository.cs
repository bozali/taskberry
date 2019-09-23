namespace TaskBerry.DataAccess.Repositories
{
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System;

    using AutoMapper;


    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(ITaskBerryUnitOfWork taskBerry, IMapper mapper) : base(taskBerry)
        {
            this._mapper = mapper;
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            return this.TaskBerry.MoodleContext.Users;
        }

        public UserEntity GetUserById(int id)
        {
            return this.TaskBerry.MoodleContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<User> GetUsersByGroupId(Guid groupId)
        {
            List<GroupAssignmentEntity> assignments = this.TaskBerry.TaskBerryContext.GroupAssignments.Where(assignment => assignment.GroupId == groupId).Distinct().ToList();
            List<User> usersToAdd = new List<User>();

            foreach (GroupAssignmentEntity assignment in assignments)
            {
                if (this.TaskBerry.MoodleContext.Users.Any(w => w.Id == assignment.UserId))
                {
                    UserEntity assignedUser = this.TaskBerry.MoodleContext.Users.SingleOrDefault(w => w.Id == assignment.UserId);
                    usersToAdd.Add(this._mapper.Map<User>(assignedUser));
                }
            }

            return usersToAdd;
        }
    }
}