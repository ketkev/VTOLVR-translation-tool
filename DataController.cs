using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace VTOLVR_Translation_tool
{
    class DataController
    {
        List<File> Files;

        public DataController()
        {
            string[] CSVPaths = GetPaths();
            Files = ReadCSVFiles(CSVPaths);
        }


        public IEnumerable<dynamic> GetAllData()
        {
            return from list in Files
                   from item in list.Data
                   select item;
        }

        public void GetUnfilledData()
        {

        }

        public void GetLanguages()
        {

        }

        public void Save()
        {
            foreach (File file in Files)
            {
                using StreamWriter writer = new StreamWriter(file.path);
                using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(file.Data);
            }
        }

        private string[] GetPaths()
        {
            string cwd = Directory.GetCurrentDirectory();
            return Directory.GetFiles(cwd, "*.csv");
        }

        private List<File> ReadCSVFiles(string[] paths)
        {
            List<File> files = new List<File>();

            foreach (string path in paths)
            {
                using StreamReader reader = new StreamReader(path);
                using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<dynamic>().ToList();


                files.Add(new File(path, records));
            }

            return files;
        }
    }
}
