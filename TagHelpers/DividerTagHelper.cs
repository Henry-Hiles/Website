using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("hr", TagStructure = TagStructure.WithoutEndTag)]
public class DividerTagHelper : TagHelper
{
    [HtmlAttributeName("dark")]
    public bool IsDark { get; set; } = false;

    [HtmlAttributeName("round")]
    public bool IsRound { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (IsRound)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "round-divider");
            output.Content.AppendHtml(@"<svg class=""round-divider-svg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 144.54 17.34"" preserveAspectRatio=""none""
    fill=""currentColor"">
    <path d=""M144.54,17.34H0V0H144.54ZM0,0S32.36,17.34,72.27,17.34,144.54,0,144.54,0""></path>
</svg>");
        }
        else
        {
            output.TagName = "i";
            output.Attributes.SetAttribute("class", "divider-icon");
        }
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}