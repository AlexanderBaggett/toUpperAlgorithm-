using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Test_func
{
    class Program
    {
        static char[] translationTable = new char[128];
        static void Main(string[] args)
        {

            const string chars = "fz";
            CreateCharacterMapping(translationTable);
            string hello = new string(Enumerable.Repeat(chars, 8000).SelectMany(s => s).ToArray());

            Console.WriteLine("Lean and Mean To Upper");
            Console.WriteLine(leanAndMean(hello));
     

            Console.WriteLine("Parallel lean and mean");
            Console.WriteLine(leanAndMean(hello));
            Console.ReadKey();

            //Console.WriteLine("Parallel to Upper Ordered");
            //Console.WriteLine(parallelUpperOrdered(hello));
            //Console.ReadKey();

            //Console.WriteLine();
            //Console.WriteLine("Parallel to Upper Unordered");
            //Console.WriteLine(parallelUpper(hello));
            //Console.ReadKey();

            //Console.WriteLine();
            //Console.WriteLine("Sequenctial built-in toUpper");
            //Console.WriteLine(sequentialToUpper(hello));
            //Console.ReadKey();
        }


        /// <summary>
        /// A flawed concept as order is not guarunteed to be preserved.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string parallelUpper (string s)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            ConcurrentQueue<char> newstring = new ConcurrentQueue<char>();
            char[] oldstring = s.ToCharArray();
                oldstring
                .AsParallel()
                .ForAll(character => newstring.Enqueue(Convert.ToChar(character.ToString().ToUpper())));


                timer.Stop();
                Console.WriteLine(timer.ElapsedMilliseconds);
            return new string(newstring.ToArray());

        }



        /// <summary>
        /// slowest
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string parallelUpperOrdered(string s)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            char [] newstring = new char [s.Length];
            var oldstring = s.ToCharArray();
            ConcurrentDictionary<int, char> indexedList = new ConcurrentDictionary<int, char>();

            for (int i = 0; i < s.Length; i++ )
            {
                indexedList.TryAdd(i,oldstring[i]);
            }

            indexedList
            .AsParallel()
            .ForAll(kvp => newstring[kvp.Key] = Convert.ToChar(kvp.Value.ToString().ToUpper()));

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);

            return new string(newstring);

        }
        static string sequentialToUpper(string s)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            var newstring = s.ToUpper();
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
            return newstring;
        }


        static string leanAndMean(string s)
        {
            //https://www.dotnetperls.com/ascii-table

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            var oldstring = s.ToArray();
            for (int i = 0; i < oldstring.Length; i++)
            {
                oldstring[i] = translationTable[oldstring[i]];
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
            return new string(oldstring);

        }

        static string Parallel_Lean_And_Mean(string s)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            char[] oldString = s.ToCharArray();
            Parallel.ForEach(oldString, (character) =>
            {
                character = translationTable[character]; 
            });
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
            return new string(oldString);
         
        }

        private static void CreateCharacterMapping(char[] translationTable)
        {
            for (int i = 0; i < 26; i++)
            {
                // 'A' starts at 65  'a' starts at 97
                //build the translation table for A-Z
                translationTable[Convert.ToChar(97 + i)] = Convert.ToChar(65 + i);

            }
            //create the mapping for everything before A-z
            for (int i = 0; i < 65; i++)
            {
                //0 starts at 48 and 9 ends at 57
                translationTable[Convert.ToChar(i)] = Convert.ToChar(i);
            }
            //create the mapping for everything after A-Z
            for (int i = 123; i < 128; i++)
            {
                //0 starts at 48 and 9 ends at 57
                translationTable[Convert.ToChar(i)] = Convert.ToChar(i);
            }
            //populate the mapping for things already capitalized
            for (int i = 0; i < 26; i++)
            {
                // 'A' starts at 65  'a' starts at 97
                //build the translation table for A-Z
                translationTable[Convert.ToChar(65 + i)] = Convert.ToChar(65 + i);

            }


        }





        static void DoSomething(string i)
        {
            
        }

    }
}
