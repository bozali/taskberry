namespace TaskBerry.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System;

    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetUsers();

        UserEntity GetUserById(int id);

        IEnumerable<User> GetUsersByGroupId(Guid groupId);
    }
}