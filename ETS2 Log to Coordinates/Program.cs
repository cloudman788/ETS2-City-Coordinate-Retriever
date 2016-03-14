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
            Dictionary<string, string> cityConversionTable = new Dictionary<string, string>();

            string cityDirectory = ConfigurationManager.AppSettings["CityDirectory"];
            string[] files = Directory.GetFiles(cityDirectory);
            int numberOfCities = files.Length;

            for (int i = 0; i < numberOfCities; i++)
            {
                StreamReader readCity = new StreamReader(files[i]);
                string line;
                //string output = read.ReadToEnd();
                //string[] outputArray = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //read.Close();
                try
                {
                    string cityName = "";
                    string cityRealName = "";
                    while ((line = readCity.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith("city_data: city."))
                        {
                            //int nameIndex = line.IndexOf("\"");
                            //string cityName = line.Substring(nameIndex + 1,
                            //    line.IndexOf("\"", nameIndex + 1) - nameIndex - 1);
                            cityName = line.Replace("city_data: city.", "");
                            if (cityName.Contains("{"))
                            {
                                cityName = cityName.Remove(cityName.IndexOf("{"));
                            }
                            cityName.Replace(" ", "");
                        }
                        if (line.TrimStart().StartsWith("city_name:"))
                        {
                            int nameIndex = line.IndexOf("\"");
                            cityRealName = line.Substring(nameIndex + 1,
                                line.IndexOf("\"", nameIndex + 1) - nameIndex - 1);
                        }
                    }
                    cityConversionTable.Add(cityName, cityRealName);
                    Console.WriteLine(cityName + " - " + cityRealName);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }

            string inputFile = ConfigurationManager.AppSettings["InputFile"];
            StreamReader read = new StreamReader(inputFile);
            string output = read.ReadToEnd();
            string[] outputArray = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            read.Close();

            Cities cities = new Cities();

            string previousX = "";
            string previousY = "";
            string previousZ = "";

            foreach (string line in outputArray)
            {
                string lineContent = line.Replace(" ", "");
                string[] lineContentArray = lineContent.Split(new char[] { ';' });
                if (
                    !(lineContentArray[2] == previousX && lineContentArray[3] == previousY &&
                      lineContentArray[4] == previousZ))
                {
                    try
                    {
                        City city = new City();
                        city.gameName = lineContentArray[0];
                        city.realName = cityConversionTable[lineContentArray[0]];
                        city.x = lineContentArray[2];
                        city.y = lineContentArray[3];
                        city.z = lineContentArray[4];
                        cities.citiesList.Add(city);

                        previousX = city.z = lineContentArray[2];
                        previousY = city.z = lineContentArray[3];
                        previousZ = city.z = lineContentArray[4];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            string jsonCitiesList = Newtonsoft.Json.JsonConvert.SerializeObject(cities, Formatting.Indented);
            Console.Write(jsonCitiesList);
            string outputFile = ConfigurationManager.AppSettings["OutputFile"];
            StreamWriter write = new StreamWriter(outputFile);
            write.Write(jsonCitiesList);
            write.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-- Done, have fun :-)");
            Console.ReadLine();
        }
    }

    class Cities
    {
        public List<City> citiesList = new List<City>();
    }

    class City
    {
        public string gameName = "";
        public string realName = "";

        public string x = "";
        public string y = "";
        public string z = "";
    }
}
