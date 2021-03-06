﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions;
using UserServiceLibrary.Exceptions.UserService;

namespace UserServiceLibrary.Interfaces
{
    /// <summary>
    /// Interface of abstract <see cref="User"/> service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds new user to the service
        /// </summary>
        /// <param name="user">User to add to the service. After insertion
        /// <see cref="User.Id"/> property will be setted to valid user id</param>
        /// <exception cref="NotInitializedUserException">Throws when <paramref name="user"/> has
        /// not initialized fields</exception>
        /// <exception cref="UserAlreadyExistsException">Throws when <paramref name="user"/> 
        /// already exists</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="user"/> is null</exception>
        void Add(User user);

        /// <summary>
        /// Removes user from the service
        /// </summary>
        /// <param name="user">User to remove from the service</param>
        /// <exception cref="UserDoesNotExistException">Throws when <paramref name="user"/>
        /// does not exist in the service</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="user"/> 
        /// is null</exception>
        void Remove(User user);

        /// <summary>
        /// Searches users by predicate 
        /// </summary>
        /// <param name="predicate">Predicate that establish if user will be included
        /// in the result set</param>
        /// <returns>Enumeration of <see cref="User"/>s on which <paramref name="predicate"/>
        /// gives true</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="predicate"/>
        /// is null</exception>
        IEnumerable<User> Search(Predicate<User> predicate);
    }
}
