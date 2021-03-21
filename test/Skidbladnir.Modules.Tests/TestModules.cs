using System;

namespace Skidbladnir.Modules.Tests
{
    public class TestModuleA : Module
    {
        public override Type[] DependsModules => new[] { typeof(DependedModuleA) };
    }

    public class TestModuleB : Module
    {
        public override Type[] DependsModules => new[] { typeof(DependedModuleB) };
    }

    public class DependedModuleA : RunnableModule
    {
    }

    public class DependedModuleB : RunnableModule
    {
    }

    public class FailModule : Module
    {
        public override Type[] DependsModules => new[] { typeof(string), typeof(DependedModuleA) };
    }
}