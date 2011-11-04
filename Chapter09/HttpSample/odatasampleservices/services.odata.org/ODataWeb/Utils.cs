//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

namespace ODataWeb
{
    using System;
    using System.Data.Services;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Utility Functions for ODataWeb
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Resolve the physical path of files related to the application root
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string ResolvePhysicalPath(string relativePath)
        {
            if (HttpContext.Current == null)
            {
                throw new DataServiceException(500, "Cannot create database. Unable to resolve physical path to the script.");
            }

            string appRoot = HttpContext.Current.Request.PhysicalApplicationPath;
            return Path.Combine(appRoot, relativePath);
        }
    }
}