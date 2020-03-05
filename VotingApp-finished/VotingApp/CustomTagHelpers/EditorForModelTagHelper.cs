using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VotingApp.CustomTagHelpers
{
    [HtmlTargetElement("editor")]
    public class EditorForModelTagHelper:TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper htmlHelper;

        public EditorForModelTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware) htmlHelper).Contextualize(ViewContext);

            output.TagName = "";
            output.Content.SetHtmlContent(htmlHelper.EditorForModel());
        }
    }
}