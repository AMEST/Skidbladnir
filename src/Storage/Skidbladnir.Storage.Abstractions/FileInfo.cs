using System;
using System.Collections.Generic;
using System.Linq;

namespace Skidbladnir.Storage.Abstractions
{
    /// <summary>
    /// File information in storage
    /// </summary>
    public class FileInfo
    {
        public FileInfo(string filePath, long size, DateTime createdDate)
        {
            FilePath = filePath.Replace("\\","/");
            Size = size;
            CreatedDate = createdDate;
        }

        /// <summary>
        /// Full file path in store
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// File name from  full path
        /// </summary>
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

        /// <summary>
        /// File size in path
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// File creation date
        /// </summary>
        public DateTime CreatedDate { get; }
    }
}