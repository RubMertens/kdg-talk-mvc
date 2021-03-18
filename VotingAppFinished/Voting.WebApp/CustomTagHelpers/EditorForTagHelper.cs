using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

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

    [HtmlTargetElement("editor-for")]
    public class EditorTagHelper : TagHelper
    {
        private readonly IHtmlHelper htmlHelper;

        public ModelExpression ModelExpression { get; set; }
        public string Template { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public EditorTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (!output.Attributes.ContainsName(nameof(Template)))
            {
                output.Attributes.Add(nameof(Template), Template);
            }

            output.SuppressOutput();

            (htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

            output.Content.SetHtmlContent(htmlHelper.Editor(ModelExpression.Name, Template));

            await Task.CompletedTask;
        }
    }
}