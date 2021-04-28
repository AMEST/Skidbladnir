using System;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Skidbladnir.Modules.Tests
{
    public class ModulesConfigurationTests
    {
        private readonly IConfiguration _appConfiguration;
        private readonly ModulesConfiguration _configuration;

        public ModulesConfigurationTests()
        {
            _appConfiguration = A.Fake<IConfiguration>();
            _configuration = new ModulesConfiguration(_appConfiguration);
        }

        [Fact]
        public void ShouldGetConfigurationByInterfaces_WhenAddedConfigurationImplementation()
        {
            var config = new TestConfiguration();

            _configuration.Add(config);

            Assert.Equal(config, _configuration.Get<IFirstConfiguration>());
            Assert.Equal(config, _configuration.Get<ISecondConfiguration>());
        }

        [Fact]
        public void ShouldGetDefaultConfiguration_WhenConfigurationNotRegistered()
        {
            var fakeConfiguration = A.Fake<ISecondConfiguration>();

            var actual = _configuration.Get<ISecondConfiguration>(fakeConfiguration);

            Assert.Equal(fakeConfiguration, actual);
        }

        [Fact]
        public void ShouldThrow_WhenConfigurationNotRegistered()
        {
            Assert.Throws<ArgumentException>(() => _configuration.Get<ISecondConfiguration>());
        }

    }

    public class TestConfiguration: IFirstConfiguration, ISecondConfiguration
    {
        public string Option1 { get; set; }
        public bool Option2 { get; set; }
    }

    public interface IFirstConfiguration
    {
        public string Option1 { get; set; }
    }

    public interface ISecondConfiguration
    {
        public bool Option2 { get; set; }
    }
}