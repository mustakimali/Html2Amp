﻿using AngleSharp.Dom;
using Html2Amp.Sanitization.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html2Amp.UnitTests.Accessors
{
	public class YouTubeVideoSanitizerAccessor : YouTubeVideoSanitizer
	{
		public new void SetVideoParams(IElement ampElement, NameValueCollection videoParams)
		{
		    if (ampElement == null) throw new ArgumentException("", nameof(ampElement));
		    if (videoParams == null) throw new ArgumentException("", nameof(videoParams));

            base.SetVideoParams(ampElement, videoParams);
		}

		public new string GetVideoId(Uri videoUri)
		{
		    if (videoUri == null) throw new ArgumentException("", nameof(videoUri));
            return base.GetVideoId(videoUri);
		}
	}
}