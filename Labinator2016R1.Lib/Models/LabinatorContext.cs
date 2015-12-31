﻿//-----------------------------------------------------------------------
// <copyright file="LabinatorContext.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// Version: 1.0 - Initial build.
/// </summary>
namespace Labinator2016R1.Lib.Models
{
    using System.Data.Entity;
    using System.Linq;
    using Lib.Headers;

    /// <summary>
    /// Implements the Database interface for the actual database.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    /// <seealso cref="Labinator2016R1.Lib.Headers.ILabinatorDb" />
    public partial class LabinatorContext : DbContext, ILabinatorDb
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabinatorContext"/> class.
        /// </summary>
        public LabinatorContext() : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Gets or sets the Users table reference.
        /// </summary>
        /// <value>
        /// A representation of the Users table in the Database.
        /// </value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Implements the Interface Query on the Database.
        /// </summary>
        /// <typeparam name="T">Table being queried</typeparam>
        /// <returns>
        /// The data from the table.
        /// </returns>
        IQueryable<T> ILabinatorDb.Query<T>()
        {
            return this.Set<T>();
        }

        /// <summary>
        /// Implements the Add Record Interface on the Database.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to add.</param>
        void ILabinatorDb.Add<T>(T entity)
        {
            this.Set<T>().Add(entity);
        }

        /// <summary>
        /// Implements the Update Record Interface on the Database.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to update.</param>
        void ILabinatorDb.Update<T>(T entity)
        {
            this.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Implements the Remove a Record Interface on the Database.
        /// </summary>
        /// <typeparam name="T">Type (Table) to add record to.</typeparam>
        /// <param name="entity">Data to remove.</param>
        void ILabinatorDb.Remove<T>(T entity)
        {
            this.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Implements the Save Outstanding Changes Interface on the Database.
        /// </summary>
        void ILabinatorDb.SaveChanges()
        {
            this.SaveChanges();
        }
    }
}