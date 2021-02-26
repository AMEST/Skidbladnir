using System;
using System.Collections.Generic;
using System.Linq;

namespace Skidbladnir.Storage.Abstractions
{
    public class FileInfo
    {
        public FileInfo(string filePath, long size, DateTime createdDate, IDictionary<string, string> attributes)
        {
            FilePath = filePath;
            Size = size;
            CreatedDate = createdDate;
            Attributes = attributes;
        }

        public string FilePath { get; }

        public string FileName
        {
            get
            {
                var segments = FilePath?.Split('/');
                if (segments == null || segments.Length == 0)
                    return null;

                return segments.Last();
            }
        }

        public long Size { get; }

        public DateTime CreatedDate { get; }

        public IDictionary<string, string> Attributes { get; }
    }
}