using System;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace Html2Amp.Sanitization.Implementation
{
	public class AudioSanitizer : MediaSanitizer
	{
		public override bool CanSanitize(IElement element)
		{
			return element != null && element is IHtmlAudioElement;
		}

		public override IElement Sanitize(IDocument document, IElement htmlElement)
		{
		    if (document == null) throw new ArgumentException("", nameof(document));
		    if (htmlElement == null) throw new ArgumentException("", nameof(htmlElement));

            return this.SanitizeCore<IHtmlAudioElement>(document, htmlElement, "amp-audio");
		}

		protected override void SetMediaElementLayout(IElement element, IElement ampElement)
		{
			if (!ampElement.HasAttribute("layout"))
			{
				if (ampElement.HasAttribute("height"))
				{
					if (ampElement.HasAttribute("width"))
					{
						if (ampElement.GetAttribute("width") == "auto")
						{
							ampElement.SetAttribute("layout", "responsive");
						}
						else
						{
							ampElement.SetAttribute("layout", "fixed");
						}
					}
					else
					{
						ampElement.SetAttribute("layout", "fixed-height");
					}
				}
			}
		}

		protected override bool ShoulRequestResourcesOnlyViaHttps
		{
			get { return true; }
		}
	}
}