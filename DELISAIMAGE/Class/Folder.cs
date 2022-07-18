using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DELISAIMAGE.Class
{
    public static class Folder
    {
        public static string? Filepath { get; private set; }

        public static void Create(string filepath)
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            Filepath = $"{filepath}{@"\"}";
        }
    }
}
