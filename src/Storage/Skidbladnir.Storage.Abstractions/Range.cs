using System;

namespace Skidbladnir.Storage.Abstractions
{
    public class Range
    {
        public Range(long from, long to)
        {
            if (from < 0)
                throw new ArgumentException("Can't be less 0", nameof(from));
            if (to < 0)
                throw new ArgumentException("Can't be less 0", nameof(to));
            From = from;
            To = to;
        }

        public long From { get; }

        public long To { get; }
    }
}