using System;

namespace LemmaSharp.Classes {

    [Serializable]
    class RuleWeighted: IComparable<RuleWeighted>{

        // Private Variables ---------------------

        private LemmaRule lrRule;
        private double dWeight;
        

        // Constructor(s) & Destructor(s) -------

        public RuleWeighted(LemmaRule lrRule, double dWeight) {
            this.lrRule = lrRule;
            this.dWeight = dWeight;
        }
        

        // Public Properties --------------------

        public LemmaRule Rule {
            get { return lrRule; }
        }
        public double Weight {
            get { return dWeight; }
        }

        
        // Essential Class Functions (comparing objects, eg.: for sorting) -------

        public int CompareTo(RuleWeighted rl) {
            if (this.dWeight < rl.dWeight) return 1;
            if (this.dWeight > rl.dWeight) return -1;
            if (this.lrRule.Id < rl.lrRule.Id) return 1;
            if (this.lrRule.Id > rl.lrRule.Id) return -1;
            return 0;
        }
        

        // Output & Serialization Functions -----------

        public override string ToString() {
            return lrRule.ToString() + dWeight.ToString("(0.00%)");
        }
        
    }
}
