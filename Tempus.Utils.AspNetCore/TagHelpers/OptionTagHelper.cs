namespace Tempus.Utils.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [HtmlTargetElement("option")]
    [HtmlTargetElement("input")]
    [HtmlTargetElement("textarea")]
    [HtmlTargetElement("button")]
    [HtmlTargetElement("select")]
    public class OptionsTagHelper : TagHelper
    {
        private const string DisabledAttributeName = "asp-disabled";

        private const string SelectedAttributeName = "asp-selected";

        private const string HiddenAttributeName = "asp-hidden";

        private const string ReadOnlyAttributeName = "asp-readonly";

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeName(SelectedAttributeName)]
        public bool Selected { get; set; }

        [HtmlAttributeName(HiddenAttributeName)]
        public bool Hidden { get; set; }

        [HtmlAttributeName(ReadOnlyAttributeName)]
        public bool ReadOnly { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (Disabled)
                output.Attributes.SetAttribute("disabled", null);

            if (Selected)
                output.Attributes.SetAttribute("selected", null);

            if (Hidden)
                output.Attributes.SetAttribute("hidden", null);

            if (ReadOnly)
                output.Attributes.SetAttribute("readonly", null);
        }
    }
}
