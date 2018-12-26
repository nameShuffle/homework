﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GUIForFTP
{
    /// <summary>
    /// Класс, реализующий логику клиентской стороны. Позволяет подключаться
    /// к серверу и передавать ему запросы, введенные с клавиатуры.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Метод для получения списка файлов и поддиректорий по указанному пути.
        /// </summary>
        /// <param name="command">Команда, сформированная для работы с сервером.</param>
        public async Task<string> GetDirectoryList(int port, string addres, string command)
        {
            string data = null;

            try
            {
                using (var client = new TcpClient(addres, port))
                {
                    var stream = client.GetStream();

                    var writer = new StreamWriter(stream); 
                 
                    await writer.WriteLineAsync(command);
                    await writer.FlushAsync();

                    var reader = new StreamReader(stream);
                    
                    data = await reader.ReadToEndAsync();

                    if (data == "-1")
                    {
                        throw new Exception();
                    }

                    return data;
                }
            }
            catch
            {
                throw new Exception("Не удается получить информацию от сервера!");
            }
        }

        /// <summary>
        /// Метод для скачивания файла в указанную папку.
        /// </summary>
        /// <param name="command">Команда, сформированная для работы с сервером.</param>
        /// <param name="pathToDownload">Путь для скачивания файлов.</param>
        /// <returns></returns>
        public async Task DownloadFile(int port, string addres, string command, string pathToDownload)
        {
            try
            {
                using (var client = new TcpClient(addres, port))
                {
                    var stream = client.GetStream();

                    var writer = new StreamWriter(stream);

                    await writer.WriteLineAsync(command);
                    await writer.FlushAsync();
                    
                    var reader = new StreamReader(stream);
                    var responce = await reader.ReadLineAsync();
                    if (responce == "-1")
                    {
                        throw new Exception();
                    }

                    var fileStream = File.OpenWrite(pathToDownload);
                    stream.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch
            {
                throw new Exception("Не удается скачать файл! Проверьте корректность введенных данных.");
            }

        }
    }
}
