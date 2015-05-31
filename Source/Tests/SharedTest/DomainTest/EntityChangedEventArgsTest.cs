using NUnit.Framework;
using Shared.Domain;
using Shared.Message;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class EntityChangedEventArgsTest
    {
        [Test]
        public void EntityCreatedDoesNotHaveAPreviousEntity()
        {
            var bandEntity = new Band(1, "Bandname");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, NotificationType.Create);
            Assert.IsNull(entityChangedEventArgs.PreviousEntity);
        }

        [Test]
        public void EntityCreatedSetsNotificationTypeToCreate()
        {
            var bandEntity = new Band(1, "Bandname");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, NotificationType.Create);
            Assert.AreEqual(NotificationType.Create, entityChangedEventArgs.NotificationType);
        }

        [Test]
        public void EntityDeletedMakesCurrentAndPreviousEntitySame()
        {
            var bandEntity = new Band(1, "Bandname");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, NotificationType.Delete);
            Assert.AreEqual(entityChangedEventArgs.Entity, entityChangedEventArgs.PreviousEntity);
        }

        [Test]
        public void EntityDeletedSetsNotificationTypeToDelete()
        {
            var bandEntity = new Band(1, "Bandname");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, NotificationType.Delete);
            Assert.AreEqual(NotificationType.Delete, entityChangedEventArgs.NotificationType);
        }

        [Test]
        public void EntityUpdatedHasPreviousEntitySet()
        {
            var bandEntity = new Band(1, "Bandname");
            var previousBandEntity = new Band(1, "Old Band Name");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, previousBandEntity);
            Assert.AreEqual(bandEntity, entityChangedEventArgs.Entity);
            Assert.AreEqual(previousBandEntity, entityChangedEventArgs.PreviousEntity);
        }

        [Test]
        public void UpdatingEntitySetsNotificationTypeToUpdate()
        {
            var bandEntity = new Band(1, "Bandname");
            var previousBandEntity = new Band(1, "Old Band Name");
            EntityChangedEventArgs<Band> entityChangedEventArgs = new EntityChangedEventArgs<Band>(bandEntity, previousBandEntity);
            Assert.AreEqual(NotificationType.Update, entityChangedEventArgs.NotificationType);
        }
    }
}