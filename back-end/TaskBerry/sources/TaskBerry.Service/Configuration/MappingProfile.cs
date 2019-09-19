namespace TaskBerry.Service.Configuration
{
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using AutoMapper;


    /// <summary>
    /// AutoMapper profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<UserEntity, User>();
            this.CreateMap<GroupEntity, Group>();
            this.CreateMap<TaskEntity, Task>();

            this.CreateMap<User, UserEntity>();
            this.CreateMap<Group, GroupEntity>();
            this.CreateMap<Task, TaskEntity>();
        }
    }
}