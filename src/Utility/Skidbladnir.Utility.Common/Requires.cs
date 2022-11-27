using System;

namespace Skidbladnir.Utility.Common
{
    /// <summary>
    ///     A static helper class that includes various parameter checking routines.
    /// </summary>
    public static class Requires
    {
        /// <summary>
        ///     Throws ArgumentNullException if the given argument is null.
        /// </summary>
        public static void ArgumentNotNull(this object argumentValue, string argumentName)
        {
            ObjectNotNull(argumentValue, argumentName);
        }

        /// <summary>
        ///     Throws ArgumentNullException if the object is null.
        /// </summary>
        public static void ObjectNotNull(object obj, string message = "Object can't be null")
        {
            if (obj == null)
                throw new ArgumentException(message);
        }

        /// <summary>
        ///     Throws an exception if the tested string argument is null or an empty string
        /// </summary>
        public static void StringNotNullOrEmpty(string text, string message = "Can't be null or empty")
        {
            if (string.IsNullOrEmpty(text))
                throw new NullReferenceException(message);
        }
    }
}