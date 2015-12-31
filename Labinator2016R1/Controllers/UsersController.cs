﻿//-----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// Version: 1.0 - Initial build.
/// </summary>
namespace Labinator2016R1.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Labinator2016R1.Lib.Headers;
    using Labinator2016R1.Lib.Models;
    using Labinator2016R1.ViewModels;
    using Lib.Utilities;
    using Utility;
    using ViewModels.DatatablesViewModel;
    /// <summary>
    /// Back-end processing for all User-related work and pages.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class UsersController : Controller
    {
        /// <summary>
        /// Handle to the database
        /// </summary>
        private ILabinatorDb db;

        /// <summary>
        /// Handle to the authenticator
        /// </summary>
        private IAuth auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// Used for regular constructions. Obtains handle to Database.
        /// </summary>
        public UsersController()
        {
            this.db = new LabinatorContext();
            this.auth = new FormsAuthWrapper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// Used for constructing when Unit Testing.
        /// </summary>
        /// <param name="db">Handle to Database stub..</param>
        public UsersController(ILabinatorDb db)
        {
            this.db = db;
        }

        /// <summary>
        /// Used to display a page listing existing users.
        /// </summary>
        /// <returns>View page for the user list.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
        /// <summary>
        /// Displays a page to edit a user
        /// </summary>
        /// <param name="id">The id of the User being edited. Zero means new.</param>
        /// <returns>The User Edit view</returns>
        public ActionResult Edit(int? id)
        {
            User user;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == 0)
            {
                user = new User() { UserId = 0, EmailAddress = "New" };
            }
            else
            {
                user = this.db.Query<User>().Where(u => u.UserId == id).FirstOrDefault();
                if (user == null)
                {
                    return this.HttpNotFound();
                }
            }
            return this.View(user);
        }

        /// <summary>
        /// Processes the data from the Edit form.
        /// </summary>
        /// <param name="user">The user object from the Edit page</param>
        /// <returns>Back to Index, or re-display Edit page if there was an error.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,EmailAddress,Password,IsInstructor,IsAdministrator")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.UserId == 0)
                {
                    this.db.Add<User>(user);
                }
                else
                {
                    this.db.Update<User>(user);
                }
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        /// <summary>
        /// First stage to delete a User.
        /// </summary>
        /// <param name="id">The id of the User to delete</param>
        /// <returns>The Delete confirmation view</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = this.db.Query<User>().Where(u => u.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return this.HttpNotFound();
            }
            return this.View(user);
        }

        /// <summary>
        /// Deletes the user after confirmation.
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <returns>The user list.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = this.db.Query<User>().Where(u => u.UserId == id).FirstOrDefault();
            this.db.Remove<User>(user);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Used to respond to an AJAX request for a list of Users. The results are then
        /// used to populate a DataTable on the Index Page.
        /// </summary>
        /// <param name="param">The information sent from the DataTable regarding sorts, filters etc.</param>
        /// <returns>A JSON response with the new information to display.</returns>
        public ActionResult Ajax(DTParameters param)
        {
            return this.Json(Generic.Ajax<User>(this.db.Query<User>().ToList(), param));
        }

        /// <summary>
        /// Sends the initial Login view to the browser.
        /// </summary>
        /// <param name="returnUrl">The URL of the page that caused the authentication request.
        /// Used to redirect back after successful login.</param>
        /// <returns>The view for the Login Dialog</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel { ReturnUrl = returnUrl };
            return this.View(model);
        }

        /// <summary>
        /// Called when the user submits a login request.
        /// Verifies the credentials and redirects to the calling page, or re-displays Login Dialog if the Login fails.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A redirect to the original page, or the view for the Login Dialog</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                string username = model.UserName;
                string password = model.Password;
                var user = this.db.Query<User>().Where(i => i.EmailAddress == username).FirstOrDefault();
                if (user != null)
                {
                    bool userValid = PasswordHash.ValidatePassword(password, user.Password);

                    // User found in the database
                    if (userValid)
                    {
                        ////Log.Write(this.db, new Log() { User = username, Message = LogMessages.logon });
                        string returnUrl = model.ReturnUrl;
                        this.auth.DoAuth(username, false);
                        ActionResult response = Redirect(returnUrl);
                        return response;
                    }
                }

                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
                ////Log.Write(this.db, new Log() { User = username, Message = LogMessages.incorrectlogon });
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        /// <summary>
        /// Processes the logout request
        /// </summary>
        /// <returns>Redirect to main homepage</returns>
        public ActionResult Logout()
        {
            this.auth.DoDeAuth();
            return this.Redirect("/");
        }
    }
}