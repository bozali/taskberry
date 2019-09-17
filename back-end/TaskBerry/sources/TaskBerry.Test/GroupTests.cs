namespace TaskBerry.Test
{
    using TaskBerry.Business.Services;

    using NUnit.Framework;


    public class GroupTests
    {
        [Test]
        public void Test_Groups()
        {
            IGroupsService service = new GroupsService(null); // Create mock for ITaskBerryUnitOfWork

            Assert.Pass();
        }
    }
}