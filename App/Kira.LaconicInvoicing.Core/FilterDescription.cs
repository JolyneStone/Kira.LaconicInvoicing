using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public class FilterDescription
    {
        private static readonly IDictionary<string, string> dicCommonOperators = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) {
           {"eq","=="}, {"neq","!="},{"lt","<"},{"lte","<="},{"gt",">"},{"gte",">="}
        };
        private static readonly IDictionary<string, string> dicStringOperators = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) {
           {"startswith","StartsWith"},{"endswith","EndsWith"},{"contains","Contains"}
        };

        public string Operator { get; set; }
        public string Field { get; set; }
        public object Value { get; set; }

        public QueryFilterItem ToFilterItem()
        {
            return new QueryFilterItem { Field = Field, Operator = CodeOperator, Value = Value.ToString() };
        }

        public bool IsStringMethodOperator
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Operator))
                    return false;

                if (dicStringOperators.ContainsKey(Operator))
                    return true;

                return false;
            }
        }

        public string CodeOperator
        {
            get
            {
                string op = "==";
                if (!String.IsNullOrEmpty(Operator))
                {
                    string tmpOp = null;
                    if (dicStringOperators.TryGetValue(Operator, out tmpOp))
                        op = tmpOp;
                    else if (dicCommonOperators.TryGetValue(Operator, out tmpOp))
                        op = tmpOp;
                }

                return op;
            }
        }

        public override string ToString()
        {
            bool isStringOp = false;
            string op = "==";
            if (!String.IsNullOrEmpty(Operator))
            {
                string tmpOp = null;
                if (dicStringOperators.TryGetValue(Operator, out tmpOp))
                {
                    op = tmpOp;
                    isStringOp = true;
                }
                else if (dicCommonOperators.TryGetValue(Operator, out tmpOp))
                    op = tmpOp;
            }

            return isStringOp ? String.Format("{0}.{1}(\"{2}\")", Field, op, Value) : String.Format("{0} {1} {2}", Field, op, Value);
        }
    }
}
