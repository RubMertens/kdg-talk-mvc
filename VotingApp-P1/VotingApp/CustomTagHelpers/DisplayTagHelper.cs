using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VotingApp.CustomTagHelpers
{
    [HtmlTargetElement("display")]
    public class DisplayTagHelper: TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression Model { get; set; }
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper htmlHelper;

        public DisplayTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware)htmlHelper)?.Contextualize(ViewContext);
            output.Content.SetHtmlContent(htmlHelper.DisplayForModel(Model));
        }
    }
}