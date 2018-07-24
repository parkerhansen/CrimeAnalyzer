using System;
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
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments. Please enter a source file and report file.");
                return;
            }
            else if (args.Length > 2)
            {
                Console.WriteLine("Too many arguments. Please only enter a source file and report file.");
                return;
            }

            string sFile = args[0];
            string rFile = args[1];

            int count = 1;

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

                    if (column.Length < 11)
                    {
                        Console.WriteLine("Error: Row {0} of data contains {1} values. Each row should have 11 data elements. Fix and try again, report was not created.", count, column.Length);
                        return;
                    }

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

                    crimeData.Add(new CrimeData() { CrimeYear = year, CrimePop = population, CrimeVC = violentCrime, CrimeMurder = murder, CrimeRape = rape, CrimeRobbery = robbery, CrimeAggA = aggravatedAssault, CrimePC = propertyCrime, CrimeBurg = burglary, CrimeTheft = theft, CrimeMVTheft = mvTheft  });

                    count++;
                }
            }

            catch (FormatException)
            {
                Console.WriteLine("Error: Source file contains data that is not of the right type on row {0} of data. Make sure all data is numerical. Report was not created.", count);
                return;
            }

            catch (Exception)
            {
                Console.WriteLine("Error: Source file can't be opened. Report was not created.");
                return;
            }

            finally
            {
                if (sourceFile != null)
                {
                    sourceFile.Close();
                }
            }

            StreamWriter report = null;

            try
            {
                report = new StreamWriter(rFile);
                report.WriteLine("Crime Analyzer Report\n");

                var yearData = from x in crimeData select x.CrimeYear;
                int numYear = yearData.Count();
                int begYear = yearData.Min();
                int endYear = yearData.Max();
                report.WriteLine("Period: {0}-{1} ({2} years)", begYear, endYear, numYear);

                var murderData = from x in crimeData where x.CrimeMurder < 15000 select x.CrimeYear;
                report.Write("\nYears where murders per year < 15000: ");
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

                var violentCrimeData = from x in crimeData where x.CrimeYear == 2010 select new { x.CrimeVC , x.CrimePop };
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

                avgMurdersData = from x in crimeData where x.CrimeYear >= 2010 && x.CrimeYear <= 2013 select x.CrimeMurder;
                report.WriteLine("Average murder per year (2010-2013): {0}", avgMurdersData.Average());

                var theftData = from x in crimeData where x.CrimeYear >= 1999 && x.CrimeYear <= 2004 select x.CrimeTheft;
                report.WriteLine("Minimum thefts per year (1999-2004): {0}", theftData.Min());
                report.WriteLine("Maximum thefts per year (1999-2004): {0}", theftData.Max());

                var mvTheftData = (from x in crimeData orderby x.CrimeMVTheft descending select new { x.CrimeYear , x.CrimeMVTheft }).FirstOrDefault();
                report.WriteLine("Year of highest number of motor vehicle thefts: {0}", mvTheftData.CrimeYear);



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
