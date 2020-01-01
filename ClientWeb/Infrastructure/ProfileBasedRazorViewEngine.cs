using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ClientWeb.Infrastructure
{
 

        /// <summary>
        /// A razor based view engine that locates views based on their profile.
        /// </summary>
           public class ProfileBasedRazorViewEngine : RazorViewEngine
        {
            private readonly IEnumerable<string> _profiles;

            /// <summary>
            /// Creates an instance of the profileBasedRazorViewEngine class.
            /// </summary>
            /// <param name="profiles">The list of profiles in priority order supported by the application.</param>
            public ProfileBasedRazorViewEngine(IEnumerable<string> profiles) : this(profiles, null)
            {
            }

            /// <summary>
            /// Creates an instance of the profileBasedRazorViewEngine class.
            /// </summary>
            /// <param name="profiles">The list of profiles in priority order supported by the application.</param>
            /// <param name="viewPageActivator">The ViewPageActivator to use for page dependency resolution.</param>
            public ProfileBasedRazorViewEngine(IEnumerable<string> profiles, IViewPageActivator viewPageActivator) : base(viewPageActivator)
            {
                _profiles = profiles ?? new String[0];

    //            AreaViewLocationFormats = new[] {
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.vbhtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.vbhtml"
    //}.Concat(base.AreaViewLocationFormats).ToArray();
    //            AreaMasterLocationFormats = new[] {
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.vbhtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.vbhtml"
    //}.Concat(base.AreaMasterLocationFormats).ToArray();
    //            AreaPartialViewLocationFormats = new[] {
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/{1}/{{0}}/{0}.vbhtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.cshtml",
    //  "~/Areas/{2}/Views/Shared/{{0}}/{0}.vbhtml"
    //}.Concat(base.AreaPartialViewLocationFormats).ToArray();

                ViewLocationFormats = new[] {
      "~/Content/UserContent/{{0}}/Theme/Views/{1}/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/{1}/{0}.vbhtml",
      "~/Content/UserContent/{{0}}/Theme/Views/Shared/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/Shared/{0}.vbhtml"
    }.Concat(base.ViewLocationFormats).ToArray();
                MasterLocationFormats = new[] {
      "~/Content/UserContent/{{0}}/Theme/Views/{1}/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/{0}.vbhtml",
      "~/Content/UserContent/{{0}}/Theme/Views/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/{0}.vbhtml"
    }.Concat(base.MasterLocationFormats).ToArray();
                PartialViewLocationFormats = new[] {
      "~/Content/UserContent/{{0}}/Theme/Views/{1}/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/{1}/{0}.vbhtml",
      "~/Content/UserContent/{{0}}/Theme/Views/Shared/{0}.cshtml",
      "~/Content/UserContent/{{0}}/Theme/Views/Shared/{0}.vbhtml"
    }.Concat(base.PartialViewLocationFormats).ToArray();
            }

            /// <summary>
            /// Creates a partial view using the specified controller context and partial path.
            /// </summary>
            /// <returns>
            /// The partial view.
            /// </returns>
            /// <param name="controllerContext">The controller context.</param><param name="partialPath">The path to the partial view.</param>
            protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
            {
                return base.CreatePartialView(controllerContext, GetprofileBasedPath(controllerContext, partialPath));
            }

            /// <summary>
            /// Creates a view by using the specified controller context and the paths of the view and master view.
            /// </summary>
            /// <returns>
            /// The view.
            /// </returns>
            /// <param name="controllerContext">The controller context.</param>
            /// <param name="viewPath">The path to the view.</param>
            /// <param name="masterPath">The path to the master view.</param>
            protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
            {
                return base.CreateView(controllerContext,
                                       GetprofileBasedPath(controllerContext, viewPath),
                                       GetprofileBasedPath(controllerContext, masterPath));
            }

            /// <summary>
            /// Resolves the path based on the profile information.
            /// </summary>
            /// <param name="controllerContext">The controller context.</param>
            /// <param name="viewPath">The path to the view.</param>
            /// <returns>The resolved view path.</returns>
            private string GetprofileBasedPath(ControllerContext controllerContext, string viewPath)
            {
           
            if ((!String.IsNullOrEmpty(viewPath)) &&
                    (controllerContext.RouteData.Values["profile"] != null))
                {
           var  p=   controllerContext.RouteData.Values["profile"].ToString();
                    foreach (string profile in _profiles.Where(profile => p==profile))
                    {
                        string resolvedViewPath = String.Format(CultureInfo.InvariantCulture, viewPath, profile);
                        if (base.FileExists(controllerContext, resolvedViewPath))
                        {
                            return (resolvedViewPath);
                        }
                    }
                }
                return (viewPath);
            }

            /// <summary>
            /// Gets a value that indicates whether a file exists in the specified virtual file system (path).
            /// </summary>
            /// <returns>
            /// true if the file exists in the virtual file system; otherwise, false.
            /// </returns>
            /// <param name="controllerContext">The controller context.</param><param name="virtualPath">The virtual path.</param>
            protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
            {
                if (controllerContext.RouteData.Values["profile"] != null)
                {
                var p = controllerContext.RouteData.Values["profile"].ToString();
                    if (_profiles.Where(profile => p==profile)
                              .Any(profile => base.FileExists(controllerContext, String.Format(CultureInfo.InvariantCulture, virtualPath, profile))))
                    {
                        return (true);
                    }
                }
                return (base.FileExists(controllerContext, virtualPath));
            }

            /// <summary>
            /// Finds the specified partial view by using the specified controller context.
            /// </summary>
            /// <returns>
            /// The partial view.
            /// </returns>
            /// <param name="controllerContext">The controller context.</param>
            /// <param name="partialViewName">The name of the partial view.</param>
            /// <param name="useCache">true to use the cached partial view.</param>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="controllerContext"/> parameter is null (Nothing in Visual Basic).</exception>
            /// <exception cref="T:System.ArgumentException">The <paramref name="partialViewName"/> parameter is null or empty.</exception>
            public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
            {
                return (base.FindPartialView(controllerContext, partialViewName, false));
            }

            /// <summary>
            /// Finds the specified view by using the specified controller context and master view name.
            /// </summary>
            /// <returns>
            /// The page view.
            /// </returns>
            /// <param name="controllerContext">The controller context.</param>
            /// <param name="viewName">The name of the view.</param>
            /// <param name="masterName">The name of the master view.</param>
            /// <param name="useCache">true to use the cached view.</param>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="controllerContext"/> parameter is null (Nothing in Visual Basic).</exception>
            /// <exception cref="T:System.ArgumentException">The <paramref name="viewName"/> parameter is null or empty.</exception>
            public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
            {
                return (base.FindView(controllerContext, viewName, masterName, false));
            }
        
    }
}