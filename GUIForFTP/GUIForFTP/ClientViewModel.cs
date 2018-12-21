
using System.Collections.Generic;

namespace GUIForFTP
{
    class ClientViewModel
    {
        readonly Client client = new Client();

        List<ObjectInfo> objects = new List<ObjectInfo>();

        public void StartConnection(int port, string addres)
        {
            var command = @"1 ...\";
            var responce = client.GetResponce(port, addres, command).Result;

            SetInfoToListOfObjects(responce);
        }

        private void SetInfoToListOfObjects(string info)
        {
            var objectsArray = info.Split(' ');
            if (int.TryParse(objectsArray[0], out int numberOfObjects))
            {
                for (int i = 1; i <= numberOfObjects; i++)
                {
                    if (objectsArray[2*i] == "true")
                    {
                        var obj = new ObjectInfo(true, objectsArray[2*i-1]);
                        objects.Add(obj);
                    }
                    else
                    {
                        var obj = new ObjectInfo(false, objectsArray[2 * i - 1]);
                        objects.Add(obj);
                    }
                }
            }
        }
    }
}
