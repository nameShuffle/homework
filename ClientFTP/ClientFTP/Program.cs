using System;

namespace ClientFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 5555;

            var client = new Client();

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    return;
                }
                var responce = client.GetResponce(port, command);
                string answer = responce.Result;
                Console.WriteLine(answer);
            } 
        }
    }
}
