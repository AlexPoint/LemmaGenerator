using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;


namespace LemmaSharp {
    public class LemmaExample : IComparable<LemmaExample>, IComparer<LemmaExample> {
        #region Private Variables

        private string sWord;
        private string sLemma;
        private string sSignature;
        private string sMsd;
        private double dWeight;
        private LemmaRule lrRule;
        private LemmatizerSettings lsett;
        
        private string sWordRearCache;
        private string sWordFrontCache;
        private string sLemmaFrontCache;

        #endregion

        #region Constructor(s) & Destructor(s)

        public LemmaExample(string sWord, string sLemma, double dWeight, string sMsd, RuleList rlRules, LemmatizerSettings lsett) {
            this.lsett = lsett;

            this.sWord = sWord;
            this.sLemma = sLemma;
            this.sMsd = sMsd;
            this.dWeight = dWeight;
            this.lrRule = rlRules.AddRule(this);

            switch (lsett.eMsdConsider) {
                case LemmatizerSettings.MsdConsideration.Ignore:
                case LemmatizerSettings.MsdConsideration.JoinAll:
                case LemmatizerSettings.MsdConsideration.JoinDistinct:
                case LemmatizerSettings.MsdConsideration.JoinSameSubstring:
                    sSignature = "[" + sWord + "]==>[" + sLemma + "]";
                    break;
                case LemmatizerSettings.MsdConsideration.Distinct:
                default:
                    sSignature = "[" + sWord + "]==>[" + sLemma + "](" + (sMsd != null ? sMsd : "") + ")";
                    break;
            }

            this.sWordRearCache = null;
            this.sWordFrontCache = null;
            this.sLemmaFrontCache = null;

        }

        #endregion

        #region Public Properties

        public string Word {
            get {
                return sWord;
            }
        }
        public string Lemma {
            get {
                return sLemma;
            }
        }
        public string Msd {
            get {
                return sMsd;
            }
        }
        public string Signature {
            get {
                return sSignature;
            }
        }
        public double Weight {
            get {
                return dWeight;
            }
        }
        public LemmaRule Rule {
            get {
                return lrRule;
            }
        }

        /// <summary>
        /// Word to be pre-lemmatized with Front-Lemmatizer into LemmaFront which is then lemmatized by standard Rear-Lemmatizer (Warning it is reversed)
        /// </summary>
        public string WordFront {
            get {
                if (sWordFrontCache == null)
                    sWordFrontCache = StringReverse(sWord);
                return sWordFrontCache;
            }
        }
        /// <summary>
        /// Lemma to be produced by pre-lemmatizing with Front-Lemmatizer (Warning it is reversed)
        /// </summary>
        public string LemmaFront {
            get {
                if (sLemmaFrontCache == null)
                    sLemmaFrontCache = StringReverse(WordRear);
                return sLemmaFrontCache;
            }
        }
        /// <summary>
        /// word to be lemmatized by standard Rear-Lemmatizer (it's beggining has been already modified by Front-Lemmatizer)
        /// </summary>
        public string WordRear {
            get {
                if (sWordRearCache == null) {
                    int lemmaPos = 0, wordPos = 0;
                    string common = LongestCommonSubstring(sWord, sLemma, ref wordPos, ref lemmaPos);
                    sWordRearCache = lemmaPos == -1 ? sLemma : (sLemma.Substring(0, lemmaPos + common.Length) + sWord.Substring(wordPos + common.Length));
                }
                return sWordRearCache;
            }
        }
        /// <summary>
        /// lemma to be produced by standard Rear-Lemmatizer from WordRear
        /// </summary>
        public string LemmaRear {
            get {
                return sLemma;
            }
        }


        #endregion

        #region Essential Class Functions (joining two examples into one)

        //TODO - this function is not totaly ok because sMsd should not be changed since it could be included in signature
        public void Join(LemmaExample leJoin) {
            dWeight += leJoin.dWeight;

            if (sMsd != null)
                switch (lsett.eMsdConsider) {
                    case LemmatizerSettings.MsdConsideration.Ignore:
                        sMsd = null;
                        break;
                    case LemmatizerSettings.MsdConsideration.Distinct:
                        break;
                    case LemmatizerSettings.MsdConsideration.JoinAll:
                        sMsd += "|" + leJoin.sMsd;
                        break;
                    case LemmatizerSettings.MsdConsideration.JoinDistinct:
                        if (!new List<string>(sMsd.Split(new char[] { '|' })).Contains(leJoin.sMsd))
                            sMsd += "|" + leJoin.sMsd;
                        break;
                    case LemmatizerSettings.MsdConsideration.JoinSameSubstring:
                        int iPos = 0;
                        int iMax = Math.Min(sMsd.Length, leJoin.sMsd.Length);
                        while (iPos < iMax && sMsd[iPos] == leJoin.sMsd[iPos]) iPos++;
                        sMsd = sMsd.Substring(0, iPos);
                        break;
                    default:
                        break;
                }

        }

        #endregion
        #region Essential Class Functions (calculating similarities betwen examples)

        public int Similarity(LemmaExample le) {
            return Similarity(this, le);
        }
        public static int Similarity(LemmaExample le1, LemmaExample le2) {
            string sWord1 = le1.sWord;
            string sWord2 = le2.sWord;
            int iLen1 = sWord1.Length;
            int iLen2 = sWord2.Length;
            int iMaxLen = Math.Min(iLen1, iLen2);

            for (int iPos = 1; iPos <= iMaxLen; iPos++)
                if (sWord1[iLen1 - iPos] != sWord2[iLen2 - iPos]) return iPos - 1;

            //TODO similarity should be bigger if two words are totaly equal
            //if (sWord1 == sWord2)
            //    return iMaxLen + 1;
            //else
            return iMaxLen;
        }

        #endregion
        #region Essential Class Functions (comparing examples - eg.: for sorting)
        /// <summary>
        /// Function used to comprare current MultextExample (ME) against argument ME.
        /// Mainly used in for sorting lists of MEs.
        /// </summary>
        /// <param name="other"> MultextExample (ME) that we compare current ME against.</param>
        /// <returns>1 if current ME is bigger, -1 if smaler and 0 if both are the same.</returns>
        public int CompareTo(LemmaExample other) {
            int iComparison;

            iComparison = CompareStrings(this.sWord, other.sWord, false);
            if (iComparison != 0) return iComparison;

            iComparison = CompareStrings(this.sLemma, other.sLemma, true);
            if (iComparison != 0) return iComparison;

            if (lsett.eMsdConsider == LemmatizerSettings.MsdConsideration.Distinct &&
                this.sMsd != null && other.sMsd != null) {
                iComparison = CompareStrings(this.sMsd, other.sMsd, true);
                if (iComparison != 0) return iComparison;
            }

            return 0;
        }
        public int Compare(LemmaExample x, LemmaExample y) {
            return x.CompareTo(y);
        }
        public static int CompareStrings(string sStr1, string sStr2, bool bForward) {
            int iLen1 = sStr1.Length;
            int iLen2 = sStr2.Length;
            int iMaxLen = Math.Min(iLen1, iLen2);

            if (bForward)
                for (int iPos = 0; iPos < iMaxLen; iPos++) {
                    if (sStr1[iPos] > sStr2[iPos]) return 1;
                    if (sStr1[iPos] < sStr2[iPos]) return -1;
                }
            else
                for (int iPos = 1; iPos <= iMaxLen; iPos++) {
                    if (sStr1[iLen1 - iPos] > sStr2[iLen2 - iPos]) return 1;
                    if (sStr1[iLen1 - iPos] < sStr2[iLen2 - iPos]) return -1;
                }

            if (iLen1 > iLen2) return 1;
            if (iLen1 < iLen2) return -1;
            return 0;
        }
        public static int EqualPrifixLen(string sStr1, string sStr2) {
            int iLen1 = sStr1.Length;
            int iLen2 = sStr2.Length;
            int iMaxLen = Math.Min(iLen1, iLen2);

            for (int iPos = 0; iPos < iMaxLen; iPos++)
                if (sStr1[iPos] != sStr2[iPos]) return iPos;

            return iMaxLen;
        }

        public static string LongestCommonSubstring(string sStr1, string sStr2, ref int iPosInStr1, ref int iPosInStr2) {
            int[,] l = new int[sStr1.Length + 1, sStr2.Length + 1];
            int z = 0;
            string ret = "";
            iPosInStr1 = -1;
            iPosInStr2 = -1;

            for (int i = 0; i < sStr1.Length; i++)
                for (int j = 0; j < sStr2.Length; j++)
                    if (sStr1[i] == sStr2[j]) {
                        if (i == 0 || j == 0) l[i, j] = 1;
                        else l[i, j] = l[i - 1, j - 1] + 1;
                        if (l[i, j] > z) {
                            z = l[i, j];
                            iPosInStr1 = i - z + 1;
                            iPosInStr2 = j - z + 1;
                            ret = sStr1.Substring(i - z + 1, z);
                        }
                    }

            return ret;
        }
        public static string StringReverse(string s) {
            if (s == null) return null;
            char[] charArray = s.ToCharArray();
            int len = s.Length - 1;

            for (int i = 0; i < len; i++, len--) {
                charArray[i] ^= charArray[len];
                charArray[len] ^= charArray[i];
                charArray[i] ^= charArray[len];
            }

            return new string(charArray);
        } 

        



        #endregion

        #region Output Functions (ToString)

        public override string ToString() {
            string sThis =
                (sWord == null ? "" : "W:\"" + sWord + "\" ") +
                (sLemma == null ? "" : "L:\"" + sLemma + "\" ") +
                (sMsd == null ? "" : "M:\"" + sMsd + "\" ") +
                (Double.IsNaN(dWeight) ? "" : "F:\"" + dWeight + "\" ") +
                (lrRule == null ? "" : "R:" + lrRule.ToString() + " ");

            return String.IsNullOrEmpty(sThis) ? "" : sThis.Substring(0, sThis.Length - 1);
        }

        #endregion

        #region Serialization Functions (Binary)

        public void Serialize(BinaryWriter binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.Write(bThisTopObject);
            
            //save value types --------------------------------------
            binWrt.Write(sWord);
            binWrt.Write(sLemma);
            binWrt.Write(sSignature);
            if (sMsd == null) 
                binWrt.Write(false);
            else {
                binWrt.Write(true);
                binWrt.Write(sMsd);
            }
            binWrt.Write(dWeight);

            //save refernce types if needed -------------------------
            if (bThisTopObject) {
                lsett.Serialize(binWrt);
                lrRule.Serialize(binWrt, false);
            }

        }
        public void Deserialize(BinaryReader binRead, LemmatizerSettings lsett, LemmaRule lrRule) {
            //load metadata
            bool bThisTopObject = binRead.ReadBoolean();            
            
            //load value types --------------------------------------
            sWord = binRead.ReadString();
            sLemma = binRead.ReadString();
            sSignature = binRead.ReadString();
            if (binRead.ReadBoolean())
                sMsd = binRead.ReadString();
            else
                sMsd = null;
            dWeight = binRead.ReadDouble();

            //load refernce types if needed -------------------------
            if (bThisTopObject) {
                this.lsett = new LemmatizerSettings(binRead);
                this.lrRule = new LemmaRule(binRead, this.lsett);
            }
            else {
                this.lsett = lsett;
                this.lrRule = lrRule;
            }

            this.sWordRearCache = null;
            this.sWordFrontCache = null;
            this.sLemmaFrontCache = null;
        }
        public LemmaExample(BinaryReader binRead, LemmatizerSettings lsett, LemmaRule lrRule) {
            Deserialize(binRead, lsett, lrRule);
        }

        #endregion
        #region Serialization Functions (Latino)
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.WriteBool(bThisTopObject);

            //save value types --------------------------------------
            binWrt.WriteString(sWord);
            binWrt.WriteString(sLemma);
            binWrt.WriteString(sSignature);
            if (sMsd == null)
                binWrt.WriteBool(false);
            else {
                binWrt.WriteBool(true);
                binWrt.WriteString(sMsd);
            }
            binWrt.WriteDouble(dWeight);

            //save refernce types if needed -------------------------
            if (bThisTopObject) {
                lsett.Save(binWrt);
                lrRule.Save(binWrt, false);
            }
        }
        public void Load(Latino.BinarySerializer binRead, LemmatizerSettings lsett, LemmaRule lrRule) {
            //load metadata
            bool bThisTopObject = binRead.ReadBool();

            //load value types --------------------------------------
            sWord = binRead.ReadString();
            sLemma = binRead.ReadString();
            sSignature = binRead.ReadString();
            if (binRead.ReadBool())
                sMsd = binRead.ReadString();
            else
                sMsd = null;
            dWeight = binRead.ReadDouble();

            //load refernce types if needed -------------------------
            if (bThisTopObject) {
                this.lsett = new LemmatizerSettings(binRead);
                this.lrRule = new LemmaRule(binRead, this.lsett);
            }
            else {
                this.lsett = lsett;
                this.lrRule = lrRule;
            }

        }
        public LemmaExample(Latino.BinarySerializer binRead, LemmatizerSettings lsett, LemmaRule lrRule) {
            Load(binRead, lsett, lrRule);
        }

        #endif
        #endregion
    }
}


