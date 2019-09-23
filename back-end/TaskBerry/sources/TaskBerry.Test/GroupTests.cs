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
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            this._mapper = new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<GroupEntity, Group>();
                config.CreateMap<Group, GroupEntity>();
            }));
        }

        [Test]
        [TestCase("Group Name", "")]
        [TestCase("Group Name", "Group Description")]
        public void Test_CreateGroup_With_GroupName_And_GroupDescription_And_Empty_GroupDescription(string groupName, string groupDescription)
        {
            Group group = new Group
            {
                Name = groupName,
                Description = groupDescription
            };

            using (AutoMock mock = AutoMock.GetLoose())
            {
                mock.Provide(this._mapper);

                Mock<IGroupRepository> groupRepository = mock.Mock<IGroupRepository>();
                groupRepository.Setup(x => x.CreateGroup(It.IsAny<GroupEntity>())).Returns(this._mapper.Map<GroupEntity>(group));

                Mock<ITaskBerryUnitOfWork> unitOfWork = mock.Mock<ITaskBerryUnitOfWork>();
                unitOfWork
                    .SetupProperty(p => p.GroupsRepository)
                    .SetupGet(x => x.GroupsRepository)
                    .Returns(groupRepository.Object);

                IGroupsService service = mock.Create<GroupsService>();
                Group newGroup = service.CreateGroup(group);

                Assert.IsTrue(!Guid.Empty.Equals(newGroup.Id));
                Assert.IsTrue(newGroup.Name == groupName);
                Assert.IsTrue(newGroup.Description == groupDescription);
            }
        }

        [Test]
        [TestCase("", "Group Description")]
        public void Test_CreateGroup_With_Empty_GroupName_And_GroupDescription(string groupName, string groupDescription)
        {
            Group group = new Group
            {
                Name = groupName,
                Description = groupDescription
            };

            using (AutoMock mock = AutoMock.GetLoose())
            {
                mock.Provide(this._mapper);

                IGroupsService service = mock.Create<GroupsService>();
                Assert.Throws<ArgumentException>(delegate { service.CreateGroup(group); });
            }
        }

        [Test]
        [TestCase("", "")]
        public void Test_CreateGroup_With_Empty_GroupName_And_Empty_GroupDescription(string groupName, string groupDescription)
        {
            Group group = new Group
            {
                Name = groupName,
                Description = groupDescription
            };

            using (AutoMock mock = AutoMock.GetLoose())
            {
                mock.Provide(this._mapper);

                IGroupsService service = mock.Create<GroupsService>();
                Assert.Throws<ArgumentException>(delegate { service.CreateGroup(group); });
            }
        }

        [Test]
        public void Test_CreateGroup_With_Null_GroupParameter()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                mock.Provide(this._mapper);

                IGroupsService service = mock.Create<GroupsService>();
                Assert.Throws<ArgumentNullException>(delegate { service.CreateGroup(null); });
            }
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

    }
}