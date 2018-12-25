using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForFTP
{
    public class FileToDownload
    {
        public FileToDownload(string status)
        {
            Status = status;
        }

        public string Status { get; set; }
    }
}
