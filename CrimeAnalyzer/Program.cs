using System;
//using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrimeAnalyzer
{
    class Program
    {

        public List<CrimeData> crimeData = new List<CrimeData>();

        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 2)
            {
                Console.WriteLine("Not enough arguments. Please enter a source file and report file.");
                return;
            }

            string sFile = args[2];
            string rFile = args[1];

//            List<CrimeData> crimeData = new List<CrimeData>();

            StreamReader sourceFile = null;

            try
            {
                sourceFile = new StreamReader(sFile);
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

                    CrimeData crimes = new CrimeData(year, population, violentCrime, murder, rape, robbery, aggravatedAssault, propertyCrime, burglary, theft, mvTheft);
                    crimeData.Add(crimes);
                }
            }

            catch (FormatException)
            {
                Console.WriteLine("Error: Source file contains data that is not of the right type. Make sure all data is numerical.");
                return;
            }

            catch (Exception)
            {
                Console.WriteLine("Error: Source file can't be opened.");
                return;
            }

            finally
            {
                if (sourceFile != null)
                {
                    sourceFile.Close();
                }
            }

            //CreateReport(crimeData, rFile);

//        }

//        public static void CreateReport<CrimeStats>(List<CrimeStats> data, string rFile)
//        {
            StreamWriter report = null;

            try
            {
                report = new StreamWriter(rFile);
                report.WriteLine("Crime Analyzer Report");

                var yearData = from CrimeData in crimeData select CrimeData.year;
                int numYear = yearData.Count();
                int begYear = yearData.Max();
                int endYear = yearData.Min();
                report.WriteLine("Period: {0}-{1} ({2} years)", begYear, endYear, numYear);

            }

            catch (Exception e)
            {
                Console.WriteLine("Unable to create report: {0}", e.Message);
            }

            finally
            {
                if (report != null)
                {
                    report.Close();
                }
            }
        }
    }
}
