using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LemmaSharp.Classes;

namespace SourceFileBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory + "/../../";
            var lemmatizerFilePath = currentDirectory + "../Test/Data/full7z-mlteast-en.lem";

            var outputFilePath = currentDirectory + "Output/english.lem";

            var irregularVerbForms = IrregularVerbs.GetIrregularVerbsFormsAndAssociatedLemma();
            EnrichFile(lemmatizerFilePath, outputFilePath, irregularVerbForms);

            Console.WriteLine("Output written at '{0}'", outputFilePath);

            Console.WriteLine("Try to read newly created file");
            using(var testStream = File.OpenRead(outputFilePath))
            {
                var testLemmatizer = new Lemmatizer(testStream);
            }
            Console.WriteLine("OK");

            Console.ReadKey();
        }

        static void EnrichFile(string lemmatizerFilePath, string outputFilePath,
            IEnumerable<Tuple<string, string>> wordsAndLemmaToAdd)
        {
            var stream = File.OpenRead(lemmatizerFilePath);
            var lemmatizer = new Lemmatizer(stream);

            // add new words and lemma
            foreach (var wordAndLemma in wordsAndLemmaToAdd)
            {
                lemmatizer.AddExample(wordAndLemma.Item1, wordAndLemma.Item2);
            }

            // write output file
            using (var oStream = File.Create(outputFilePath))
            {
                lemmatizer.Serialize(oStream, true, Lemmatizer.Compression.Lzma, true);
            }
        }
    }
}
