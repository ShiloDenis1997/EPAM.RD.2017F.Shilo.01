using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UserServiceLibrary;

namespace UserStorageLibrary.Tests
{
    [TestFixture]
    public class UserStorageTests
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

        [TestCase(null)]
        [TestCase("   ")]
        [TestCase("")]
        [Test]
        public void Ctor_NullEmptyOrWhitespaceFilename_ArgumentExceptionThrown(string filename)
        {
            // Act-Assert
            Assert.Throws<ArgumentException>(() => new UserStorage(filename));
        }

        [Test]
        public void StoredUsersLoaded_EqualCollectionsExpected()
        {
            //// Arrange
            // UserStorage userStorage = new UserStorage("testStorage.xml");
            // IEnumerable<User> actualUsers;

            //// Act
            // userStorage.StoreUsers(this.usersEnumeration);
            // actualUsers = userStorage.LoadUsers();

            //// Assert
            // CollectionAssert.AreEquivalent(this.usersEnumeration, actualUsers);
        }
    }
}
