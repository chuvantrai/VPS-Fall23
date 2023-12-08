global using Xunit;
using Xunit.Abstractions;

namespace xUnitTest
{
    public class GlobalBase
    {
        protected readonly ITestOutputHelper output;
        public GlobalBase(ITestOutputHelper output)
        {
            this.output = output;
        }
    }
}