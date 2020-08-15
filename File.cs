using System;
using System.Collections.Generic;
using System.Text;

namespace VTOLVR_Translation_tool
{
    class File
    {
        public string path;
        public List<dynamic> Data;

        public File(string path, List<dynamic> data)
        {
            this.path = path;
            Data = data;
        }
    }
}
