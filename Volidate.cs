using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba1_Token_
{
    internal class Volidate
    {
        private char[] _separators = { '=', '(', ')', '*', '/', '-', '+', ':', ';' , '\n', '>', '<'}; 
        public static bool VolidateIsID(char c)
        {
            string chars = "" + c;
            return Regex.IsMatch(chars, "^[a-zA-Z]+$");
        }
        public static bool VolidateIsLiteral(char c)
        {
            string chars = "" + c;
            return Regex.IsMatch(chars, "^[0-9]+$");
        }
        public  bool VolidateIsSeparator(char c)
        {
            char separator = c;
            return _separators.Contains(separator);
        }
    }
}