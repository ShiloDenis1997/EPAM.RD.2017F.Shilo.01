using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Tests
{
    [TestFixture]
    public class SlaveUserServiceTests
    {
        private IEnumerable<User> usersEnumeration;

        [OneTimeSetUp]
        public void PreTestInit()
        {
            this.usersEnumeration = new[]
            {
                new User
                {
                    DateOfBirth = DateTime.MinValue,
                    Firstname = "Denis",
                    Id = 1,
                    Secondname = "Shilo"
                },
                new User
                {
                    DateOfBirth  = DateTime.MaxValue,
                    Firstname = "Ivan",
                    Id = 2,
                    Secondname = "Matyash"
                },
                new User
                {
                    DateOfBirth = DateTime.MinValue,
                    Secondname = "Drozd",
                    Firstname = "Artsiom",
                    Id = 3
                },
            };
        }

        [Test]
        public void AddUser_ExceptionThrown()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act-Assert
            Assert.Throws<NotSupportedException>(() => sus.Add(new User()));
        }

        [Test]
        public void RemoveUser_ExceptionThrown()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act-Assert
            Assert.Throws<NotSupportedException>(() => sus.Remove(new User()));
        }

        [Test]
        public void AddEventHandler_NullUser_ExceptionThrown()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act-Assert
            Assert.Throws<ArgumentNullException>(
                () => sus.UserAddedHandler(
                    new object(), new UserEventArgs { User = null }));
        }

        [Test]
        public void RemoveEventHandler_NullUser_ExceptionThrown()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act-Assert
            Assert.Throws<ArgumentNullException>(
                () => sus.UserRemovedHandler(
                    new object(), new UserEventArgs { User = null }));
        }

        [Test]
        public void AddedEventHandler_SearchAllAdded_EqualEnumerationsExpected()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act
            foreach (var user in this.usersEnumeration)
            {
                sus.UserAddedHandler(new object(), new UserEventArgs { User = user });
            }

            // Assert
            CollectionAssert.AreEquivalent(this.usersEnumeration, sus.Search(u => true));
        }

        [Test]
        public void RemoveEventHandler_SearchAllAdded_EqualEnumerationsExpected()
        {
            // Arrange
            SlaveUserService sus = new SlaveUserService();

            // Act
            foreach (var user in this.usersEnumeration)
            {
                sus.UserAddedHandler(new object(), new UserEventArgs { User = user });
            }

            sus.UserRemovedHandler(
                new object(), new UserEventArgs { User = this.usersEnumeration.First() });
            this.usersEnumeration = this.usersEnumeration.Skip(1);

            // Assert
            CollectionAssert.AreEquivalent(this.usersEnumeration, sus.Search(u => true));
        }
    }
}
