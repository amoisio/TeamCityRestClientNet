using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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
    /// 
    /// ApiCall.RequestPath task the form: [resource]/[locator]/[property]/[descriptor]
    /// - Only [resource] is mandatory, all other placeholders are optional.
    /// - [locator] can be a single value, or a TeamCity locator string.
    ///     - As a single value, the locator is unnamed and its value is stored in _locator
    ///     - Locator string is of the form "[name]:[value], ..."
    /// </summary>
    public class ApiCall
    {
        private string _locator;
        private readonly Dictionary<string, string> _locators;
        public ApiCall(HttpRequestMessage request)
        {
            this.Request = request;
            this.Content = request.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
            this._locators = new Dictionary<string, string>();
            ParseSegments(request.RequestUri.Segments);
            this.QueryParameters = new Dictionary<string, string[]>();
            ParseQuery(request.RequestUri.Query);
        }

        public HttpRequestMessage Request { get; private set; }
        public HttpMethod Method => Request.Method;
        public string RequestPath => Request.RequestUri.AbsolutePath;
        public Dictionary<string, string[]> QueryParameters { get; private set; }
        public HttpRequestHeaders RequestHeaders => Request.Headers;
        public string Content { get; }
        public T JsonContentAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }

        public string Resource { get; private set; }
        public bool HasLocators => _locators != null && _locators.Count > 0 || !String.IsNullOrEmpty(_locator);
        public bool HasNamedLocator(string locatorName) => _locators.ContainsKey(locatorName);

        public string GetLocatorOrDefault(string locatorName = null)
        {
            try 
            {
                return GetLocatorValue(locatorName);
            } 
            catch 
            {
                return default(string);    
            }
        }
        public string GetLocatorValue(string locatorName = null)
        {
            if (!HasLocators)
                throw new InvalidOperationException("ApiCall has no locators.");

            if (String.IsNullOrEmpty(locatorName))
            {
                if (!String.IsNullOrEmpty(_locator))
                    return _locator;
                
                if (_locators.Count == 1)
                {
                    var value = _locators.First().Value;
                    if (!String.IsNullOrEmpty(value))
                        return value;
                }
            } 
            else 
            {
                if (_locators.ContainsKey(locatorName) && !String.IsNullOrEmpty(_locators[locatorName]))
                    return _locators[locatorName];
            }

            throw new InvalidOperationException("Locator not found.");
        }
        public string Property { get; private set; }
        public string Descriptor { get; private set; }
        public bool RespondAsStream
            => Descriptor == "contents" || RequestPath.EndsWith("downloadBuildLog.html");
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
                        foreach(var locator in ParseLocators(value))
                        {
                            _locators.Add(locator.name, locator.value);
                        }
                    }
                    else
                    {
                        this._locator = value;
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

        private static IEnumerable<(string name, string value)> ParseLocators(string value)
        {
            var locatorStrings = value.Split(',');
            foreach (var locatorString in locatorStrings)
            {
                var locatorParts = locatorString.Split(':');
                yield return (locatorParts[0]?.Trim(), locatorParts[1]?.Trim());
            }
        }

        private void ParseQuery(string query)
        {
            if (!String.IsNullOrEmpty(query))
            {
                var parameters = query.Substring(1).Split('&');
                foreach (var parameter in parameters)
                {
                    var parts = parameter.Split('=');
                    var key = parts[0].Trim().ToLower();
                    var val = WebUtility.UrlDecode(parts[1]?.Trim());

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