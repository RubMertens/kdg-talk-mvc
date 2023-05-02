using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Voting.WebApp.CustomTagHelpers
{
    [HtmlTargetElement("editor-for-model")]
    public class EditorForModelTagHelper: TagHelper
    {
        private readonly IHtmlHelper htmlHelper;

        public EditorForModelTagHelper(IHtmlHelper htmlHelper)
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