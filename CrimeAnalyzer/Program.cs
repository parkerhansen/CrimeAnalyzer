﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrimeAnalyzer
{
    public class CrimeData
    {
        public int CrimeYear;
        public int CrimePop;
        public int CrimeVC;
        public int CrimeMurder;
        public int CrimeRape;
        public int CrimeRobbery;
        public int CrimeAggA;
        public int CrimePC;
        public int CrimeBurg;
        public int CrimeTheft;
        public int CrimeMVTheft;
    }

    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 2)
            {
                Console.WriteLine("Not enough arguments. Please enter a source file and report file.");
                return;
            }

            string sFile = args[0];
            string rFile = args[1];

            List<CrimeData> crimeData = new List<CrimeData>();

            StreamReader sourceFile = null;

            try
            {
                sourceFile = new StreamReader(sFile);
                sourceFile.ReadLine();

                while(!sourceFile.EndOfStream)
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

                    crimeData.Add(new CrimeData() { CrimeYear = year, CrimePop = population, CrimeVC = violentCrime, CrimeMurder = murder, CrimeRape = rape, CrimeRobbery = robbery, CrimeAggA = aggravatedAssault, CrimePC = propertyCrime, CrimeBurg = burglary, CrimeTheft = theft, CrimeMVTheft = mvTheft  });
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

            /*CreateReport(crimeData, rFile);

//        }

//        public static void CreateReport<CrimeStats>(List<CrimeStats> data, string rFile)
        {*/
            StreamWriter report = null;

            try
            {
                report = new StreamWriter(rFile);
                report.WriteLine("Crime Analyzer Report\n");

                var yearData = from x in crimeData select x.CrimeYear;
                int numYear = yearData.Count();
                int begYear = yearData.Max();
                int endYear = yearData.Min();
                report.WriteLine("Period: {0}-{1} ({2} years)", begYear, endYear, numYear);

                var murderData = from x in crimeData where x.CrimeMurder < 15000 select x.CrimeYear;
                report.Write("Years where murders per year < 15000: ");
                string data = " ";
                foreach(var year in murderData)
                {
                    data += string.Format("{0}, ", year);
                }
                report.Write(data.Substring(0, data.Length - 2));

                var robberyData = from x in crimeData where x.CrimeRobbery > 500000 select new { x.CrimeYear , x.CrimeRobbery };
                report.Write("\nRobberies per year > 500000:");
                data = " ";
                foreach(var year in robberyData)
                {
                    data += string.Format("{0} = {1}, ", year.CrimeYear, year.CrimeRobbery);
                }
                report.Write(data.Substring(0, data.Length - 2));

                var violentCrimeData = from x in crimeData where x.CrimeYear == 2010 select new { x.CrimeVC, x.CrimePop };
                double z = 0, y = 0;
                foreach(var vcCrime in violentCrimeData)
                {
                    y = vcCrime.CrimeVC;
                }
                foreach(var popCrime in violentCrimeData)
                {
                    z = popCrime.CrimePop;
                }
                double vcPerCap = y / z;
                report.WriteLine("\nViolent crime per capita rate (2010): {0}", vcPerCap);

                var avgMurdersData = from x in crimeData select x.CrimeMurder;
                report.WriteLine("Average murder per year (all years): {0}", avgMurdersData.Average());

                avgMurdersData = from x in crimeData where x.CrimeYear >= 1994 && x.CrimeYear <= 1997 select x.CrimeMurder;
                report.WriteLine("Average murder per year (1994-1997): {0}", avgMurdersData.Average());

                avgMurdersData = from x in crimeData where x.CrimeYear >= 2010 && x.CrimeYear <= 2014 select x.CrimeMurder;
                report.WriteLine("Average murder per year (2010-2014): {0}", avgMurdersData.Average());

                var theftData = from x in crimeData where x.CrimeYear >= 1999 && x.CrimeYear <= 2004 select x.CrimeTheft;
                report.WriteLine("Minimum thefts per year (1999-2004): {0}", theftData.Min());
                report.WriteLine("Maximum thefts per year (1999-2004): {0}", theftData.Max());



                Console.WriteLine("{0} has been created", rFile);
            }

            catch (Exception e)
            {
                Console.WriteLine("Unable to create report: {0}", e.Message);
            }

            finally
            {
                    report.Close();
            }
        }
    }
}
