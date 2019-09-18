namespace TaskBerry.Test
{
    using TaskBerry.DataAccess.Repositories;
    using TaskBerry.DataAccess.Domain;

    using TaskBerry.Business.Services;
    using TaskBerry.Data.Entities;

    using Autofac.Extras.Moq;

    using NUnit.Framework;

    using Moq.Language.Flow;

    using System.Collections.Generic;
    using System;
    using Moq;

    public class GroupTests
    {
        [Test]
        public void Test_Groups()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IGroupRepository> groupsRepositoryMock = mock.Mock<IGroupRepository>();
                groupsRepositoryMock
                    .Setup(x => x.GetGroups())
                    .Returns(new List<GroupEntity>
                    {
                        new GroupEntity { Id = Guid.NewGuid(), Name = "Test Name 1", Description = "Test Description 1" },
                        new GroupEntity { Id = Guid.NewGuid(), Name = "Test Name 2", Description = "Test Description 2" },
                        new GroupEntity { Id = Guid.NewGuid(), Name = "Test Name 3", Description = "Test Description 3" },
                    });

                mock.Mock<ITaskBerryUnitOfWork>()
                    .SetupProperty(p => p.GroupsRepository)
                    .SetupGet(x => x.GroupsRepository)
                    .Returns(groupsRepositoryMock.Object);

                IGroupsService service = mock.Create<GroupsService>(); // Create mock for ITaskBerryUnitOfWork

                service.GetGroups();

                Assert.Pass();
            }
        }
    }
}