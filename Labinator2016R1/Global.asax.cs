﻿//-----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// </summary>
namespace Labinator2016R1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Labinator2016R1.Lib.Headers;
    using Lib.Models;
    /// <summary>
    /// The main class representing the overall MVC application
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Handle to the database
        /// </summary>
        private ILabinatorDb db;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcApplication"/> class.
        /// This version used in production and creates a handle for the Database
        /// </summary>
        public MvcApplication()
        {
            this.db = new LabinatorContext();
        }

        /// <summary>
        /// Main entry point.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        /// <summary>
        /// Handles the PostAuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        // let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        List<string> roles = new List<string>();
                        User user = this.db.Query<User>().Where(u => u.EmailAddress == username).FirstOrDefault();
                        if (user != null && user.IsInstructor)
                        {
                            roles.Add("Instructor");
                        }

                        if (user != null && user.IsAdministrator)
                        {
                            roles.Add("Administrator");
                        }

                        // Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.ToArray());
                    }
                    catch (Exception)
                    {
                        // something went wrong
                    }
                }
            }
        }
    }
}