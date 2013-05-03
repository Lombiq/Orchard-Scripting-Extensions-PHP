using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using OrchardHUN.Scripting.Models;

namespace OrchardHUN.Scripting.Php.Drivers
{
    public class ScriptPartDriver : ContentPartDriver<ScriptPart>
    {
        protected override string Prefix
        {
            get { return "OrchardHUN.Scripting.ScriptPart"; }
        }

        protected override DriverResult Editor(ScriptPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ScriptPhp_Edit",
                    () => shapeHelper.EditorTemplate(
                            TemplateName: "Parts.ScriptPhp",
                            Model: part,
                            Prefix: Prefix));
        }

        protected override DriverResult Editor(ScriptPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return Editor(part, shapeHelper);
        }
    }
}