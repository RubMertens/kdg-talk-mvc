using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Voting.WebApp.CustomTagHelpers
{
    [HtmlTargetElement("display")]
    public class DisplayForTagHelper: TagHelper
    {
        private readonly IHtmlHelper htmlHelper;

        public DisplayForTagHelper(IHtmlHelper htmlHelper)
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
            output.Content.SetHtmlContent(htmlHelper.DisplayForModel());
        }
    }
}