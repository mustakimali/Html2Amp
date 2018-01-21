using System;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace Html2Amp.Sanitization.Implementation
{
	public class HrefJavaScriptSanitizer : Sanitizer
	{
		public override bool CanSanitize(IElement element)
		{
			if (element == null || !(element is IHtmlAnchorElement))
			{
				return false;
			}

			var hrefAttribute = element.GetAttribute("href");
			return hrefAttribute != null && hrefAttribute.StartsWith("javascript:");
		}

		public override IElement Sanitize(IDocument document, IElement htmlElement)
		{
		    if (htmlElement == null) throw new ArgumentException("", nameof(htmlElement));

            htmlElement.SetAttribute("href", "#");

			return htmlElement;
		}
	}
}