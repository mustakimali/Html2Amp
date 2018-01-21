﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Html2Amp.Sanitization;
using Html2Amp.Sanitization.Implementation;
using System.Collections.Generic;

namespace Html2Amp
{
	public class HtmlToAmpConverter
	{
		private HtmlParser parser;

		private HashSet<ISanitizer> sanitizers;

		private RunConfiguration configuration;

		private RunContext runContext;

		private volatile bool isInitialized = false;

		private object initializationLock = new object();

		public HtmlToAmpConverter()
		{
			// We should load the css engine of AngleSharp,
			//otherwise it doesn't pasre the css.
			// https://github.com/AngleSharp/AngleSharp/issues/90
			this.parser = new HtmlParser(Configuration.Default.WithCss());
			this.configuration = new RunConfiguration();
			this.sanitizers = new HashSet<ISanitizer>();

			// Initializing a default collection of sanitizers
			// With high priority should be the sanitizers which remove and rewrite elements in order to
			// eliminate redundant removing/rewriting of them itself or their attributes. After that
			// they should be ordered by logical behaviour. E.g. the YouTubeVideoSanitizer should be
			// before IFrameSanitizer, because YouTubeVideoSanitizer relies on iframe element.

			// Removing elements
			this.sanitizers.Add(new ScriptElementSanitizer());

			// Rewriting elements
			this.sanitizers.Add(new ImageSanitizer());
			this.sanitizers.Add(new YouTubeVideoSanitizer());
			this.sanitizers.Add(new IFrameSanitizer());
            this.sanitizers.Add(new AudioSanitizer());

			// Removing attributes
			this.sanitizers.Add(new StyleAttributeSanitizer());
			this.sanitizers.Add(new JavaScriptRelatedAttributeSanitizer());
			this.sanitizers.Add(new XmlAttributeSanitizer());
			// Changing attributes
			this.sanitizers.Add(new HrefJavaScriptSanitizer());
			this.sanitizers.Add(new TargetAttributeSanitizer());
		}

		/// <summary>
		/// Sets the collection of <see cref="ISanitizer"/> which will be used for convertion.
		/// </summary>
		/// <param name="sanitizers">Collection of <see cref="ISanitizer"/></param>
		/// <returns><see cref="HtmlToAmpConverter"/></returns>
		public HtmlToAmpConverter WithSanitizers(HashSet<ISanitizer> sanitizers)
		{
			this.sanitizers = sanitizers;

			return this;
		}

		/// <summary>
		/// Sets the <see cref="RunConfiguration" /> which will be used for convertion.
		/// </summary>
		/// <param name="configuration">The <see cref="RunConfiguration"/></param>
		/// <returns><see cref="HtmlToAmpConverter"/></returns>
		public HtmlToAmpConverter WithConfiguration(RunConfiguration configuration)
		{
			this.configuration = configuration;

			return this;
		}

		/// <summary>
		/// Converts HTML to "AMP HTML".
		/// </summary>
		/// <param name="htmlSource">HTML source code to be converted.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="htmlSource"/> is null or empty string.</exception>
		/// <returns>Returns <see cref="ConvertionResult"/> which contains the result of the convertion.</returns>
		public ConvertionResult ConvertFromHtml(string htmlSource)
		{
			IHtmlDocument document = parser.Parse(htmlSource);
			IHtmlHtmlElement htmlElement = (IHtmlHtmlElement)document.DocumentElement;

			this.EnsureInitilized();

			ConvertFromHtmlElement(document, document.Body);

			return new ConvertionResult {
				AmpHtml = document.Body.InnerHtml
			};
		}

		private void EnsureInitilized()
		{
			if (!isInitialized)
			{
				lock (this.initializationLock)
				{
					if (!isInitialized)
					{
						this.Initialize();
						this.isInitialized = true;
					}
				}
			}
		}

		private void Initialize()
		{
			this.runContext = new RunContext(this.configuration);

			foreach (var sanitizer in this.sanitizers)
			{
				sanitizer.Configure(this.runContext);
			}
		}

		private void ConvertFromHtmlElement(IDocument document, IElement htmlElement)
		{
			foreach (var sanitizer in sanitizers)
			{
				if (sanitizer.CanSanitize(htmlElement))
				{
					htmlElement = sanitizer.Sanitize(document, htmlElement);
					if (htmlElement == null) // If the element is removed
					{
						break;
					}
				}
			}

			if (htmlElement != null)
			{
				foreach (var childElement in htmlElement.Children)
				{
					ConvertFromHtmlElement(document, childElement);
				}
			}
		}
	}
}