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

        public IEnumerable<UserEntity> GetUsersByFirstName(string firstName)
        {
            return this.TaskBerry.Context.Users.Where(user => user.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<UserEntity> GetUsersByLastName(string lastName)
        {
            return this.TaskBerry.Context.Users.Where(user => user.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase));
        }

        public UserEntity GetUserById(int id)
        {
            return this.TaskBerry.Context.Users.FirstOrDefault(user => user.Id == id);
        }
    }
}