using CLImber.Wrappers;
using System;

namespace CLImber.Tests.Wrappers
{
    internal class TestResponse : IResponse
    {
        public int StatusCode => throw new NotImplementedException();
        public string Body => throw new NotImplementedException();
    }
}
