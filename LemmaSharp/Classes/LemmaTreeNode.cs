using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LemmaSharp {
    [Serializable()]
    public class LemmaTreeNode : ILemmatizerModel {
        #region Private Variables

        //settings
        private LemmatizerSettings lsett; 

        //tree structure references
        private Dictionary<char, LemmaTreeNode> dictSubNodes;
        private LemmaTreeNode ltnParentNode;

        //essential node properties
        private int iSimilarity; //similarity among all words in this node
        private string sCondition; //suffix that must match in order to lemmatize
        private bool bWholeWord; //true if condition has to match to whole word

        //rules and weights;
        private LemmaRule lrBestRule; //the best rule to be applied when lemmatizing
        private RuleWeighted[] aBestRules; //list of best rules
        private double dWeight;

        //source of this node
        private int iStart;
        private int iEnd;
        private ExampleList elExamples;

        #endregion

        #region Constructor(s) & Destructor(s)

        private LemmaTreeNode(LemmatizerSettings lsett) {
            this.lsett = lsett;
        }
        public LemmaTreeNode(LemmatizerSettings lsett, ExampleList elExamples)
            : this(lsett, elExamples, 0, elExamples.Count-1, null) {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsett"></param>
        /// <param name="elExamples"></param>
        /// <param name="iStart">Index of the first word of the current group</param>
        /// <param name="iEnd">Index of the last word of the current group</param>
        /// <param name="ltnParentNode"></param>
        private LemmaTreeNode(LemmatizerSettings lsett, ExampleList elExamples, int iStart, int iEnd, LemmaTreeNode ltnParentNode) : this(lsett) {
            this.ltnParentNode = ltnParentNode;
            this.dictSubNodes = null;

            this.iStart = iStart;
            this.iEnd = iEnd;
            this.elExamples = elExamples;

            if (iStart >= elExamples.Count || iEnd >= elExamples.Count || iStart > iEnd) {
                lrBestRule = elExamples.Rules.DefaultRule;
                aBestRules = new RuleWeighted[1];
                aBestRules[0] = new RuleWeighted(lrBestRule, 0);
                dWeight = 0;
                return;
            }


            int iConditionLength = Math.Min(ltnParentNode == null ? 0 : ltnParentNode.iSimilarity + 1, elExamples[iStart].Word.Length);
            this.sCondition = elExamples[iStart].Word.Substring(elExamples[iStart].Word.Length - iConditionLength);
            this.iSimilarity = elExamples[iStart].Similarity(elExamples[iEnd]);
            this.bWholeWord = ltnParentNode == null ? false : elExamples[iEnd].Word.Length == ltnParentNode.iSimilarity;

            FindBestRules();
            AddSubAll();


            //TODO check this heuristics, can be problematic when there are more applicable rules
            if (dictSubNodes != null) {
                List<KeyValuePair<char, LemmaTreeNode>> lReplaceNodes = new List<KeyValuePair<char, LemmaTreeNode>>();
                foreach (KeyValuePair<char, LemmaTreeNode> kvpChild in dictSubNodes)
                    if (kvpChild.Value.dictSubNodes != null && kvpChild.Value.dictSubNodes.Count == 1) {
                        IEnumerator<LemmaTreeNode> enumChildChild = kvpChild.Value.dictSubNodes.Values.GetEnumerator();
                        enumChildChild.MoveNext();
                        LemmaTreeNode ltrChildChild = enumChildChild.Current;
                        if (kvpChild.Value.lrBestRule == lrBestRule)
                            lReplaceNodes.Add(new KeyValuePair<char, LemmaTreeNode>(kvpChild.Key, ltrChildChild));
                    }
                foreach (KeyValuePair<char, LemmaTreeNode> kvpChild in lReplaceNodes) {
                    dictSubNodes[kvpChild.Key] = kvpChild.Value;
                    kvpChild.Value.ltnParentNode = this;
                }

            }

        }

        #endregion

        #region Public Properties

        public int TreeSize {
            get {
                int iCount = 1;
                if (dictSubNodes != null)
                    foreach (LemmaTreeNode ltnChild in dictSubNodes.Values)
                        iCount += ltnChild.TreeSize;
                return iCount;
            }
        }
        public double Weight {
            get {
                return dWeight;
            }
        }

        #endregion

        #region Essential Class Functions (building model)

        private void FindBestRules() {
            /*
             *  LINQ SPEED TEST (Slower than current metodology)
             * 
             
            List<LemmaExample> leApplicable = new List<LemmaExample>();
            for (int iExm = iStart; iExm <= iEnd; iExm++)
                if (elExamples[iExm].Rule.IsApplicableToGroup(sCondition.Length))
                    leApplicable.Add(elExamples[iExm]);

            List<KeyValuePair<LemmaRule, double>> lBestRules = new List<KeyValuePair<LemmaRule,double>>();
            lBestRules.AddRange(
            leApplicable.
                GroupBy<LemmaExample, LemmaRule, double, KeyValuePair<LemmaRule, double>>(
                    le => le.Rule,
                    le => le.Weight,
                    (lr, enumDbl) => new KeyValuePair<LemmaRule, double>(lr, enumDbl.Aggregate((acc, curr) => acc + curr))
                ).
                OrderBy(kvpLrWght=>kvpLrWght.Value)
            );

            if (lBestRules.Count > 0)
                lrBestRule = lBestRules[0].Key;
            else {
                lrBestRule = elExamples.Rules.DefaultRule;

            }
            */

            dWeight = 0;

            //calculate dWeight of whole node and calculates qualities for all rules
            Dictionary<LemmaRule, double> dictApplicableRules = new Dictionary<LemmaRule,double>();
            //dictApplicableRules.Add(elExamples.Rules.DefaultRule, 0);
            while (dictApplicableRules.Count == 0) {
                for (int iExm = iStart; iExm <= iEnd; iExm++) {
                    LemmaRule lr = elExamples[iExm].Rule;
                    double dExmWeight = elExamples[iExm].Weight;
                    dWeight += dExmWeight;

                    if (lr.IsApplicableToGroup(sCondition.Length)) {
                        if (dictApplicableRules.ContainsKey(lr))
                            dictApplicableRules[lr] += dExmWeight;
                        else
                            dictApplicableRules.Add(lr, dExmWeight);
                    }
                }
                //if none found then increase condition length or add some default appliable rule
                if (dictApplicableRules.Count == 0) {
                    if (this.sCondition.Length < iSimilarity)
                        this.sCondition = elExamples[iStart].Word.Substring(elExamples[iStart].Word.Length - (sCondition.Length+1));
                    else
                        //TODO preveri hevristiko, mogoce je bolje ce se doda default rule namesto rulea od starsa
                        dictApplicableRules.Add(ltnParentNode.lrBestRule, 0);
                }
            }
            
            //TODO can optimize this step using sorted list (dont add if it's worse than the worst)
            List<RuleWeighted> lSortedRules = new List<RuleWeighted>();
            foreach (KeyValuePair<LemmaRule, double> kvp in dictApplicableRules)
                lSortedRules.Add(new RuleWeighted(kvp.Key, kvp.Value / dWeight));
            lSortedRules.Sort();

            //keep just best iMaxRulesPerNode rules
            int iNumRules = lSortedRules.Count;
            if (lsett.iMaxRulesPerNode > 0) iNumRules = Math.Min(lSortedRules.Count, lsett.iMaxRulesPerNode);

            aBestRules = new RuleWeighted[iNumRules];
            for (int iRule = 0; iRule < iNumRules; iRule++) {
                aBestRules[iRule] = lSortedRules[iRule];
            }

            
            //set best rule
            lrBestRule = aBestRules[0].Rule;

            
            //TODO must check if this hevristics is OK (to privilige parent rule)
            if (ltnParentNode != null)
                for (int iRule = 0; iRule < lSortedRules.Count && lSortedRules[iRule].Weight==lSortedRules[0].Weight; iRule++) {
                    if (lSortedRules[iRule].Rule == ltnParentNode.lrBestRule) {
                        lrBestRule = lSortedRules[iRule].Rule;
                        break;
                    }
                }
             
        }
        private void AddSubAll() {
            int iStartGroup = iStart;
            char chCharPrev = '\0';
            bool bSubGroupNeeded = false;
            for (int iWrd = iStart; iWrd <= iEnd; iWrd++) {
                string sWord = elExamples[iWrd].Word;

                char chCharThis = sWord.Length > iSimilarity ? sWord[sWord.Length - 1 - iSimilarity] : '\0';

                if (iWrd != iStart && chCharPrev != chCharThis) {
                    if (bSubGroupNeeded) {
                        AddSub(iStartGroup, iWrd - 1, chCharPrev);
                        bSubGroupNeeded = false;
                    }
                    iStartGroup = iWrd;
                }

                //TODO check out bSubGroupNeeded when there are multiple posible rules (not just lrBestRule)
                if (elExamples[iWrd].Rule != lrBestRule)
                    bSubGroupNeeded = true;

                chCharPrev = chCharThis;
            }
            if (bSubGroupNeeded && iStartGroup != iStart) AddSub(iStartGroup, iEnd, chCharPrev);
        }
        private void AddSub(int iStart, int iEnd, char chChar) {
            LemmaTreeNode ltnSub = new LemmaTreeNode(lsett, elExamples, iStart, iEnd, this);
            
            //TODO - maybe not realy appropriate because loosing statisitcs from multiple possible rules
            if (ltnSub.lrBestRule == lrBestRule && ltnSub.dictSubNodes == null) return;

            if (dictSubNodes == null) dictSubNodes = new Dictionary<char, LemmaTreeNode>();
            dictSubNodes.Add(chChar, ltnSub);
        }

        #endregion
        #region Essential Class Functions (running model = lemmatizing)

        public bool ConditionSatisfied(string sWord) {
            //if (bWholeWord)
            //    return sWord == sCondition;
            //else 
            //    return sWord.EndsWith(sCondition);

            int iDiff = sWord.Length - sCondition.Length;
            if (iDiff < 0 || (bWholeWord && iDiff > 0)) return false;

            int iWrdEnd = sCondition.Length - ltnParentNode.sCondition.Length - 1;
            for (int iChar = 0; iChar < iWrdEnd; iChar++)
                if (sCondition[iChar] != sWord[iChar + iDiff])
                    return false;

            return true;
        }
        public string Lemmatize(string sWord) {
            if (sWord.Length >= iSimilarity && dictSubNodes != null) {
                char chChar = sWord.Length > iSimilarity ? sWord[sWord.Length - 1 -iSimilarity] : '\0';
                if (dictSubNodes.ContainsKey(chChar) && dictSubNodes[chChar].ConditionSatisfied(sWord))
                    return dictSubNodes[chChar].Lemmatize(sWord);
            }
            
            return lrBestRule.Lemmatize(sWord);
        }

        #endregion

        #region Output Functions (ToString)

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            ToString(sb, 0);
            return sb.ToString();
        }
        private void ToString(StringBuilder sb, int iLevel) {
            sb.Append(new string('\t', iLevel));
            sb.Append("Suffix=\"" + (bWholeWord?"^":"") + sCondition + "\"; ");
            sb.Append("Rule=\"" + lrBestRule.ToString() + "\"; ");
            sb.Append("Weight=" + dWeight + "\"; ");
            if (aBestRules != null && aBestRules.Length>0) sb.Append("Cover=" + aBestRules[0].Weight + "; ");
            sb.Append("Rulles=");
            if (aBestRules != null)
                foreach (RuleWeighted rw in aBestRules)
                    sb.Append(" " + rw.ToString());
            sb.Append("; ");

            sb.AppendLine();

            if (dictSubNodes != null)
                foreach (LemmaTreeNode ltnChild in dictSubNodes.Values)
                    ltnChild.ToString(sb, iLevel + 1);
        }

        #endregion

        #region Serialization Functions (Binary)

        public void Serialize(BinaryWriter binWrt) {
            binWrt.Write(dictSubNodes != null);
            if (dictSubNodes != null) {
                binWrt.Write(dictSubNodes.Count);
                foreach (KeyValuePair<char, LemmaTreeNode> kvp in dictSubNodes) {
                    binWrt.Write(kvp.Key);
                    kvp.Value.Serialize(binWrt);
                }
            }

            binWrt.Write(iSimilarity);
            binWrt.Write(sCondition);
            binWrt.Write(bWholeWord);

            binWrt.Write(lrBestRule.Signature);
            binWrt.Write(aBestRules.Length);
            for (int i = 0; i < aBestRules.Length; i++) {
                binWrt.Write(aBestRules[i].Rule.Signature);
                binWrt.Write(aBestRules[i].Weight);
            }
            binWrt.Write(dWeight);

            binWrt.Write(iStart);
            binWrt.Write(iEnd);
        }
        public void Deserialize(BinaryReader binRead, LemmatizerSettings lsett, ExampleList elExamples, LemmaTreeNode ltnParentNode) {
            this.lsett = lsett;

            if (binRead.ReadBoolean()) {
                dictSubNodes = new Dictionary<char, LemmaTreeNode>();
                int iCount = binRead.ReadInt32();
                for (int i = 0; i < iCount; i++) {
                    char cKey = binRead.ReadChar();
                    LemmaTreeNode ltrSub = new LemmaTreeNode(binRead, this.lsett, elExamples, this);
                    dictSubNodes.Add(cKey, ltrSub);
                }
            }
            else
                dictSubNodes = null;

            this.ltnParentNode = ltnParentNode;

            iSimilarity = binRead.ReadInt32();
            sCondition = binRead.ReadString();
            bWholeWord = binRead.ReadBoolean();

            lrBestRule = elExamples.Rules[binRead.ReadString()];

            int iCountBest = binRead.ReadInt32();
            aBestRules = new RuleWeighted[iCountBest];
            for (int i = 0; i < iCountBest; i++)
                aBestRules[i] = new RuleWeighted(elExamples.Rules[binRead.ReadString()], binRead.ReadDouble());

            dWeight = binRead.ReadDouble();

            iStart = binRead.ReadInt32();
            iEnd = binRead.ReadInt32();
            this.elExamples = elExamples;
        }
        public LemmaTreeNode(BinaryReader binRead, LemmatizerSettings lsett, ExampleList elExamples, LemmaTreeNode ltnParentNode) {
            Deserialize(binRead, lsett, elExamples, ltnParentNode);
        }

        #endregion
        #region Serialization Functions (Latino)
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt) {
            binWrt.WriteBool(dictSubNodes != null);
            if (dictSubNodes != null) {
                binWrt.WriteInt(dictSubNodes.Count);
                foreach (KeyValuePair<char, LemmaTreeNode> kvp in dictSubNodes) {
                    binWrt.WriteChar(kvp.Key);
                    kvp.Value.Save(binWrt);
                }
            }

            binWrt.WriteInt(iSimilarity);
            binWrt.WriteString(sCondition);
            binWrt.WriteBool(bWholeWord);

            binWrt.WriteString(lrBestRule.Signature);
            binWrt.WriteInt(aBestRules.Length);
            for (int i = 0; i < aBestRules.Length; i++) {
                binWrt.WriteString(aBestRules[i].Rule.Signature);
                binWrt.WriteDouble(aBestRules[i].Weight);
            }
            binWrt.WriteDouble(dWeight);

            binWrt.WriteInt(iStart);
            binWrt.WriteInt(iEnd);
        }
        public void Load(Latino.BinarySerializer binRead, LemmatizerSettings lsett, ExampleList elExamples, LemmaTreeNode ltnParentNode) {
            this.lsett = lsett;

            if (binRead.ReadBool()) {
                dictSubNodes = new Dictionary<char, LemmaTreeNode>();
                int iCount = binRead.ReadInt();
                for (int i = 0; i < iCount; i++) {
                    char cKey = binRead.ReadChar();
                    LemmaTreeNode ltrSub = new LemmaTreeNode(binRead, this.lsett, elExamples, this);
                    dictSubNodes.Add(cKey, ltrSub);
                }
            }
            else
                dictSubNodes = null;

            this.ltnParentNode = ltnParentNode;

            iSimilarity = binRead.ReadInt();
            sCondition = binRead.ReadString();
            bWholeWord = binRead.ReadBool();

            lrBestRule = elExamples.Rules[binRead.ReadString()];

            int iCountBest = binRead.ReadInt();
            aBestRules = new RuleWeighted[iCountBest];
            for (int i = 0; i < iCountBest; i++)
                aBestRules[i] = new RuleWeighted(elExamples.Rules[binRead.ReadString()], binRead.ReadDouble());

            dWeight = binRead.ReadDouble();

            iStart = binRead.ReadInt();
            iEnd = binRead.ReadInt();
            this.elExamples = elExamples;

        }
        public LemmaTreeNode(Latino.BinarySerializer binRead, LemmatizerSettings lsett, ExampleList elExamples, LemmaTreeNode ltnParentNode) {
            Load(binRead, lsett, elExamples, ltnParentNode);
        }

        #endif
        #endregion

        #region Other (Temporarly)

        //TODO - this is temp function, remove it
        public bool CheckConsistency() {
            bool bReturn = true;
            if (dictSubNodes != null)
                foreach (LemmaTreeNode ltnChild in dictSubNodes.Values)
                    bReturn = bReturn &&
                        ltnChild.CheckConsistency() &&
                        ltnChild.sCondition.EndsWith(sCondition);
            return bReturn;
        }

        #endregion
    }
}
