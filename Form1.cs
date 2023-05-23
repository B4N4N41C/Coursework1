using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1_Token_
{
    public partial class Form1 : Form
    {
        List<Token> tokens = new List<Token>();
        List<string> lexemes = new List<string>();
        public Form1()
        {
            InitializeComponent();
            textBox3.Text = "\\\\Mac\\Home\\Documents\\Programming\\AutomataTheory\\Laba1(Token)\\code.txt";
            _Form1 = this;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            string path = textBox3.Text;
            StreamReader text = new StreamReader(path);
            string code = text.ReadToEnd();
            textBox1.Text = code;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            char[] chars = textBox1.Text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Volidate.VolidateIsID(chars[i]) == true)
                {
                    string currentLexem = "";
                    while (Volidate.VolidateIsID(chars[i]) == true && i < chars.Length)
                    {
                        currentLexem += chars[i];
                        i++;
                    }
                    lexemes.Add($"{currentLexem} I");
                    textBox2.Text += $"{currentLexem} - Индификатор \n" + Environment.NewLine;
                }
                if (Volidate.VolidateIsLiteral(chars[i]) == true)
                {
                    string litetal = "";
                    while (Volidate.VolidateIsLiteral(chars[i]) == true && i < chars.Length)
                    {
                        litetal += chars[i];
                        i++;
                    }
                    lexemes.Add($"{litetal} L");
                    textBox2.Text += $"{litetal} - Литерал \n" + Environment.NewLine;
                }
                Volidate volidate = new Volidate();
                if (volidate.VolidateIsSeparator(chars[i]) == true)
                {
                    if (chars[i] == '\n')
                    {
                        textBox2.Text += "# - Разделитель \n" + Environment.NewLine;
                        lexemes.Add($"{chars[i]} #");
                    }
                    else
                    {
                        textBox2.Text += $"{chars[i]} - Разделитель \n" + Environment.NewLine;
                        lexemes.Add($"{chars[i]} R");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Token token;
            string content;
            string type;
            for (int i = 0; i < lexemes.Count; i++)
            {
                content = lexemes[i].Split(' ')[0];
                type = lexemes[i].Split(' ')[1];
                if (type == "I")
                {
                    if (Token.IsSpecialWord(content))
                    {
                        token = new Token(Token.SpecialWords[content]);
                        token.Value = content;
                        tokens.Add(token);
                        textBox4.Text += $"{token}" + Environment.NewLine;
                        continue;
                    }

                    else
                    {
                        token = new Token(Token.TokenType.IDENTIFIER);
                        token.Value = content;
                        tokens.Add(token);
                        textBox4.Text += $"{token}" + Environment.NewLine;
                        continue;
                    }
                }
                else if (type == "L")
                {
                    token = new Token(Token.TokenType.LITERAL);
                    token.Value = content;
                    tokens.Add(token);
                    textBox4.Text += $"{token}" + Environment.NewLine;
                    continue;
                }
                else if (type == "R")
                {
                    token = new Token(Token.SpecialWords[content]);
                    token.Value = content;
                    tokens.Add(token);
                    textBox4.Text += $"{token}" + Environment.NewLine;
                    continue;
                }
                else if (type == "#")
                {
                    token = new Token(Token.SpecialWords[content]);
                    token.Value = content;
                    tokens.Add(token);
                    textBox4.Text += $"{token}" + Environment.NewLine;
                    
                    continue;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            Analyzer analyzer = new Analyzer(tokens);

            analyzer.Begin();
            if (analyzer.finish == true)
                MessageBox.Show("Анализ прошел успешно, ошибок не выявлено");
            else
                MessageBox.Show("Анализ прошел успешно, присутствуют ошибки");
            lexemes.Clear();
            tokens.Clear();
        }

        public static Form1 _Form1;
        public void update(string message)
        {
            textBox5.Text += message + Environment.NewLine;
        }
    }
}
