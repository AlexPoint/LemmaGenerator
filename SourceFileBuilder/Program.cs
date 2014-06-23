using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LemmaSharp.Classes;
using SourceFileBuilder.Classes;

namespace SourceFileBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory + "/../../";
            var lemmatizerFilePath = currentDirectory + "../Test/Data/full7z-mlteast-en.lem";

            var fileName = Path.GetFileNameWithoutExtension(lemmatizerFilePath) + "-modified";
            var extension = Path.GetExtension(lemmatizerFilePath);
            var outputFilePath = string.Format("{0}Output/{1}{2}", currentDirectory, fileName, extension);

            var enricherFilePath = currentDirectory + "Input/english-irregular_verbs-enricher.txt";

            EnrichFile(lemmatizerFilePath, outputFilePath, enricherFilePath);

            /*using (var stream = File.OpenRead(lemmatizerFilePath))
            {
                var lemmatizer = new Lemmatizer(stream);
                var word = "reading";
                lemmatizer.AddExample("laid", "lay", 10000);
                var computedLemma = lemmatizer.Lemmatize(word);

                Console.WriteLine("{0} -> {1}", word, computedLemma);
            }*/

            /*var word = "wrung";
            var lemma = "wring";
            using(var stream = File.OpenRead(lemmatizerFilePath))
            {
                var lemmatizer = new Lemmatizer(stream);
                var lemma1 = lemmatizer.Lemmatize(word);
                Console.WriteLine("Before adding example:");
                Console.WriteLine("{0} --> {1}", word, lemma1);

                var i = 1;
                while (i < 10000)
                {
                    lemmatizer.AddExample(word, lemma);
                    var lemmaX = lemmatizer.Lemmatize(word);
                    Console.WriteLine("After adding example (i={0}):", i);
                    Console.WriteLine("{0} --> {1}", word, lemmaX);

                    i = i*10;
                }
            }*/


            /*var irregularVerbForms = IrregularVerbs.GetIrregularVerbsFormsAndAssociatedLemma();
            EnrichFile(lemmatizerFilePath, outputFilePath, irregularVerbForms);

            Console.WriteLine("Output written at '{0}'", outputFilePath);

            Console.WriteLine("Try to read newly created file");
            using(var testStream = File.OpenRead(outputFilePath))
            {
                var testLemmatizer = new Lemmatizer(testStream);
            }*/

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private static void EnrichFile(string lemmatizerFilePath, string outputFilePath, string enricherFilePath)
        {
            var fileReader = new EnricherFileReader(enricherFilePath);
            var allNewLemmas = fileReader.ReadAllLemmaEntries();

            EnrichFile(lemmatizerFilePath, outputFilePath, allNewLemmas);
        }

        private static void EnrichFile(string lemmatizerFilePath, string outputFilePath,
            IEnumerable<Tuple<string, string, int>> wordsAndLemmaToAdd)
        {
            var stream = File.OpenRead(lemmatizerFilePath);
            var lemmatizer = new Lemmatizer(stream);

            // add new words and lemma
            foreach (var wordAndLemma in wordsAndLemmaToAdd)
            {
                AddLemmaExample(lemmatizer, wordAndLemma.Item1, wordAndLemma.Item2);
            }

            // write output file
            Console.WriteLine("Writing output file...");
            using (var oStream = File.Create(outputFilePath))
            {
                lemmatizer.Serialize(oStream, true, Lemmatizer.Compression.Lzma, true);
            }
            Console.WriteLine("Outuput file written at {0}", outputFilePath);
        }

        private static void AddLemmaExample(Lemmatizer lemmatizer, string word, string lemma)
        {
            var computedLemma = lemmatizer.Lemmatize(word);

            if(computedLemma != lemma)
            {
                // add example
                lemmatizer.AddExample(word, lemma);
                // if still doesn't work --> add exception
                var computedLemma2 = lemmatizer.Lemmatize(word);
                if (computedLemma2 != word)
                {
                    Console.WriteLine("Added lemma exception: {0} -> {1}", word, lemma);
                    lemmatizer.AddException(word, lemma);
                }
            }
        }
    }
}

