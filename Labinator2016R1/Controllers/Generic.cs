//-----------------------------------------------------------------------
// <copyright file="Generic.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul.Simpson
/// Version: 1.0 - Initial build - 12/27/2015 6:31:34 PM
/// </summary>
namespace Labinator2016R1.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using ViewModels.DatatablesViewModel;
    public class Generic
    {
        public static DTResult<T> Ajax<T>(List<T> allRecords, DTParameters param) where T : class
        {
            String Search = null;
            if (param.Search.Value != null)
            {
                Search = param.Search.Value.ToLower();
            }
            List<T> displayedRecords = allRecords.Skip(param.Start).Take(param.Length).ToList();
            DTResult<T> result = new DTResult<T>
            {
                draw = param.Draw,
                data = displayedRecords,
                recordsFiltered = allRecords.Count(),
                recordsTotal = allRecords.Count()
            };
            return result;
        }
    }
}