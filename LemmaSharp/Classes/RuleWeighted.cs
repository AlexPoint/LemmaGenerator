using System;
using System.Collections.Generic;
using System.Text;

namespace LemmaSharp {
    [Serializable()]
    class RuleWeighted: IComparable<RuleWeighted>{
        #region Private Variables

        private LemmaRule lrRule;
        private double dWeight;

        #endregion

        #region Constructor(s) & Destructor(s)

        public RuleWeighted(LemmaRule lrRule, double dWeight) {
            this.lrRule = lrRule;
            this.dWeight = dWeight;
        }

        #endregion

        #region Public Properties

        public LemmaRule Rule {
            get { return lrRule; }
        }
        public double Weight {
            get { return dWeight; }
        }

        #endregion

        #region Essential Class Functions (comparing objects, eg.: for sorting)

        public int CompareTo(RuleWeighted rl) {
            if (this.dWeight < rl.dWeight) return 1;
            if (this.dWeight > rl.dWeight) return -1;
            if (this.lrRule.Id < rl.lrRule.Id) return 1;
            if (this.lrRule.Id > rl.lrRule.Id) return -1;
            return 0;
        }

        #endregion

        #region Output & Serialization Functions

        public override string ToString() {
            return lrRule.ToString() + dWeight.ToString("(0.00%)");
        }

        #endregion
    }
}
