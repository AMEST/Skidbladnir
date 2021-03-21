using System;

namespace Skidbladnir.Modules
{
    public class ModuleInitializeException : Exception
    {
        public ModuleInitializeException(string message)
            : base(message)
        {
        }
    }
}