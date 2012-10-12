using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;
using Orchard.Security;

namespace OrchardHUN.Scripting.Php
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder/*.AddImageSet("scripting")*/
                .Add(T("Scripting"), "4",
                    menu => menu.Add(T("PHP"), "0", item => item.Action("TestBed", "Admin", new { area = "OrchardHUN.Scripting.Php" })
                        .Permission(StandardPermissions.SiteOwner)));
        }
    }
}