using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("hr", TagStructure = TagStructure.WithoutEndTag)]
public class DividerTagHelper : TagHelper
{
    [HtmlAttributeName("dark")]
    public bool IsDark { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "i";
        output.Attributes.SetAttribute("class", "divider-icon" + (IsDark ? " divider-icon-dark" : ""));
        output.TagMode = TagMode.StartTagAndEndTag;
        // <i class="divider-icon-dark divider-icon"></i>
    }
}