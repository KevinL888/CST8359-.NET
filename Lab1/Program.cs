/*******************************************************************************************************************************************
Filename:			Program.cs
Version:			1.0
Author:				Kevin Lai
Student No:			040 812 704
Course Name/Number:	.NET Enterprise Appl. Dev. CST8359
Lab Sect:			300
Lab #:              1		
Assignment name:	Lab1 - Hello World! An introduction to C#
Due Date:			January 24 2019
Submission Date:    January 21 2019
Professor:			Justin Antoszek
Purpose:			This lab is designed to give you a basic understanding of a C# application and provide an opportunity to use many
                    simple C# techniques. 
********************************************************************************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> words = new List<string>();
            bool running = true;
            string choice;
            while (running) {
                Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write("Hello World!!! My First C# App\nOptions\n----------\n1 - Import Words From File\n2 - Bubble Sort words\n3 - LINQ/Lambda sort words\n4 - Count the Distinct Words\n5 - Take the first 10 words\n6 - Get the number of words that start with 'j' and display the count\n7 - Get and display of words that end with 'd' and display the count\n8 - Get and display of words that are greater than 4 characters long, and display the count\n9 - Get and display of words that are less than 3 characters long and start with the letter 'a', and display the count\nx - Exit\n\nMake a selection: ");
                choice = Console.ReadLine();
                Console.WriteLine("\n");
                Console.Clear();

                    switch (choice)
                    {
                        case "1": importWords(words); break;
                        case "2": bubbleSort(words); break;
                        case "3": lambdaSort(words); break;
                        case "4": countDistinctWords(words); break;
                        case "5": firstTenWords(words); break;
                        case "6": wordsStartWithJ(words); break;
                        case "7": wordsEndWithD(words); break;
                        case "8": wordsGreaterThanFour(words); break;
                        case "9": wordsLessThanThree(words); break;
                        case "x": running = false; break;
                        default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input\n\n"); break;
                }// end switch
            }//end while loop
        }// end main

       public static void importWords(List<string> words)
        {         
            try
            {
                using (StreamReader sr = new StreamReader("Words.txt"))
                {
                    string line;
                    Console.WriteLine("Reading Words");
                    while((line = sr.ReadLine()) != null) {
                        words.Add(line);
                    }
                    Console.WriteLine("Reading Words complete");
                    Console.WriteLine("Number of words found: " + words.Count()+ "\n\n");
                }
            }catch(Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }

        public static string[] bubbleSort(List<string> words)
        {
            string[] temp = words.ToArray();
            bool wasSwapped = true;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
             
            for (int i = 0; i < words.Count() - 1 && wasSwapped; i++)
            {
                wasSwapped = false;
                for (int j = 0; j < words.Count() - i - 1; j++)
                {
                    if (string.Compare(temp[j],temp[j+1]) >= 0)
                    {
                        string tempString = temp[j];
                        temp[j] = temp[j + 1];
                        temp[j + 1] = tempString;
                        wasSwapped = true;
                    }
                }
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("Time elapsed: " + ts.Milliseconds +"ms\n\n");
            return temp;
        }

        public static void lambdaSort(List<string> words)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            var _Results = from elements in words
                           orderby elements.ToString()
                           select elements;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("Time elapsed: " + ts.Milliseconds+ "ms\n\n");
        }

       public static void countDistinctWords(List<string> words)
        {

           // var _Results = words.Distinct().Count();

            var _Results = (from element in words
                           select element).Distinct().Count();

            Console.WriteLine("The number of distinct words is: "+ _Results + "\n\n");
        }

       public static void firstTenWords(List<string> words)
        {
            // var _Result = words.Take(10);

            var _Results = (from element in words
                            select element).Take(10);

            foreach (string value in _Results)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine("\n");
        }

       public static void wordsStartWithJ(List<string> words)
        {
            var _Results = (from elements in words
                           where elements.StartsWith("j")
                           select elements);

            foreach (string value in _Results)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("Number of words that start with 'j': "+ _Results.Count()+"\n\n");
        }

       public static void wordsEndWithD(List<string> words)
        {
            var _Results = (from elements in words
                            where elements.EndsWith("d")
                            select elements);

            foreach (string value in _Results)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("Number of words that end with 'd': "+_Results.Count() + "\n\n");
        }

       public static void wordsGreaterThanFour(List<string> words)
        {
            var _Results = (from elements in words
                           where elements.Length > 4
                           select elements);

            foreach (string value in _Results)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("Number of words longer than 4 characters: "+_Results.Count() +"\n\n");
        }

       public static void wordsLessThanThree(List<string> words)
        {
            var _Results = (from elements in words
                            where elements.Length < 3 && elements.StartsWith("a")
                            select elements);

            foreach (string value in _Results)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("Number of words less than 3 characters and start with 'a': "+_Results.Count() + "\n\n");
        }
    }
}
