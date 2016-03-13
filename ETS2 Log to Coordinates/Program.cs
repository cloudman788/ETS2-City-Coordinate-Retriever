using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ETS2_Log_to_Coordinates
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        static void Main(string[] args)
        {
            string inputFile = ConfigurationManager.AppSettings["InputFile"];
            StreamReader read = new StreamReader(inputFile);
            string output = read.ReadToEnd();
            string[] outputArray = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            read.Close();

            Cities cities = new Cities();

            foreach (string line in outputArray)
            {
                string lineContent = line.Replace(" ", "");
                string[] lineContentArray = lineContent.Split(new char[] { ';' });
                cities.citiesList.Add(lineContentArray);
            }
            string jsonCitiesList = Newtonsoft.Json.JsonConvert.SerializeObject(cities, Formatting.Indented);
            Console.Write(jsonCitiesList);
            string outputFile = ConfigurationManager.AppSettings["OutputFile"];
            StreamWriter write = new StreamWriter(outputFile);
            write.Write(jsonCitiesList);
            write.Close();
            Console.ReadLine();
        }
    }

    class Cities
    {
         public List<string[]> citiesList = new List<string[]>();
    }
}
