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
    }
}