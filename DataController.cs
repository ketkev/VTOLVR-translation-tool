﻿using CsvHelper;
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

        public List<string> Languages { get; set; }
        public string CurrentLanguage { get; set; }

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

        public IEnumerable<dynamic> GetUnfilledData()
        {
            return from list in Files
                   from item in list.Data
                   where (string)((IDictionary<string, object>)item)[CurrentLanguage] == ""
                   select item;
        }

        public void CreateLanguage(string languageCode)
        {
            IEnumerable<dynamic> data = GetAllData();

            foreach (dynamic item in data)
            {
                ((IDictionary<String, object>)item).TryAdd(languageCode, string.Empty);
            }
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
                try
                {
                    using StreamReader reader = new StreamReader(path);
                    using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    var records = csv.GetRecords<dynamic>().ToList();

                    files.Add(new File(path, records));

                    Languages = csv.Context.HeaderRecord.Skip(1).Skip(1).ToList();
                    CurrentLanguage = Languages.FirstOrDefault();

                    if (csv.Context.HeaderRecord.ToList().FindAll(item => item == "Key" || item == "Description" || item == "en").Count != 3)
                    {
                        throw new InvalidCsvException($"Invalid CSV. Error in {path}. Make sure your header contains a Key, Description and en column.");
                    }
                }
                catch (CsvHelper.BadDataException exception)
                {
                    throw new InvalidCsvException($"Invalid CSV. Error in {path} on line {exception.ReadingContext.RawRow}");
                }
            }

            return files;
        }
    }
}
