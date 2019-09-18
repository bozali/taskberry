using System.Linq;

namespace TaskBerry.Test
{
    using TaskBerry.DataAccess.Repositories;
    using TaskBerry.DataAccess.Domain;
    using TaskBerry.Business.Services;
    using TaskBerry.Data.Entities;
    using TaskBerry.Data.Models;

    using Autofac.Extras.Moq;

    using NUnit.Framework;
    using Moq;

    using System.Collections.Generic;
    using System;


    public class GroupTests
    {
        [Test]
        public void Test_GetGroups()
        {
            List<GroupEntity> entities = new List<GroupEntity>
            {
                new GroupEntity {Id = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), Name = "Test Name 1", Description = "Test Description 1"},
                new GroupEntity {Id = Guid.Parse("495ABCC4-A310-41CE-A730-8A6692347F4D"), Name = "Test Name 2", Description = "Test Description 2"},
                new GroupEntity {Id = Guid.Parse("E3315CB8-0CAE-452F-9A67-2DF46ABF2AA8"), Name = "Test Name 3", Description = "Test Description 3"},
            };

            List<Group> should = new List<Group>
            {
                new Group {Id = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), Name = "Test Name 1", Description = "Test Description 1"},
                new Group {Id = Guid.Parse("495ABCC4-A310-41CE-A730-8A6692347F4D"), Name = "Test Name 2", Description = "Test Description 2"},
                new Group {Id = Guid.Parse("E3315CB8-0CAE-452F-9A67-2DF46ABF2AA8"), Name = "Test Name 3", Description = "Test Description 3"},
            };

            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IGroupRepository> groupsRepositoryMock = mock.Mock<IGroupRepository>();
                groupsRepositoryMock
                    .Setup(x => x.GetGroups())
                    .Returns(entities);

                mock.Mock<ITaskBerryUnitOfWork>()
                    .SetupProperty(p => p.GroupsRepository)
                    .SetupGet(x => x.GroupsRepository)
                    .Returns(groupsRepositoryMock.Object);

                IGroupsService service = mock.Create<GroupsService>();

                Group[] groups = service.GetGroups().ToArray();

                for (int i = 0; i < should.Count; ++i)
                {
                    Assert.AreEqual(groups[i].Id, should[i].Id);
                    Assert.AreEqual(groups[i].Name, should[i].Name);
                    Assert.AreEqual(groups[i].Description, should[i].Description);
                }

                Assert.Pass();
            }
        }
    }
}