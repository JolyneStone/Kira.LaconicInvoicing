using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public sealed class ParseException : Exception
    {
        public ParseException(string message, int position) : base(message)
        {
            this.position = position;
        }

        public int Position
        {
            get
            {
                return this.position;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} （索引位置 {1}）", this.Message, this.position);
        }

        private int position;
    }
}
