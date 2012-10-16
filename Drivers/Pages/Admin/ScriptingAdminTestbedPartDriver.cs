using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrchardHUN.Scripting.Models.Pages.Admin;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using OrchardHUN.Scripting.Exceptions;
using OrchardHUN.Scripting.Services;
using Orchard.UI.Notify;
using Orchard.Localization;

namespace OrchardHUN.Scripting.Php.Drivers.Pages.Admin
{
    public class ScriptingAdminTestbedPartDriver : ContentPartDriver<ScriptingAdminTestbedPart>
    {
        protected override string Prefix
        {
            get { return "OrchardHUN.Scripting.ScriptingAdminTestbedPart"; }
        }

        protected override DriverResult Display(ScriptingAdminTestbedPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_ScriptingAdminTestbedPhp",
                        () => shapeHelper.DisplayTemplate(
                                        TemplateName: "Pages/Admin/TestbedPhp",
                                        Model: part,
                                        Prefix: Prefix));
        }
    }
}