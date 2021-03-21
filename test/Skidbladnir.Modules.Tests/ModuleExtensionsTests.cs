using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Skidbladnir.Modules.Tests
{
    public class ModuleExtensionsTests
    {
        [Fact]
        public void ShouldAddUniqueModules_WhenModulesReferenceDuplicate()
        {
            var modulesList = ModuleExtensions.GetModulesRecursively(typeof(ModuleWithDupes));

            Assert.Equal(3, modulesList.Length);
        }

        [Fact]
        public void ShouldThrow_WhenDependsModuleNotImplementModule()
        {
            Assert.Throws<ModuleInitializeException>(() => ModuleExtensions.GetModulesRecursively(typeof(FailModule)));
        }

        [Fact]
        public void ShouldRegisterAllServices_WhenRegistrationInModule()
        {
            var serviceCollection = A.Fake<IServiceCollection>();
            var collection = new List<ServiceDescriptor>();
            A.CallTo(() => serviceCollection.Add(A<ServiceDescriptor>._))
                .Invokes(call => collection.Add((ServiceDescriptor)call.Arguments[0]));

            serviceCollection.AddSkidbladnirModules<FirstModule>();

            Assert.True(collection.Any(s => s.ImplementationType == typeof(ModuleWithRegistration) && s.Lifetime == ServiceLifetime.Singleton ));
            Assert.True(collection.Any(s => s.ImplementationType == typeof(ModuleExtensionsTests) && s.Lifetime == ServiceLifetime.Scoped ));
        }
    }

    internal class ModuleWithDupes : Module
    {
        public override Type[] DependsModules => new[] {typeof(TestModuleA), typeof(DependedModuleA)};
    }

    internal class FirstModule : Module
    {
        public override Type[] DependsModules => new[] { typeof(ModuleWithDupes), typeof(ModuleWithRegistration)};
    }

    internal class ModuleWithRegistration : Module
    {
        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton<ModuleWithRegistration>();
            services.AddScoped<ModuleExtensionsTests>();
        }
    }

}
