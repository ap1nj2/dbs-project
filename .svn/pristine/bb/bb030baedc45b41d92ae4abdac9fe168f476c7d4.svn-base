using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace FastReadTextFile
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime end;
            DateTime start = DateTime.Now;
            Console.WriteLine("### Overall Start Time: " + start.ToLongTimeString());
            Console.WriteLine();
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 5000)), 5);
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 5000)), 10);
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 5000)), 25);
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 5);
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 10);
            TestReadingAndProcessingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 25);
            end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("### Overall End Time: " + end.ToLongTimeString());
            Console.WriteLine("### Overall Run Time: " + (end - start));
            Console.WriteLine();
            Console.WriteLine("Hit Enter to Exit");
            Console.ReadLine();
        }
        //####################################################
        //Does a comparison of reading all the lines in from a file and performing some rudimentary
        //operations on them. Which way is fastest?
        static void TestReadingAndProcessingLinesFromFile(int numberOfLines, int numTimesGuidRepeated)
        {
            Console.WriteLine("######## " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("######## Number of lines in file: " + numberOfLines);
            Console.WriteLine("######## Number of times Guid repeated on each line: " + numTimesGuidRepeated);
            Console.WriteLine("###########################################################");
            Console.WriteLine();
            string g = String.Join(" ", Enumerable.Repeat(new Guid().ToString(), numTimesGuidRepeated));
            string[] AllLines = null;
            string fileName = "Performance_Test_File.txt";
            int MAX = numberOfLines;
            DateTime end;
            DateTime start = DateTime.Now;
            //Create the file populating it with GUIDs
            Console.WriteLine("Generating file: " + start.ToLongTimeString());
            using (StreamWriter sw = File.CreateText(fileName))
            {
                for (int x = 0; x < MAX; x++)
                {
                    sw.WriteLine(g);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Just read everything into one string
            Console.WriteLine("Reading file reading to end into string: ");
            start = DateTime.Now;
            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = sr.ReadToEnd();
                    TestReadingAndProcessingLinesFromFile_DoStuff(s);
                }
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine("Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine("EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read the entire contents into a StringBuilder object
            Console.WriteLine("Reading file reading to end into stringbuilder: ");
            start = DateTime.Now;
            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(sr.ReadToEnd());
                    TestReadingAndProcessingLinesFromFile_DoStuff(sb.ToString()); //to simulate work
                }
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine("Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine("EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Standard and probably most common way of reading a file. 
            Console.WriteLine("Reading file assigning each line to string: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(s); //to simulate work
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Doing it the most common way, but using a Buffered Reader now.
            Console.WriteLine("Buffered reading file assigning each line to string: ");
            start = DateTime.Now;
            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(s); //to simulate work
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Reading each line using a buffered reader again, but setting the buffer size since we know what it will be.
            Console.WriteLine("Buffered reading with preset buffer size assigning each line to string: ");
            start = DateTime.Now;
            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs, System.Text.ASCIIEncoding.Unicode.GetByteCount(g)))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(s); //to simulate work
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read every line of the file reusing a StringBuilder object to save on string memory allocation times
            Console.WriteLine("Reading file assigning each line to StringBuilder: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                StringBuilder sb = new StringBuilder();
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(sb.ToString()); //to simulate work
                    sb.Clear();
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Reading each line into a StringBuilder, but setting the StringBuilder object to an initial
            //size since we know how long the longest line in the file is.
            Console.WriteLine("Reading file assigning each line to preset size StringBuilder: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                StringBuilder sb = new StringBuilder(g.Length);
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(sb.ToString()); //to simulate work
                    sb.Clear();
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read each line into an array index. 
            Console.WriteLine("Reading each line into string array. Process with Parallel.For: ");
            start = DateTime.Now;
            try
            {
                AllLines = new string[MAX];    //only allocate memory here
                using (StreamReader sr = File.OpenText(fileName))
                {
                    int x = 0;
                    while (!sr.EndOfStream)
                    {
                        //we're just testing read speeds
                        AllLines[x] = sr.ReadLine();
                        x += 1;
                    }
                } //CLOSE THE FILE because we are now DONE with it.
                Parallel.For(0, AllLines.Length, x =>
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(AllLines[x]); //to simulate work
                });
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine("Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine("EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            finally
            {
                if (AllLines != null)
                {
                    Array.Clear(AllLines, 0, AllLines.Length);
                    AllLines = null;
                }
            }
            GC.Collect();
            Thread.Sleep(1000);
            //Read the entire file using File.ReadAllLines. 
            Console.WriteLine("Performing File ReadAllLines into array. Process with Parallel.For: ");
            start = DateTime.Now;
            try
            {
                AllLines = new string[MAX];    //only allocate memory here
                AllLines = File.ReadAllLines(fileName);
                Parallel.For(0, AllLines.Length, x =>
                {
                    TestReadingAndProcessingLinesFromFile_DoStuff(AllLines[x]); //to simulate work
                });
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine("Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine("EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            finally
            {
                if (AllLines != null)
                {
                    Array.Clear(AllLines, 0, AllLines.Length);
                    AllLines = null;
                }
            }
            File.Delete(fileName);
            fileName = null;
            GC.Collect();
        }
        //Just simulates doing work on a line read from an input file
        static void TestReadingAndProcessingLinesFromFile_DoStuff(string s)
        {
            string[] sa = s.Split(new char[' ']);
            int[] ia = new int[sa.Length];
            int num = 0;
            for (int x = 0; x < sa.Length; x++)
            {
                foreach (char c in sa[x])
                {
                    if (int.TryParse(c.ToString(), out num))
                    {   //just doing some bogus mathematical calculations to simulate work
                        ia[x] = (int)((Math.Sqrt(Math.Log(num) % Math.Log10(num))) * (Math.Log(Math.Log10(num) / Math.Sqrt(num))));
                    }
                }
            }
            //clean up
            Array.Clear(ia, 0, ia.Length);
            Array.Clear(sa, 0, sa.Length);
            ia = null;
            sa = null;
        }
    } //class
}
