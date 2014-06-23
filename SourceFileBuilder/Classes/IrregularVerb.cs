using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFileBuilder.Classes
{
    class IrregularVerb
    {
        public string BaseForm { get; set; }
        public string ThirdPersonSingular { get; set; }
        public string PresentParticiple { get; set; }
        public List<string> Preterit { get; set; }
        public List<string> PastParticiple { get; set; }
    }
}
