using System;
using AngleSharp.Dom;
using Html2Amp.UnitTests.Spies;

namespace Html2Amp.UnitTests.Accessors
{
	public class MediaSanitizerAccessor : MediaSanitizerSpy
	{
		public MediaSanitizerAccessor(bool shoulRequestResourcesOnlyViaHttps = false)
			: base(shoulRequestResourcesOnlyViaHttps)
		{
		}

		public new bool ShoulRequestResourcesOnlyViaHttps
		{
			get
			{
				return base.ShoulRequestResourcesOnlyViaHttps;
			}
		}

		public new void SetElementLayout(IElement element, IElement ampElement)
		{
			base.SetElementLayout(element, ampElement);
		}

		public new void SetMediaElementLayout(IElement element, IElement ampElement)
		{
            if(ampElement == null) throw new ArgumentException("", nameof(ampElement));
            if(element == null) throw new ArgumentException("", nameof(element));
			base.SetMediaElementLayout(element, ampElement);
		}

		public new void RewriteSourceAttribute(IElement htmlElement)
		{
			base.RewriteSourceAttribute(htmlElement);
		}

		public new IElement SanitizeCore<T>(IDocument document, IElement htmlElement, string ampElementTagName)
		where T : IElement
		{
		    if (string.IsNullOrEmpty(ampElementTagName)) throw new ArgumentException("", nameof(ampElementTagName));
		    if (document == null) throw new ArgumentException("", nameof(document));
		    if (htmlElement == null) throw new ArgumentException("", nameof(htmlElement));
		    
            return base.SanitizeCore<T>(document, htmlElement, ampElementTagName);
		}
	}
}