using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TeamCityRestClientNet.FakeServer
{
    /// <summary>
    /// ApiCall decomposes a Team City Rest Api queries into contextual fields.
    /// The Team City REST API Url structure is documented here https://www.jetbrains.com/help/teamcity/rest-api.html#URL+Structure.
    ///
    /// ApiCall makes the following assumptions about every query:
    /// 1. _authType_ is not provided in the URL.
    /// 2. the root path of the TeamCity Rest Api is app/rest.
    /// 3. _apiVersion_ is not provided in the URL.
    /// </summary>
    public class ApiCall
    {
        public ApiCall(HttpRequestMessage request)
        {
            this.Method = request.Method;
            this.RequestPath = request.RequestUri.AbsolutePath;
            ParseSegments(request.RequestUri.Segments);
            this.QueryParameters = new Dictionary<string, string[]>();
            this.RequestHeaders = request.Headers;
        }

        public HttpMethod Method { get; private set; }
        public string RequestPath { get; private set; }
        public Dictionary<string, string[]> QueryParameters { get; private set; }
        public HttpRequestHeaders RequestHeaders { get; private set; }
        public string Resource { get; private set; }
        public string Locator { get; private set; }
        public Dictionary<string, string> Locators { get; private set; }
        public bool HasLocators => Locators != null && Locators.Count > 0;
        public string Property { get; private set; }
        public string Descriptor { get; private set; }
        private void ParseSegments(string[] segments)
        {
            int count = segments.Length;
            if (count < 4)
                throw new ArgumentException("URL expected to contain the Team City resource specifier.");

            for (int i = 3; i < count; i++)
            {
                var value = WebUtility.UrlDecode(segments[i].TrimEnd('/'));
                if (i == 3)
                    this.Resource = value;
                else if (i == 4) {
                    if (value.Contains(':')) 
                    {
                        this.Locators = ParseLocators(value);
                    } 
                    else 
                    {
                        this.Locator = value;
                    }
                }
                else if (i == 5)
                    this.Property = value;
                else if (i == 6)
                    this.Descriptor = value;
                else
                    continue;
            }
        }

        private static Dictionary<string, string> ParseLocators(string value)
        {
            Dictionary<string, string> locators = new Dictionary<string, string>();
            var locatorStrings = value.Split(',');
            foreach (var locatorString in locatorStrings)
            {
                var locatorParts = locatorString.Split(':');
                locators.Add(locatorParts[0]?.Trim(), locatorParts[1]?.Trim());
            }
            return locators;
        }
    }
}