using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Voting.WebApp.CustomTagHelpers
{
    [HtmlTargetElement("editor")]
    public class EditorForTagHelper: TagHelper
    {
        private readonly IHtmlHelper htmlHelper;

        public EditorForTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            ((IViewContextAware) htmlHelper).Contextualize(ViewContext);
            output.Content.SetHtmlContent(htmlHelper.EditorForModel());
        }
    }
}