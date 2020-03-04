using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VotingApp.TagHelpers
{
    [HtmlTargetElement("editor")]
    public class EditorTagHelper: TagHelper 
    {
        private readonly IHtmlHelper htmlHelper;

        public EditorTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }
        
        [HtmlAttributeName("for")]
        public ModelExpression Model { get; set; }
        
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Context { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware) htmlHelper)?.Contextualize(Context);
            if (Model != null)
            {
                output.Content.SetHtmlContent(htmlHelper.EditorForModel(Model.Model));
            }
        }
    }
}