using System;
using Skidbladnir.Modules.Sample.Core;

namespace Skidbladnir.Modules.Sample
{
    public class StartupModule : Module
    {
        public override Type[] DependsModules => new[]
            {typeof(CoreSampleModule), typeof(ScheduledSpeedModule), typeof(LongRunningModule)};
    }
}