using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.FakeServer;
using System.Linq.Expressions;

namespace TeamCityRestClientNet.Tests
{
    public static class ApiCallExtensions
    {
        public static void Assert(this ApiCall apiCall, HttpMethod expectedMethod)
            => Xunit.Assert.Equal(expectedMethod, apiCall.Method);

        public static void Assert(this ApiCall apiCall, string expectedRequestPath)
            => Xunit.Assert.Equal(expectedRequestPath, apiCall.RequestPath);

        public static void Assert(this ApiCall apiCall, HttpMethod expectedMethod, string expectedRequestPath)
        {
            Assert(apiCall, expectedMethod);
            Assert(apiCall, expectedRequestPath);
        }
    }

    public abstract class TestsBase
    {
        protected readonly TeamCityFixture _fixture;
        protected readonly TeamCity _teamCity;
        protected readonly string _serverUrl;
        protected readonly RedirectToFakeServer _handler;
        private List<ApiCall> ApiCalls => _handler.ApiCalls;


        public TestsBase() : this(new TeamCityFixture()) { }

        public TestsBase(TeamCityFixture fixture)
        {
            _fixture = fixture;
            _teamCity = fixture.TeamCity;
            _serverUrl = fixture.serverUrl;
            _handler = fixture.Handler;
        }


        internal ApiCallAssert ApiCall2 => new ApiCallAssert(ApiCalls);

        private void AssertApiCall(Predicate<ApiCall> predicate) => Xunit.Assert.Contains(ApiCalls, predicate);
        protected ApiCall ApiCall(HttpMethod method, string requestPath, Func<ApiCall, bool> filter = null)
        {
            var calls = ApiCalls.Where(call => call.Method == method && call.RequestPath == requestPath);
            if (filter != null)
                calls = calls.Where(filter);
            return calls.SingleOrDefault();
        }
    }

    class ApiCallAssert
    {
        private readonly List<ApiCall> _apiCalls;
        private HttpMethod _method;
        private string _requestPath;
        private bool _matchAny = false;

        public ApiCallAssert(List<ApiCall> apiCalls)
        {
            this._apiCalls = apiCalls;
        }

        public ApiCallAssert MatchAny()
        {
            _matchAny = true;
            return this;
        }

        public ApiCallAssert ExpectedRequest(HttpMethod method, string requestPath)
        {
            _method = method;
            _requestPath = requestPath;
            return this;
        }
        

        public void Assert()
        {
            ParameterExpression pe = Expression.Parameter(typeof(ApiCall), "apiCall");

            List<Expression> expressions = new List<Expression>();

            if (_method != null)
            {
                // apiCall.Method == _method
                Expression left = Expression.Property(pe, typeof(ApiCall).GetProperty("Method"));
                Expression right = Expression.Constant(_method, typeof(HttpMethod));
                expressions.Add(Expression.Equal(left, right));
            }

            if (_requestPath != null)
            {
                // apiCall.RequestPath == _requestPath
                Expression left = Expression.Property(pe, typeof(ApiCall).GetProperty("RequestPath"));
                Expression right = Expression.Constant(_requestPath, typeof(string));
                expressions.Add(Expression.Equal(left, right));
            }

            if (expressions.Count == 0)
            {
                throw new InvalidOperationException("At least one condition must be given.");
            }

            // (apiCall.Method == _method && apiCall.RequestPath == _requestPath)
            Expression predicateBody = expressions.Count == 1
                ? expressions[0]
                : expressions.Aggregate((e1, e2) => Expression.And(e1, e2));

            var le = Expression.Lambda<Predicate<ApiCall>>(predicateBody, new ParameterExpression[] { pe });
            var predicate = le.Compile();

            if (_matchAny)
                Xunit.Assert.Contains(_apiCalls, predicate);
            else
                Xunit.Assert.Single(_apiCalls, predicate);
        }
    }
}
