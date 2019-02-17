using Xunit;
using ServerFTP;
using ClientFTP;
using System.IO;

namespace SimpleFTP.Test
{
    public class UnitTest1
    {
        private DirectoryInfo GetDir()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            string dirToServer = dir.FullName + "/testdir";
            dir = new DirectoryInfo(dirToServer);
            return dir;
        }

        [Fact]
        public void IncorrectCommandTest()
        {
            var server = new Server();
            var client = new Client();

            var serverTask = server.Work(8888);
            var responce = client.GetResponce(8888, $"void").Result;

            Assert.Equal("Неверный формат команды", responce);
        }

        [Fact]
        public void NumberOfObjectsInDirTest()
        {
            var dir = GetDir();

            int objectsNumber = dir.GetDirectories().Length + dir.GetFiles().Length;

            var server = new Server();
            var client = new Client();

            var serverTask = server.Work(8888);
            var responce = client.GetResponce(8888, $"1 {dir.FullName}").Result;

            Assert.Equal(objectsNumber.ToString(), responce.Substring(0, responce.IndexOf(' ')));
        }

        [Fact]
        public void ListOfFilesAndSubDirsFromServer()
        {
            var dir = GetDir();

            int objectsNumber = dir.GetDirectories().Length + dir.GetFiles().Length;
            var contentList = objectsNumber.ToString();

            foreach (var subDir in dir.GetDirectories())
            {
                contentList += $" {subDir.Name} - true ";
            }

            foreach (var file in dir.GetFiles())
            {
                contentList += $" {file.Name} - false ";
            }

            var server = new Server();
            var client = new Client();

            var serverTask = server.Work(8888);
            var responce = client.GetResponce(8888, $"1 {dir.FullName}").Result;

            Assert.Equal(contentList, responce);
        }

        [Fact]
        public void FileContentTest()
        {
            var dir = GetDir();
            var filePath = dir.FullName + "/hello.txt";

            var fileContent = File.ReadAllText(filePath);
            var fileLength = (new FileInfo(filePath)).Length;

            var expectedResponce = $"{fileLength} {fileContent}";

            var server = new Server();
            var client = new Client();

            var serverTask = server.Work(8888);
            var responce = client.GetResponce(8888, $"2 {filePath}").Result;

            Assert.Equal(expectedResponce, responce);
        }
    }
}
