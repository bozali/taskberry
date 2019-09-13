namespace TaskBerry.DataAccess.Repositories
{
    using System.Collections.Generic;

    using TaskBerry.Data.Entities;


    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetUsers();

        UserEntity GetUserById(int id);
    }
}