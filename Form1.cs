using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Tiny_Language
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Splite_Click(object sender, EventArgs e)
        {
            string inputString = input.Text;

            if (string.IsNullOrWhiteSpace(inputString))
            {
                MessageBox.Show("Please enter some code first.", "Empty Input");
                return;
            }

            // ── 1. TOKEN PATTERNS (order matters — longest/specific first) ──────
            string comment = @"/\*[^*]*\*+([^/*][^*]*\*+)*/";
            string strLit = @"""[^""]*""";
            string keywords = @"\b(int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|main)\b";
            string boolOp = @"&&|\|\|";
            string condOp = @"<>|<=|>=";
            string assignOp = @":=";
            string number = @"\b[0-9]+(\.[0-9]+)?\b";
            string identifier = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            string singleOp = @"[+\-*/=<>]";
            string symbols = @"[;{}\(\),]";

            // Master pattern — order determines priority
            string masterPattern = $"{comment}|{strLit}|{keywords}|{boolOp}|{condOp}|{assignOp}|{number}|{identifier}|{singleOp}|{symbols}";

            // ── 2. EXTRACT ALL LEXEMES IN ONE PASS ───────────────────────────────
            MatchCollection matches = Regex.Matches(inputString, masterPattern);

            // ── 3. PREPARE RESULTS TABLE ─────────────────────────────────────────
            DataTable dt = new DataTable();
            dt.Columns.Add("Lexeme");
            dt.Columns.Add("Token Type");

            int tokenCount = 0;

            foreach (Match m in matches)
            {
                string lex = m.Value;
                string type = ClassifyToken(lex, keywords, number, strLit, comment,
                                            boolOp, condOp, assignOp, singleOp);
                dt.Rows.Add(lex, type);
                tokenCount++;
            }

            // ── 4. BIND TO DATAGRIDVIEW ───────────────────────────────────────────
            table.DataSource = dt;

            // Optional: show token count
            // label1.Text = "Total Tokens: " + tokenCount;
        }

        // ── TOKEN CLASSIFIER ────────────────────────────────────────────────────
        private string ClassifyToken(string lex, string kw, string num,
                                     string str, string comment,
                                     string boolOp, string condOp,
                                     string assignOp, string singleOp)
        {
            // Comments
            if (Regex.IsMatch(lex, @"^/\*.*\*/$", RegexOptions.Singleline))
                return "Comment";

            // String literals
            if (Regex.IsMatch(lex, @"^""[^""]*""$"))
                return "String";

            // Reserved keywords
            if (Regex.IsMatch(lex, @"^\b(int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|main)\b$"))
                return "Keyword";

            // Boolean operators
            if (lex == "&&") return "AND_Op";
            if (lex == "||") return "OR_Op";

            // Multi-char condition operators
            if (lex == "<>") return "NotEqual_Op";
            if (lex == "<=") return "LessOrEqual_Op";
            if (lex == ">=") return "GreaterOrEqual_Op";

            // Assignment operator
            if (lex == ":=") return "Assignment_Op";

            // Numbers (int & float)
            if (Regex.IsMatch(lex, @"^\b[0-9]+(\.[0-9]+)?\b$"))
                return "Number";

            // Single-char operators & relations
            if (lex == "+") return "Plus_Op";
            if (lex == "-") return "Minus_Op";
            if (lex == "*") return "Multiply_Op";
            if (lex == "/") return "Divide_Op";
            if (lex == "=") return "Equal_Op";
            if (lex == "<") return "LessThan_Op";
            if (lex == ">") return "GreaterThan_Op";

            // Symbols / delimiters
            if (lex == ";") return "Semicolon";
            if (lex == "(") return "Left_Paren";
            if (lex == ")") return "Right_Paren";
            if (lex == "{") return "Left_Brace";
            if (lex == "}") return "Right_Brace";
            if (lex == ",") return "Comma";

            // Identifiers — letter followed by letters/digits
            if (Regex.IsMatch(lex, @"^\b[a-zA-Z][a-zA-Z0-9]*\b$"))
                return "Identifier";

            return "Unknown";
        
    }
    }
}
