using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace TeamCityRestClientNet.Tests
{
    public class ApiCall
    {
        public HttpMethod Method { get; private set; }
        public string RequestPath { get; private set; }
        public Dictionary<string, string> Locators { get; private set; }
        public Dictionary<string, string[]> QueryParameters { get; private set; }
        public HttpRequestHeaders RequestHeaders { get; private set; }
        public ApiCall(HttpRequestMessage request)
        {
            this.Method = request.Method;
            this.RequestPath = WebUtility.UrlDecode(request.RequestUri.AbsolutePath);
            this.QueryParameters = new Dictionary<string, string[]>();
            this.RequestHeaders = request.Headers;

            var segments = request.RequestUri.Segments;
            var locators = HasLocators(segments);
            if (locators.hasLocator)
            {
                this.Locators = ParseLocators(locators.decodedSegment);
            }
        }

        private static Regex locatorRegex = new Regex(@"(([a-zA-Z]+:[a-zA-Z0-9_\.]+),?)+?");
        private static (bool hasLocator, string decodedSegment) HasLocators(string[] segments)
        {
            foreach (var segment in segments)
            {
                var decodedSegment = WebUtility.UrlDecode(segment);
                var match = locatorRegex.Match(decodedSegment);
                if (match.Success)
                {
                    return (true, decodedSegment);
                }
            }

            return (false, null);
        }

        private static Dictionary<string, string> ParseLocators(string segment)
        {
            Dictionary<string, string> locators = new Dictionary<string, string>();
            var matches = locatorRegex.Matches(segment);
            foreach (Match match in matches)
            {
                if (match.Success) 
                {
                    var locator = match.Groups.Values.ToArray()[1].Value;
                    var parts = locator.Split(':');
                    locators.Add(parts[0], parts[1]);
                }
            }
            return locators;
        }
    }
}