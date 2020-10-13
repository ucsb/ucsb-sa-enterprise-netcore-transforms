using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Jdt;
using Moq;
using Ucsb.Sa.Enterprise.NetCore.Transforms.Console;
using Xunit;

namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Tests
{
    public class JsonTransformWrapperTest
    {
        [Fact]
        public void Test1()
        {
            var jsonLogger = new Mock<IJsonTransformationLogger>();

            JdtTransformWrapper wrapper = new JdtTransformWrapper(jsonLogger.Object);

            Assert.NotNull(wrapper);
        }
    }
}
