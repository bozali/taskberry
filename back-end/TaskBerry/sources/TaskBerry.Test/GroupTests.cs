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
    using System.Linq;
    using System;

    using AutoMapper;


    public class GroupTests
    {
        [Test]
        public void Test_GetGroups_Without_User_Assignments()
        {
        }

        [Test]
        public void Test_GetGroups_With_User_Assignments()
        {
            List<GroupEntity> entities = new List<GroupEntity>
            {
                new GroupEntity {Id = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), Name = "Test Name 1", Description = "Test Description 1"},
                new GroupEntity {Id = Guid.Parse("495ABCC4-A310-41CE-A730-8A6692347F4D"), Name = "Test Name 2", Description = "Test Description 2"},
                new GroupEntity {Id = Guid.Parse("E3315CB8-0CAE-452F-9A67-2DF46ABF2AA8"), Name = "Test Name 3", Description = "Test Description 3"},
            };

            List<GroupAssignmentEntity> assignments = new List<GroupAssignmentEntity>
            {
                new GroupAssignmentEntity { Id = Guid.NewGuid(), GroupId = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), UserId = 1},
                new GroupAssignmentEntity { Id = Guid.NewGuid(), GroupId = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), UserId = 2}
            };

            List<Group> should = new List<Group>
            {
                new Group {Id = Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85"), Name = "Test Name 1", Description = "Test Description 1", Members = new List<int> { 1, 2 }},
                new Group {Id = Guid.Parse("495ABCC4-A310-41CE-A730-8A6692347F4D"), Name = "Test Name 2", Description = "Test Description 2", Members = new List<int> { }},
                new Group {Id = Guid.Parse("E3315CB8-0CAE-452F-9A67-2DF46ABF2AA8"), Name = "Test Name 3", Description = "Test Description 3", Members = new List<int> { }},
            };

            using (AutoMock mock = AutoMock.GetLoose())
            {
                Mock<IGroupRepository> groupsRepositoryMock = mock.Mock<IGroupRepository>();
                Mock<IMapper> mapperMock = mock.Mock<IMapper>();
                mapperMock
                    .Setup(x => x.Map<Group>(It.Is<GroupEntity>(p => p.Id == entities[0].Id)))
                    .Returns(should[0]);

                mapperMock
                    .Setup(x => x.Map<Group>(It.Is<GroupEntity>(p => p.Id == entities[1].Id)))
                    .Returns(should[1]);

                mapperMock
                    .Setup(x => x.Map<Group>(It.Is<GroupEntity>(p => p.Id == entities[2].Id)))
                    .Returns(should[2]);

                groupsRepositoryMock
                    .Setup(x => x.GetGroupAssignment(Guid.Parse("DFFB361C-3A4F-4761-8365-58EA22CCDA85")))
                    .Returns(assignments);

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

                    Assert.NotNull(groups[i].Members);
                    Assert.AreEqual(groups[i].Members.Count, should[i].Members.Count);

                    // Check if member ids are same
                    CollectionAssert.AreEqual(groups[i].Members, should[i].Members);
                }
            }
        }

        [Test]
        public void Test_RemoveUsersFromGroup_Where_Users_Are_In_Group()
        {
        }

        [Test]
        public void Test_RemoveUsersFromGroup_Where_Users_Are_Not_In_Group()
        {
        }

        [Test]
        public void Test_EditGroup_Where_Group_Does_Not_Exist()
        {
        }

        [Test]
        public void Test_EditGroup_Where_Group_Does_Exist()
        {
        }

        [Test]
        public void Test_DeleteGroup_Where_Group_Does_Not_Exist()
        {
        }

        [Test]
        public void Test_DeleteGroup_Where_Group_Does_Exist()
        {
        }

        [Test]
        public void Test_AssignsUsersToGroup_Where_Group_Does_Not_Exist()
        {
        }

        [Test]
        public void Test_AssignUsersToGroup_Where_Users_Does_Not_Exist()
        {
        }

        [Test]
        public void Test_AssignUsersToGroup_Where_Users_And_Group_Exists()
        {
        }
    }
}