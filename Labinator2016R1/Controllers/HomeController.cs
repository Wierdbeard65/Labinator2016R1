//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// Version: 1.0 - Initial build.
/// </summary>
namespace Labinator2016R1.Controllers
{
    using System.Web.Mvc;
    using Lib.Headers;
    using Lib.Models;
    using ViewModels;

    /// <summary>
    /// Controller class for the Home Page
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Handle to the Database
        /// </summary>
        private ILabinatorDb db;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// This version used in production and creates handles for both the Database and the SkyTap interface
        /// </summary>
        public HomeController()
        {
            this.db = new LabinatorContext();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// This version is used when unit testing and takes in the handles.
        /// </summary>
        /// <param name="db">Stub for the fake Database.</param>
        public HomeController(ILabinatorDb db)
        {
            this.db = db;
        }

        /// <summary>
        /// Initial Index with a default view of "Owner's only"
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel() { ShowAll = false };
            return this.View(model);
        }

        /// <summary>
        /// Index if re-display is required (e.g. if the scope of classes is changed).
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            return this.View(model);
        }
    }
}