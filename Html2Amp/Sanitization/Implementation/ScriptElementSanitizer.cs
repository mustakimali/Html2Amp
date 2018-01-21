using System;
using AngleSharp.Dom;

namespace Html2Amp.Sanitization.Implementation
{
	public class ScriptElementSanitizer : Sanitizer
	{
		public override bool CanSanitize(IElement element)
		{
			return element != null && element.TagName == "SCRIPT";
		}

		public override IElement Sanitize(IDocument document, IElement htmlElement)
		{
		    if (htmlElement == null) throw new ArgumentException("", nameof(htmlElement));

            htmlElement.Parent.RemoveChild(htmlElement);

			return null;
		}
	}
}