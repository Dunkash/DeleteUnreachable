using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HelloWorld
{

    public class Remover
    {
        public class Pair<T1, T2>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }
        }

        public Remover(string filename)
        {
            List<string> file = new List<string>(File.ReadAllLines(filename));
            file.RemoveAll(Comment);
            if (file.Count < 2)
                Console.WriteLine("Incorrect input");

            input = new Dictionary<string, string[]>();
            terminal = file[1].Split(',');
            nonterminal = new Dictionary<string, bool>();
            foreach (var i in file[0].Split(','))
                nonterminal[i] = false;
            st = file[0].Split(',')[0];
            for (int i = 2; i < file.Count; i++)
            {
                var t1 = file[i].Split(new string[] { "=>" }, StringSplitOptions.None);
                var t2 = t1[1].Split('|');
                Add(t1[0], t2);
            }
        }

        public Remover(string[] t,string[] n)
        {



        }
        string st;
        Dictionary<string, string[]> input;
        string[] terminal;
        Dictionary<string,bool> nonterminal;
        public void Add(string from, string[] to)
        {
            input[from] = to;
        }

        public void Start()
        {
            { 
                Console.WriteLine("Grammatic input:");
                foreach (var t in nonterminal.Keys)
                    Console.Write(t + ',');
                Console.WriteLine();
                foreach (var t in terminal)
                    Console.Write(t + ',');
                Console.WriteLine();
                foreach (var t in input)
                {
                    Console.Write(t.Key + "=>");
                    foreach (var k in t.Value)
                        Console.Write(k + '|');
                    Console.WriteLine();
                }
            }
            Evaluate(st);
            { 
                Console.WriteLine();
                Console.WriteLine("Result:");
                foreach (var t in nonterminal)
                    if (t.Value)
                    Console.Write(t.Key + ',');
                Console.WriteLine();
                foreach (var t in terminal)
                    Console.Write(t + ',');
                Console.WriteLine();
                foreach (var t in input)
                {
                    if (nonterminal[t.Key])
                    { 
                    Console.Write(t.Key + "=>");
                    foreach (var k in t.Value)
                        Console.Write(k + '|');
                    Console.WriteLine();
                    }
                }
            }
        }

        private void Evaluate(string value)
        {
            if (!nonterminal.ContainsKey(value))
                throw new Exception("Invalid nonterminals list");
            nonterminal[value] = true;
            if (!input.ContainsKey(value))
                return;


            foreach (var line in input[value])
            {
                foreach (var nont in nonterminal.Keys.ToArray())
                    if (!nonterminal[nont] && line.Contains(nont))
                        Evaluate(nont);
            }
        }

        private static bool Comment(string s)
        {
            return s.ToLower().StartsWith("//");
        }
    }

    class Program
    {
 

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Print name of file, required. Print 'exit' to stop the application.");
                Console.WriteLine();
            Here: var fname = Console.ReadLine();
                if (fname == "exit")
                    Environment.Exit(0);
                if (!File.Exists(fname))
                {
                    Console.WriteLine("File doesn't exist");
                    goto Here;
                }
                var remove = new Remover(fname);
                remove.Start();
            }
        }
    }


}