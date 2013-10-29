using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace LemmaSharp {
    public interface ITrainableLemmatizer:ILemmatizer {
        ExampleList Examples {
            get;
        }
        ILemmatizerModel Model {
            get;
        }

        void AddExample(string sWord, string sLemma);
        void AddExample(string sWord, string sLemma, double dWeight);
        void AddExample(string sWord, string sLemma, double dWeight, string sMsd);

        void BuildModel();
    }
}
