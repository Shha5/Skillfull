using Autofac.Extras.Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Skillfull_Api_Tests
{
    internal class ApiTestsBase<T> where T : class
    {
        protected AutoMock AutoMock;
        protected T SystemUnderTest { get; set; }
        protected Moq.Mock<IConfiguration> ConfigMock { get; set; }

        [SetUp]
        public virtual void Setup()
        {
            AutoMock = AutoMock.GetLoose();
            SystemUnderTest = AutoMock.Create<T>();
            ConfigMock = AutoMock.Mock<IConfiguration>();
        }

        [TearDown]
        public virtual void Teardown()
        {
            AutoMock.Dispose();
        }
    }
}