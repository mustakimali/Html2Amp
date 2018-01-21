﻿using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace Html2Amp.Sanitization.Implementation
{
    public class YouTubeVideoSanitizer : MediaSanitizer
    {
		protected readonly List<string> AllowedAttribtes = new List<string>() { "width", "height", "id" };

        public const string VideoIdRegex = @"^/embed/(?<id>[^/\?]+)/?$";

        public override bool CanSanitize(IElement element)
        {
            if (element == null || !(element is IHtmlInlineFrameElement))
            {
                return false;
            }

            var sourceAttributeValue = ((IHtmlInlineFrameElement)element).Source;

            Uri sourceUri;
            if (Uri.TryCreate(sourceAttributeValue, UriKind.Absolute, out sourceUri))
            {
                return sourceUri.LocalPath.StartsWith("/embed/")
                    && Regex.IsMatch(sourceUri.Host, @"^(www\.)?youtube(-nocookie)?\.com$");
            }

            return false;
        }

        public override IElement Sanitize(IDocument document, IElement htmlElement)
        {
            if (document == null) throw new ArgumentException("", nameof(document));
            if (htmlElement == null) throw new ArgumentException("", nameof(htmlElement));
            
            var ampElement = document.CreateElement("amp-youtube");

			htmlElement.CopyAttributes(ampElement, this.AllowedAttribtes);
            this.SetElementLayout(htmlElement, ampElement);

            Uri videoUri = new Uri(htmlElement.GetAttribute("src"));

            var videoId = this.GetVideoId(videoUri);
            ampElement.SetAttribute("data-videoid", videoId);

            var videoParams = HttpUtility.ParseQueryString(videoUri.Query);
            this.SetVideoParams(ampElement, videoParams);

            htmlElement.Parent.ReplaceChild(ampElement, htmlElement);

            return ampElement;
        }

        protected virtual void SetVideoParams(IElement ampElement, NameValueCollection videoParams)
        {
            
            

            foreach (var paramName in videoParams.AllKeys)
            {
                var ampParamAttributeName = "data-param-" + paramName;
                ampElement.SetAttribute(ampParamAttributeName, videoParams[paramName]);
            }
        }

        protected virtual string GetVideoId(Uri videoUri)
        {
            

            var videoIdMatch = Regex.Match(videoUri.LocalPath, VideoIdRegex);

            return videoIdMatch.Groups["id"].Value;
        }
    }
}