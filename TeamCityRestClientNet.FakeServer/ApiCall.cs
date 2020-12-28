using System;
using System.Collections.Generic;
using System.Linq;
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
            this.Request = request;
            this.Locators = new Dictionary<string, string>();
            ParseSegments(request.RequestUri.Segments);
            this.QueryParameters = new Dictionary<string, string[]>();
            ParseQuery(request.RequestUri.Query);
        }

        public HttpRequestMessage Request { get; private set; }
        public HttpMethod Method => Request.Method;
        public string RequestPath => Request.RequestUri.AbsolutePath;
        public Dictionary<string, string[]> QueryParameters { get; private set; }
        public HttpRequestHeaders RequestHeaders => Request.Headers;
        public HttpContent Content => Request.Content;
        public string Resource { get; private set; }
        public string Locator { get; private set; }
        public Dictionary<string, string> Locators { get; private set; }
        public bool HasLocators => Locators != null && Locators.Count > 0 || !String.IsNullOrEmpty(Locator);
        public string GetLocatorValue(string locator)
        {
            if (!HasLocators)
                throw new InvalidOperationException("ApiCall has no locators.");

            if (Locators.ContainsKey(locator))
                return Locators[locator];
            else
                return Locator;
        }
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
                else if (i == 4)
                {
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

        private void ParseQuery(string query)
        {
            if (!String.IsNullOrEmpty(query))
            {
                var parameters = query.Split('&');
                foreach (var parameter in parameters)
                {
                    var parts = parameter.Split('=');
                    var key = parts[0].Trim().ToLower();
                    var val = parts[1]?.Trim();

                    if (!this.QueryParameters.ContainsKey(key)) 
                    {
                        this.QueryParameters.Add(key, new string[0]);
                    }

                    if (!String.IsNullOrEmpty(val))
                    {
                        var values = this.QueryParameters[key];
                        this.QueryParameters[key] = values.Append(val).ToArray();
                    }
                }
            }
        }
    }
}