using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPuzzle {
    class ReductString {
        public bool Reducted { get; set; } = false;
        public string OriginalText { get; set; }
        public string Text { get; private set; }
        public int Begin { get; set; }
        public int End { get; set; }
        public string After { get; set; }

        public ReductString(bool Reducted) { this.Reducted = Reducted; }
        public ReductString(bool Reducted, int Begin, int End, string After) {
            this.Reducted = Reducted;
            this.Begin = Begin;
            this.End = End;
            this.After = After;
        }

        public string Reduct(int Begin, int End, string After) {
            this.Begin = Begin;
            this.End = End;
            this.After = After;
            return Reduct();
        }

        public string Reduct() {
            Reducted = true;
            if (OriginalText.Length > End) Text = OriginalText.Substring(Begin, End) + After;
            else Text = OriginalText;
            return Text;
        }

        public string Unreduct() {
            Reducted = false;
            Text = OriginalText;
            return Text;
        }
    }
}
