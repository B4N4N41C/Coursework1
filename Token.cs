using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Laba1_Token_
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);
        }

        public enum TokenType
        {
            DIM, AS, IDENTIFIER, LITERAL, INTEGER, LONG, DOUBLE, STRING,
            DO, WHILE, ENTER, OR, AND,  MORE, LESS, EQUALLY, PLUS, LPAR, MULTIPLICATION,
            RPAR, LOOP, MINUS, IF, ELSE, COMMA, DOT, EXPR, DIVISION
        }
        static TokenType[] Delimiters = new TokenType[]
        {
            TokenType.ENTER, TokenType.PLUS,
            TokenType.MINUS, TokenType.EQUALLY,
            TokenType.RPAR, TokenType.LPAR
        };

        public static bool IsDelimiter(Token token)
        {
            return Delimiters.Contains(token.Type);
        }

        public static Dictionary<string, TokenType> SpecialWords =
        new Dictionary<string, TokenType>()
        {
            {"integer", TokenType.INTEGER },
            {"double", TokenType.DOUBLE},
            {"long", TokenType.LONG},
            {"string", TokenType.STRING },
            {"else", TokenType.ELSE },
            {"Dim", TokenType.DIM},
            {"as", TokenType.AS},
            {"do", TokenType.DO},
            {"while", TokenType.WHILE},
            {"\n", TokenType.ENTER},
            {"or", TokenType.OR},
            {"and", TokenType.AND},
            {">", TokenType.MORE},
            {"<", TokenType.LESS},
            {"=", TokenType.EQUALLY},
            {"+", TokenType.PLUS},
            {"-", TokenType.MINUS},
            {"*", TokenType.MULTIPLICATION},
            {":", TokenType.DIVISION},
            {"loop", TokenType.LOOP},
            {"if", TokenType.IF},
            {")", TokenType.RPAR},
            {"(", TokenType.LPAR},
            {",", TokenType.COMMA},
            {"expr", TokenType.EXPR}
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (SpecialWords.ContainsKey(word));
        }
    }
}
