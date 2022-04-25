using System;
using System.IO;

namespace Common
{
    public class FileHelper
    {
        public static byte[] ReaderFileStream(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
