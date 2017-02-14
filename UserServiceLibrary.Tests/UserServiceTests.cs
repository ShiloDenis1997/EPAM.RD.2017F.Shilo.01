﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void Add_NotFullyInitializedUser_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();
            User user = new User();

            // Act-Assert
            Assert.Throws<NotInitializedUserException>(() => service.Add(user));
        }

        [Test]
        public void Add_ExistingUser_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();
            User user = new User
            {
                DateOfBirth = DateTime.Now,
                Firstname = "Denis",
                Secondname = "Shilo",
            };

            // Act-Assert
            Assert.Throws<UserAlreadyExistsException>(
                () =>
                {
                    service.Add(user);
                    service.Add(user);
                });
        }

        [Test]
        public void Add_NullUser_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();
            
            // Act-Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    service.Add(null);
                });
        }

        [Test]
        public void Add_ValidUser_NothingHappend()
        {
            // Arrange
            IUserService service = new UserService();
            User user = new User
            {
                DateOfBirth = DateTime.Now,
                Secondname = "Shilo",
                Firstname = "Denis",
            };

            // Act-Assert
            Assert.DoesNotThrow(() => service.Add(user));
        }

        [Test]
        public void Remove_ExistingUser_NothingHappend()
        {
            // Arrange
            IUserService service = new UserService();
            User user = new User
            {
                DateOfBirth = DateTime.Now,
                Secondname = "Shilo",
                Firstname = "Denis",
            };

            // Act-Assert
            Assert.DoesNotThrow(
                () =>
                {
                    service.Add(user);
                    service.Remove(user);
                });
        }

        [Test]
        public void Remove_NullUser_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();

            // Act-Assert
            Assert.Throws<ArgumentNullException>(() => service.Remove(null));
        }

        [Test]
        public void Remove_NotExistingUser_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();
            User user = new User
            {
                DateOfBirth = DateTime.Now,
                Secondname = "Shilo",
                Firstname = "Denis",
            };

            // Act-Assert
            Assert.Throws<UserDoesNotExistException>(() => service.Remove(user));
        }

        [Test]
        public void Search_NullPredicate_ExceptionThrown()
        {
            // Arrange
            IUserService service = new UserService();

            // Act-Assert
            Assert.Throws<ArgumentNullException>(() => service.Search(null));
        }

        [Test]
        public void Search_EmptyService_EmptyEnumerationExpected()
        {
            // Arrange
            IUserService service = new UserService();

            // Act
            IEnumerable<User> actual = service.Search(user => true);

            // Assert
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Search_ByFirstnameServiceHasSeveralUsers_EnumerationWithSeveralUsersExpected()
        {
            // Arrange
            IUserService service = new UserService();
            User[] users =
            {
                new User { Firstname = "Denis", Secondname = "Shilo", DateOfBirth = DateTime.Now },
                new User { Firstname = "Lena", Secondname = "Pavlova", DateOfBirth = DateTime.Now },
                new User { Firstname = "Denis", Secondname = "Ivanov", DateOfBirth = DateTime.Now }
            };
            foreach (var user in users)
            {
                service.Add(user);
            }

            // Act
            IEnumerable<User> actual = service.Search(user => user.Firstname.Equals("Denis"));

            // Asssert
            Assert.AreEqual(2, actual.Count());
            foreach (var user in actual)
            {
                Assert.AreEqual("Denis", user.Firstname);
            }
        }
    }
}