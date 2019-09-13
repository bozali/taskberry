namespace TaskBerry.DataAccess.Repositories
{
    using System.Collections.Generic;

    using TaskBerry.Data.Entities;


    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetUsers();

        IEnumerable<UserEntity> GetUsersByFirstName(string firstName);

        IEnumerable<UserEntity> GetUsersByLastName(string lastName);

        UserEntity GetUserById(int id);
    }
}