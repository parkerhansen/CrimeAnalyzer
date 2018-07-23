using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrimeAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Lenght > 2)
            {
                Console.WriteLine("Not enough arguments. Please enter a source file and report file.");
                return;
            }

            string sfile = args[2];
            string rfile = args[1];

            StreamReader sourceFile = null;

            try
            {
                sourceFile = StreamReader(sfile);
                sourceFile.ReadLine();

                while(sourceFile.EndOfStream == true)
                {
                    string row = sourceFile.ReadLine();
                    string[] column = row.Split(',');

                    int year = int.Parse(column[0]);
                    int population = int.Parse(column[1]);
                    int violentCrime = int.Parse(column[2]);
                    int murder = int.Parse(column[3]);
                    int rape = int.Parse(column[4]);
                    int robbery = int.Parse(column[5]);
                    int aggravatedAssault = int.Parse(column[6]);
                    int propertyCrime = int.Parse(column[7]);
                    int burglary = int.Parse(column[8]);
                    int theft = int.Parse(column[9]);
                    int mvTheft = int.Parse(column[10]);

                    if (column.Length < 11 || column.Length > 11)
                    {
                        Console.WriteLine("Row does not contain the correct number of data elements. Each row should have 11 data elements.");
                        continue;
                    }
                }
            }

            catch (Exception)
            {
                Console.WriteLine("Error: Source file can't be opened.");
                return;
            }

            catch (FormatException)
            {
                Console.WriteLine("Error: Source file contains data that is not of the right type. Make sure all data is numerical.");
                return;
            }

            finally
            {
                if (sourceFile != null)
                {
                    sourceFile.Close();
                }
            }
        }
    }
}
