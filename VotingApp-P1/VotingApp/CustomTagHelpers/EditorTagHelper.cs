using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VotingApp.TagHelpers
{
    [HtmlTargetElement("editor")]
    public class EditorTagHelper: TagHelper 
    {
        private readonly IHtmlHelper htmlHelper;
        private readonly HtmlEncoder encoder;

        public EditorTagHelper(IHtmlHelper htmlHelper, HtmlEncoder encoder)
        {
            this.htmlHelper = htmlHelper;
            this.encoder = encoder;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
        }
    }
}