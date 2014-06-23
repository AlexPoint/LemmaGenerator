using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFileBuilder.Classes
{
    public class EnricherFileReader : StreamReader
    {
        public EnricherFileReader(Stream stream) : base(stream) { }
        public EnricherFileReader(string filePath) : base(filePath) { }


        public Tuple<string, string, int> ReadLemmaEntry()
        {
            var line = this.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                // don't read comment lines
                if (!line.StartsWith("//") && !line.StartsWith("#"))
                {
                    var parts = line.Split(' ');
                    var weight = parts.Length > 2 ? int.Parse(parts[2]) : 1;
                    return new Tuple<string, string, int>(parts[0], parts[1], weight); 
                }
            }
            return null;
        }

        public List<Tuple<string, string, int>> ReadAllLemmaEntries()
        {
            var results = new List<Tuple<string, string, int>>();
            while (!this.EndOfStream)
            {
                var lemmaEntry = this.ReadLemmaEntry();
                if (lemmaEntry != null)
                {
                    results.Add(lemmaEntry); 
                }
            }
            return results;
        }
    }
}
