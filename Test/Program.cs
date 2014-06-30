using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LemmaSharp.Classes;
using Test.Classes;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            // Create readable file
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Custom", "full7z-mlteast-en-modified.lem");

            using (var fstream = File.OpenRead(dataFilePath))
            {
                var lemmatizer = new Lemmatizer(fstream);

                // add examples
                var examples = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("got", "get"),
                    new Tuple<string, string>("attached", "attach"),
                    new Tuple<string, string>("went", "go"),
                };
                foreach (var example in examples)
                {
                    var lemma = lemmatizer.Lemmatize(example.Item1);
                    Console.WriteLine("{0} --> {1}", example.Item1, lemma);
                }
            }

            

            Console.WriteLine("OK");
            Console.ReadLine();
        }

        private static Lemmatizer CreatePreBuiltLemmatizer()
        {
            var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);
            return lemmatizer;
        }

        private static Lemmatizer CreateLemmatizerFromFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Custom", "english.lem");
            using (var stream = File.OpenRead(dataFilePath))
            {
                var lemmatizer = new Lemmatizer(stream);
                return lemmatizer;
            }
        }
    }
}
