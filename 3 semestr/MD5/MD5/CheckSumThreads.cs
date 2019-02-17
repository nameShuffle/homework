using System;
using System.IO;
using System.Threading.Tasks;

namespace MD5
{
    class CheckSumThreads
    {
        public string CheckSumFull(string dir)
        {
            string CheckSum = "";
            string checkSumForFiles = "";
            string checkSumForDirs = "";
            if (File.Exists(dir))
                checkSumForFiles += CheckSumFile(dir);
            else if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                DirectoryInfo[] subDirInfo = dirInfo.GetDirectories();
                Task<string>[] tasks = new Task<string>[subDirInfo.Length];
                for (int i = 0; i < subDirInfo.Length; i++)
                {
                    tasks[i] = new Task<string>(() => CheckSumFull(subDirInfo[i].FullName));
                    tasks[i].Start();
                }
                for (int i = 0; i < tasks.Length; i++)
                {
                    checkSumForDirs += tasks[i].Result;
                }
                FileInfo[] fileInfo = dirInfo.GetFiles();
                Task<string>[] tasksFiles = new Task<string>[fileInfo.Length];
                for (int j = 0; j < fileInfo.Length; j++)
                {
                    tasksFiles[j] = new Task<string>(() => CheckSumFile(fileInfo[j].FullName));
                    tasksFiles[j].Start();
                }
                for (int j = 0; j < tasks.Length; j++)
                {
                    checkSumForFiles += tasksFiles[j].Result;
                }
            }

            CheckSum = checkSumForFiles + checkSumForDirs;

            return CheckSum;
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
