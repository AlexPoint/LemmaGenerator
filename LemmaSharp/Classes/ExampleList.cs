using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace LemmaSharp {
    [Serializable()]
    public class ExampleList : ISerializable {
        #region Private Variables

        private LemmatizerSettings lsett;
        private RuleList rlRules;
        private Dictionary<string, LemmaExample> dictExamples;
        private List<LemmaExample> lstExamples;

        #endregion

        #region Constructor(s) & Destructor(s)

        public ExampleList(LemmatizerSettings lsett): base() {
            this.lsett = lsett;

            this.dictExamples = new Dictionary<string, LemmaExample>();
            this.lstExamples = null;
            this.rlRules = new RuleList(lsett);
        }
        public ExampleList(StreamReader srIn, string sFormat, LemmatizerSettings lsett): this(lsett) {
            AddMultextFile(srIn, sFormat);
        }
        
        #endregion
        
        #region Public Properties & Indexers

        public LemmaExample this[int i] {
            get {
                if (lstExamples == null) FinalizeAdditions();
                return lstExamples[i];
            }
        }
        public int Count {
            get {
                if (lstExamples == null) FinalizeAdditions();
                return lstExamples.Count;
            }
        }
        public double WeightSum {
            get {
                if (lstExamples == null) FinalizeAdditions();

                double dWeight = 0;

                foreach (LemmaExample exm in lstExamples)
                    dWeight += exm.Weight;

                return dWeight;
            }
        }
        public RuleList Rules {
            get {
                return rlRules;
            }
        }
        public List<LemmaExample> ListExamples {
            get {
                if (lstExamples == null) FinalizeAdditions();
                return lstExamples;
            }
        }

        #endregion

        #region Essential Class Functions (adding/removing examples)

        public void AddMultextFile(StreamReader srIn, string sFormat) {
            //read from file
            string sLine = null;
            int iError = 0;
            int iLine = 0;

            int iW = sFormat.IndexOf('W');
            int iL = sFormat.IndexOf('L');
            int iM = sFormat.IndexOf('M');
            int iF = sFormat.IndexOf('F');
            int iLen = Math.Max(Math.Max(iW, iL), Math.Max(iM, iF))+1;

            if (iW < 0 || iL < 0) {
                Console.WriteLine("  Can not find word and lemma location in the format specification");
                return;
            }

            while ((sLine = srIn.ReadLine()) != null && iError < 50) {
                iLine++;

                string[] asWords = sLine.Split(new char[] { '\t' });
                if (asWords.Length < iLen) {
                    Console.WriteLine("  ERROR: Line doesn't confirm to the given format \"" + sFormat + "\"! Line " + iLine.ToString() + ".");
                    iError++;
                    continue;
                }

                string sWord = asWords[iW];
                string sLemma = asWords[iL];
                if (sLemma == "=") sLemma = sWord;
                string sMsd = null;
                if (iM > -1) sMsd = asWords[iM];
                double dWeight = 1; ;
                if (iF > -1)
                    Double.TryParse(asWords[iM], out dWeight);

                AddExample(sWord, sLemma, dWeight, sMsd);
            }
            if (iError == 50) Console.WriteLine("  Parsing stopped because of too many (50) errors. Check format specification");
        }
        public LemmaExample AddExample(string sWord, string sLemma, double dWeight, string sMsd) {
            string sNewMsd = lsett.eMsdConsider != LemmatizerSettings.MsdConsideration.Ignore ? sMsd : null;
            LemmaExample leNew = new LemmaExample(sWord, sLemma, dWeight, sNewMsd, rlRules, lsett);
            return Add(leNew);
        }

        private LemmaExample Add(LemmaExample leNew) {
            LemmaExample leReturn = null;

            if (!dictExamples.TryGetValue(leNew.Signature, out leReturn)) {
                leReturn = leNew;
                dictExamples.Add(leReturn.Signature, leReturn);
            }
            else
                leReturn.Join(leNew);


            lstExamples = null;

            return leReturn;
        }
        public void DropExamples() {
            dictExamples.Clear();
            lstExamples = null;
        }
        public void FinalizeAdditions() {
            if (lstExamples != null) return;
            lstExamples = new List<LemmaExample>(dictExamples.Values);
            lstExamples.Sort();
        }

        public ExampleList GetFrontRearExampleList(bool front) {
            ExampleList elExamplesNew = new ExampleList(lsett);

            foreach (LemmaExample le in this.ListExamples) {
                if (front)
                    elExamplesNew.AddExample(le.WordFront, le.LemmaFront, le.Weight, le.Msd);
                else
                    elExamplesNew.AddExample(le.WordRear, le.LemmaRear, le.Weight, le.Msd);
            }
            elExamplesNew.FinalizeAdditions();

            return elExamplesNew;
        }

        #endregion

        #region Output Functions (ToString)

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            foreach (LemmaExample exm in lstExamples) {
                sb.AppendLine(exm.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region Serialization Functions (.Net Default - ISerializable)

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            
            info.AddValue("lsett", lsett);
            info.AddValue("iNumExamples", dictExamples.Count);

            string[] aWords = new string[dictExamples.Count];
            string[] aLemmas = new string[dictExamples.Count];
            double[] aWeights = new double[dictExamples.Count];
            string[] aMsds = new string[dictExamples.Count];
            int iExm = 0;
            foreach (LemmaExample exm in dictExamples.Values) {
                aWords[iExm] = exm.Word;
                aLemmas[iExm] = exm.Lemma;
                aWeights[iExm] = exm.Weight;
                aMsds[iExm] = exm.Msd;
                iExm++;
            }

            info.AddValue("aWords", aWords);
            info.AddValue("aLemmas", aLemmas);
            info.AddValue("aWeights", aWeights);
            info.AddValue("aMsds", aMsds);
        }
        public ExampleList(SerializationInfo info, StreamingContext context) {

            lsett = (LemmatizerSettings)info.GetValue("lsett", typeof(LemmatizerSettings));

            this.dictExamples = new Dictionary<string, LemmaExample>();
            this.lstExamples = null;
            this.rlRules = new RuleList(lsett);

            string[] aWords = (string[])info.GetValue("aWords", typeof(string[]));
            string[] aLemmas = (string[])info.GetValue("aLemmas", typeof(string[]));
            double[] aWeights = (double[])info.GetValue("aWeights", typeof(double[]));
            string[] aMsds = (string[])info.GetValue("aMsds", typeof(string[]));

            for (int iExm = 0; iExm < aWords.Length; iExm++)
                AddExample(aWords[iExm], aLemmas[iExm], aWeights[iExm], aMsds[iExm]);
        }

        #endregion
        #region Serialization Functions (Binary)

        public void Serialize(BinaryWriter binWrt, bool bSerializeExamples, bool bThisTopObject) {
            //save metadata
            binWrt.Write(bThisTopObject);
            
            //save refernce types if needed -------------------------
            if (bThisTopObject)
                lsett.Serialize(binWrt);

            rlRules.Serialize(binWrt, false);

            if (!bSerializeExamples) {
                binWrt.Write(false); // lstExamples == null
                binWrt.Write(0); // dictExamples.Count == 0
            }
            else {
                if (lstExamples == null) {
                    binWrt.Write(false); // lstExamples == null

                    //save dictionary items
                    int iCount = dictExamples.Count;
                    binWrt.Write(iCount);

                    foreach (KeyValuePair<string, LemmaExample> kvp in dictExamples) {
                        binWrt.Write(kvp.Value.Rule.Signature);
                        kvp.Value.Serialize(binWrt, false);
                    }
                }
                else {
                    binWrt.Write(true); // lstExamples != null

                    //save list & dictionary items
                    int iCount = lstExamples.Count;
                    binWrt.Write(iCount);

                    foreach (LemmaExample le in lstExamples) {
                        binWrt.Write(le.Rule.Signature);
                        le.Serialize(binWrt, false);
                    }
                }
            }
        }
        public void Deserialize(BinaryReader binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBoolean();
            
            //load refernce types if needed -------------------------
            if (bThisTopObject)
                this.lsett = new LemmatizerSettings(binRead);
            else
                this.lsett = lsett;

            rlRules = new RuleList(binRead, this.lsett);

            bool bCreateLstExamples = binRead.ReadBoolean();

            lstExamples = bCreateLstExamples ? new List<LemmaExample>() : null;
            dictExamples = new Dictionary<string, LemmaExample>();

            //load dictionary items
            int iCount = binRead.ReadInt32();
            for (int iId = 0; iId < iCount; iId++) {
                LemmaRule lrRule = rlRules[binRead.ReadString()];
                LemmaExample le = new LemmaExample(binRead, this.lsett, lrRule);

                dictExamples.Add(le.Signature, le);
                if (bCreateLstExamples) lstExamples.Add(le);
            }
        }
        public ExampleList(BinaryReader binRead, LemmatizerSettings lsett) {
            Deserialize(binRead, lsett);
        }

        #endregion
        #region Serialization Functions (Latino)

        #if LATINO

        public void Save(Latino.BinarySerializer binWrt, bool bSerializeExamples, bool bThisTopObject) {
            //save metadata
            binWrt.WriteBool(bThisTopObject);

            //save refernce types if needed -------------------------
            if (bThisTopObject)
                lsett.Save(binWrt);

            rlRules.Save(binWrt, false);

            if (!bSerializeExamples) {
                binWrt.WriteBool(false); // lstExamples == null
                binWrt.WriteInt(0); // dictExamples.Count == 0
            }
            else {
                if (lstExamples == null) {
                    binWrt.WriteBool(false); // lstExamples == null

                    //save dictionary items
                    int iCount = dictExamples.Count;
                    binWrt.WriteInt(iCount);

                    foreach (KeyValuePair<string, LemmaExample> kvp in dictExamples) {
                        binWrt.WriteString(kvp.Value.Rule.Signature);
                        kvp.Value.Save(binWrt, false);
                    }
                }
                else {
                    binWrt.WriteBool(true); // lstExamples != null

                    //save list & dictionary items
                    int iCount = lstExamples.Count;
                    binWrt.WriteInt(iCount);

                    foreach (LemmaExample le in lstExamples) {
                        binWrt.WriteString(le.Rule.Signature);
                        le.Save(binWrt, false);
                    }
                }
            }

        }
        public void Load(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBool();

            //load refernce types if needed -------------------------
            if (bThisTopObject)
                this.lsett = new LemmatizerSettings(binRead);
            else
                this.lsett = lsett;

            rlRules = new RuleList(binRead, this.lsett);

            bool bCreateLstExamples = binRead.ReadBool();

            lstExamples = bCreateLstExamples ? new List<LemmaExample>() : null;
            dictExamples = new Dictionary<string, LemmaExample>();

            //load dictionary items
            int iCount = binRead.ReadInt();
            for (int iId = 0; iId < iCount; iId++) {
                LemmaRule lrRule = rlRules[binRead.ReadString()];
                LemmaExample le = new LemmaExample(binRead, this.lsett, lrRule);

                dictExamples.Add(le.Signature, le);
                if (bCreateLstExamples) lstExamples.Add(le);
            }

        }
        public ExampleList(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            Load(binRead, lsett);
        }

        #endif

        #endregion

    }
}
