using System;
using System.IO;

namespace MD5
{
    class CheckSum
    {
        public string CheckSumFull(string dir)
        {
            string checkSum = "";
            if (File.Exists(dir))
                checkSum += CheckSumFile(dir);
            else if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] fileInfo = dirInfo.GetFiles();
                for (int i = 0; i < fileInfo.Length; i++)
                    checkSum += CheckSumFile(fileInfo[i].FullName);
                DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
                for (int i = 0; i < subDirInfo.Length; i++)
                    checkSum += CheckSumFull(subDirInfo[i].FullName);
            }

            return checkSum;
        }

        public string CheckSumFile(string filePath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    var checkSum = md5.ComputeHash(fileStream);
                    string checkSumString = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                    return checkSumString;
                }
            }
        }
    }
}
