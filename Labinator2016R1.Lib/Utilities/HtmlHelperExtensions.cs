//-----------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// </summary>
namespace Labinator2016R1.Lib.Utilities
{
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Sundry extensions to shove stuff into pages....
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Get the Current Version from the AssemblyInfo.cs file.
        /// </summary>
        /// <param name="helper">The helper.?!</param>
        /// <returns>The current version</returns>
        public static MvcHtmlString CurrentVersion(this HtmlHelper helper)
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return MvcHtmlString.Create(version.ToString());
            }
            catch
            {
                return MvcHtmlString.Create("?.?.?.?");
            }
        }
    }
}