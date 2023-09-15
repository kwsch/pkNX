using Xunit;

namespace SharpFileSystem.Tests.FileSystems
{

    public class EntityMoverRegistrationTest
    {
        private TypeCombinationDictionary<IEntityMover> Registration;
        private IEntityMover physicalEntityMover = new PhysicalEntityMover();
        private IEntityMover standardEntityMover = new StandardEntityMover();

        public EntityMoverRegistrationTest()
        {
            Registration = new TypeCombinationDictionary<IEntityMover>();
            Registration.AddLast(typeof(PhysicalFileSystem), typeof(PhysicalFileSystem), physicalEntityMover);
            Registration.AddLast(typeof(IFileSystem), typeof(IFileSystem), standardEntityMover);
        }

        [Fact]
        public void When_MovingFromPhysicalToGenericFileSystem_Expect_StandardEntityMover()
        {
            Assert.Equal(
                Registration.GetSupportedRegistration(typeof(PhysicalFileSystem), typeof(IFileSystem)).Value,
                standardEntityMover
                );
        }

        [Fact]
        public void When_MovingFromOtherToPhysicalFileSystem_Expect_StandardEntityMover()
        {
            Assert.Equal(
                Registration.GetSupportedRegistration(typeof(IFileSystem), typeof(PhysicalFileSystem)).Value,
                standardEntityMover
                );
        }

        [Fact]
        public void When_MovingFromGenericToGenericFileSystem_Expect_StandardEntityMover()
        {
            Assert.Equal(
                Registration.GetSupportedRegistration(typeof(IFileSystem), typeof(IFileSystem)).Value,
                standardEntityMover
                );
        }

        [Fact]
        public void When_MovingFromPhysicalToPhysicalFileSystem_Expect_PhysicalEntityMover()
        {
            Assert.Equal(
                Registration.GetSupportedRegistration(typeof(PhysicalFileSystem), typeof(PhysicalFileSystem)).Value,
                physicalEntityMover
                );
        }
    }
}
