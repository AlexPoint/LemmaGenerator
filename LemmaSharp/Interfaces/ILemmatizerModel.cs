using System;
namespace LemmaSharp {
    public interface ILemmatizerModel {
        string Lemmatize(string sWord);
        string ToString();
    }
}
