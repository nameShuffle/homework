using Xunit;
using ServerFTP;
using ClientFTP;
using System.IO;

namespace SimpleFTP.Test
{
    public class UnitTest1
    {
        [Fact]
        public void NumberOfObjectsInDirTest()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            int objectsNumber = dir.GetDirectories().Length + dir.GetDirectories().Length;

            var server = new Server();
            var client = new Client();

            var serverTask = server.Work(8888);
            var responce = client.GetResponce(8888, dir.FullName).Result;

            Assert.Equal(objectsNumber.ToString(), responce.Substring(0, responce.IndexOf(' ')));
        }

        /*[Fact]
        public void ListOfFilesAndSubDirsFromServer()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            int objectsNumber = dir.GetDirectories().Length + dir.GetDirectories().Length;
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
            var responce = client.GetResponce(8888, dir.FullName).Result;

            Assert.Equal(contentList, responce);
        }*/
    }
}
