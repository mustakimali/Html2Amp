using System;
using AngleSharp.Dom;

namespace Html2Amp.Sanitization.Implementation
{
	public class StyleAttributeSanitizer : Sanitizer
	{
		public override bool CanSanitize(IElement element)
		{
			return element != null && element.HasAttribute("style");
		}

		public override IElement Sanitize(IDocument document, IElement htmlElement)
		{
			if(htmlElement == null) throw new ArgumentException("", nameof(htmlElement));

			htmlElement.RemoveAttribute("style");

			return htmlElement;
		}
	}
}