﻿//-----------------------------------------------------------------------
// <copyright file="ILabinatorDb.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// Version: 1.0 - Initial build.
/// </summary>
namespace Labinator2016R1.Lib.Headers
{
    using System;
    using System.Linq;

    /// <summary>
    /// Interface to abstract the database. This is so we can Unit Test
    /// without needing to access the deal DB
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ILabinatorDb : IDisposable
    {
        /// <summary>
        /// Performs a query for the specified type..
        /// </summary>
        /// <typeparam name="T">Table being queried</typeparam>
        /// <returns>The data from the table.</returns>
        IQueryable<T> Query<T>() where T : class;
        
        /// <summary>
        /// Adds a record to a table.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to add.</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Updates a record in a table.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to update.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Removes a record from a table.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to remove.</param>
        void Remove<T>(T entity) where T : class;
        
        /// <summary>
        /// Saves outstanding changes to the database.
        /// </summary>
        void SaveChanges();
    }
}