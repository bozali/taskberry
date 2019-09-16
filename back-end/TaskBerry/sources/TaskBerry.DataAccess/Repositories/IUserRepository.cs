namespace TaskBerry.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System;

    using TaskBerry.Data.Entities;


    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetUsers();

        UserEntity GetUserById(int id);

        IEnumerable<UserEntity> GetUsersByGroupId(Guid groupId);
    }
}