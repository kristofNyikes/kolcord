using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolcordWebApi.Tests.Helpers
{
    public class TestTester
    {
        [Fact]
        public void TestSetup_ShouldWork()
        {
            var expected = 4;
            var actual = 2 + 2;
            Assert.Equal(expected, actual);
        }
    }
}
