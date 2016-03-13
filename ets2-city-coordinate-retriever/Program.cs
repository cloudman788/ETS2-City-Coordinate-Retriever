using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using InputManager;

namespace ets2_city_coordinate_retriever
{

    class Program
    {
        private const string CityCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_";

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        static void Main()
        {

            bool debugMode = bool.Parse(ConfigurationManager.AppSettings["DebugMode"]);

            Console.WriteLine(@"After pressing {ENTER} you will have 10 seconds to switch to ETS2 / ATS for coordinate capture. 

Once the operation is complete, all coordinates will be stored in:

%USERPROFILE%\Documents\{ETS2/ATS Folder}\game.log.txt" + "\n");

            // Wait until user presses {ENTER}
            Console.ReadLine();

            const int numberOfSeconds = 10;
            for (int i = numberOfSeconds; i >= 1; i--)
            {
                Console.Write(i + " ");
                Thread.Sleep(1000);
            }

            string cityDirectory = ConfigurationManager.AppSettings["CityDirectory"];

            string[] files = Directory.GetFiles(cityDirectory);
            int numberOfCities = files.Length;

            if (debugMode)
            {
                Console.WriteLine($"Processing {numberOfCities} cities");
            }

            for (int i = 0; i < numberOfCities; i++)
            {
                //string cityName = Path.GetFileNameWithoutExtension(files[i]);
                StreamReader read = new StreamReader(files[i]);
                String output = read.ReadToEnd();
                String[] outputArray = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                read.Close();
                try
                {
                    foreach (var line in outputArray)
                    {
                        if (line.Trim().StartsWith("city_data: city."))
                        {
                            //int nameIndex = line.IndexOf("\"");
                            //string cityName = line.Substring(nameIndex + 1,
                            //    line.IndexOf("\"", nameIndex + 1) - nameIndex - 1);
                            string cityName = line.Replace("city_data: city.", "");
                            if (cityName.Contains("{"))
                            {
                                cityName = cityName.Remove(cityName.IndexOf("{"));
                            }
                            if (debugMode)
                            {
                                Console.WriteLine($"Processing city {cityName} ({i + 1} / {numberOfCities})");
                            }

                            Keyboard.KeyPress(Keys.Oemtilde);
                            Thread.Sleep(250);
                            Keyboard.KeyPress(Keys.Back);
                            Thread.Sleep(50);
                            Keyboard.KeyPress(Keys.G);
                            Thread.Sleep(50);
                            Keyboard.KeyPress(Keys.O);
                            Thread.Sleep(50);
                            Keyboard.KeyPress(Keys.T);
                            Thread.Sleep(50);
                            Keyboard.KeyPress(Keys.O);
                            Thread.Sleep(50);
                            Keyboard.KeyPress(Keys.Space, 10);
                            Thread.Sleep(50);
                            foreach (char index in cityName)
                            {
                                if (CityCharacters.Contains(index.ToString().ToUpper()))
                                {
                                    Keyboard.KeyPress(
                                        (Keys) System.Enum.Parse(typeof (Keys), index.ToString().ToUpper()));
                                    Thread.Sleep(50);
                                }
                            }
                            Keyboard.KeyPress(Keys.Enter);
                            Thread.Sleep(3000);
                            Keyboard.KeyPress(Keys.Oemtilde);
                            Thread.Sleep(100);
                            Keyboard.KeyPress(Keys.F11);
                            Thread.Sleep(100);
                            foreach (char index in cityName)
                            {
                                if (CityCharacters.Contains(index.ToString().ToUpper()))
                                {
                                    Keyboard.KeyPress(
                                        (Keys)System.Enum.Parse(typeof(Keys), index.ToString().ToUpper()));
                                    Thread.Sleep(50);
                                }
                            }
                            Keyboard.KeyPress(Keys.Escape);
                            Thread.Sleep(100);
                            if (!debugMode)
                            {
                                DrawProgressBar(i + 1, numberOfCities, new decimal(.5)*Console.WindowWidth, '=', '-');
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    for (int a = 0; a <= 20; a++)
                    {
                        Keyboard.KeyPress(Keys.Back);
                    }
                    Thread.Sleep(1000);
                }
            }


            // Complete
            SetForegroundWindow(GetConsoleWindow());
            Console.WriteLine("Complete! Press {ENTER} to exit.");
            Console.ReadLine();
        }

        private static void DrawProgressBar(int complete, int maxVal, decimal barSize, char completeProgressChar, char incompleteProgressChar)
        {
            Console.CursorVisible = false;
            int left = Console.CursorLeft;
            decimal perc = (decimal)complete / maxVal;
            int chars = (int)Math.Floor(perc / (1 / barSize));
            string p1 = string.Empty, p2 = string.Empty;

            for (int i = 0; i < chars; i++) p1 += completeProgressChar;
            for (int i = 0; i < barSize - chars; i++) p2 += incompleteProgressChar;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('[');
            Console.Write(p1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(p2);
            Console.Write("]");

            Console.ResetColor();
            string percentage = (perc * 100).ToString("N2");
            Console.Write(" {0}%", percentage);

            if (percentage != "100.00")
            {
                Console.CursorLeft = left;
            }
            else {
                Console.WriteLine();
            }
        }
    }
}