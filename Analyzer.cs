using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Laba1_Token_
{
    public class Analyzer
    {
        Token currentToken;

        List<Token> currentTokens = new List<Token>();

        public bool finish;

        int index;

        public Analyzer(List<Token> tokens)
        {
            currentTokens = tokens;
        }
        private void Exception(Token.TokenType wrongType)
        {
            throw new Exception($"Ожидалось конец строки, а было получено {wrongType},{index}");
        }

        private void Exception(Token.TokenType trueType, Token.TokenType wrongType)
        {
            throw new Exception($"Ожидалось {trueType}, а было получено {wrongType},{index}");
        }

        private void Exception(Token.TokenType trueType, Token.TokenType trueType1, Token.TokenType wrongType)
        {
            throw new Exception($"Ожидалось {trueType} или {trueType1}, а было получено {wrongType},{index}");
        }

        private void Exception(Token.TokenType trueType, Token.TokenType trueType1, Token.TokenType trueType2, Token.TokenType wrongType)
        {
            throw new Exception($"Ожидалось {trueType} или {trueType1}, или {trueType2} а было получено {wrongType},{index}");
        }

        private void Exception(Token.TokenType trueType, Token.TokenType trueType1, Token.TokenType trueType2, Token.TokenType trueType3, Token.TokenType wrongType)
        {
            throw new Exception($"Ожидалось {trueType} или {trueType1}, или {trueType2}, или {trueType3} а было получено {wrongType},{index}");
        }

        private void Next()
        {
            index++;
            if (index < currentTokens.Count)
            {
                currentToken = currentTokens[index];
            }
        }
        public void Begin()
        {
            index = 0;

            currentToken = currentTokens[index];
            try
            {
                Programm();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR 404");
            }
        }

        public void Expresion()
        {
            Expression expression = new Expression();

            while (currentToken.Type != Token.TokenType.ENTER)
            {
                expression.TakeToken(currentToken);
                Next();
            }

            expression.Start();
        }

        private void Programm()//Программа
        {
            finish = false;

            if (currentToken.Type != Token.TokenType.DIM)
                Exception(Token.TokenType.DIM, currentToken.Type);

            Next();

            ListOfDescriptions();

            ListOfOperators();

            if (currentToken.Type != Token.TokenType.ENTER)
                Exception(currentToken.Type);

            Next();

            finish = true;
        }

        private void ListOfDescriptions()//Список описание
        {
            Description();
            Y();
        }

        private void AdditionalDescription() //Дополнительное описание
        {
            Description();

            if (currentToken.Type != Token.TokenType.ENTER)
                Exception(currentToken.Type);

            Next();

            Y();
        }

        private void Y()
        {
            if (currentToken.Type == Token.TokenType.ENTER)
                AdditionalDescription();
        }

        private void Description()//Описание
        {
            ListOfVariables();

            

            Type();

            if (currentToken.Type != Token.TokenType.ENTER)
                Exception(currentToken.Type);

            Next();
        }

        private void ListOfVariables()//Список переменных
        {
            if (currentToken.Type != Token.TokenType.IDENTIFIER)
                Exception(Token.TokenType.IDENTIFIER, currentToken.Type);

            Next();

            X();
        }
        private void AdditionalVariables()//Доолнительная переменная
        {
            if (currentToken.Type != Token.TokenType.COMMA)
                Exception(Token.TokenType.COMMA, currentToken.Type);

            Next();

            if (currentToken.Type != Token.TokenType.IDENTIFIER)
                Exception(Token.TokenType.IDENTIFIER, currentToken.Type);

            Next();

            X();
        }

        private void X()
        {
            if (currentToken.Type == Token.TokenType.AS)
                Next();
            else if (currentToken.Type == Token.TokenType.COMMA)
                AdditionalVariables();
            else
                Exception(Token.TokenType.COMMA, Token.TokenType.AS, currentToken.Type);
        }

        private void Type()//Тип
        {
            if (currentToken.Type == Token.TokenType.INTEGER)
                Next();
            else if (currentToken.Type == Token.TokenType.LONG)
                Next();
            else if (currentToken.Type == Token.TokenType.DOUBLE)
                Next();
            else
                Exception(Token.TokenType.IDENTIFIER, Token.TokenType.LONG, Token.TokenType.DOUBLE, currentToken.Type);
        }

        private void ListOfOperators()//Список операторов
        {
            
            Operator();
            Z();
        }

        private void AdditionalOperator()//Дополнительный оператор
        {
            
            Operator();
            Z();
        }

        private void Z()
        {
            if (currentToken.Type == Token.TokenType.ENTER)
                Next();
            else if (currentToken.Type == Token.TokenType.DO || currentToken.Type == Token.TokenType.IDENTIFIER)
                AdditionalOperator();
            else
                Exception(Token.TokenType.ENTER, Token.TokenType.DO, Token.TokenType.IDENTIFIER, currentToken.Type);
        }

        private void Operator()//Оператор
        {
            if (currentToken.Type == Token.TokenType.DO)
                Conditional();
            else if (currentToken.Type == Token.TokenType.IDENTIFIER)
                Assignment();
            else
                Exception(Token.TokenType.IDENTIFIER, Token.TokenType.DO, currentToken.Type);
        }


        private void Conditional() //Условный оператор
        {
            if (currentToken.Type != Token.TokenType.DO)
                Exception(Token.TokenType.DO, currentToken.Type);

            Next();

            if (currentToken.Type != Token.TokenType.WHILE)
                Exception(Token.TokenType.WHILE, currentToken.Type);

            Next();

            if (currentToken.Type == Token.TokenType.LPAR)
                Expresion();

            Next();

            OperatorBlock();

            if (currentToken.Type != Token.TokenType.LOOP)
                Exception(Token.TokenType.LOOP, currentToken.Type);

            Next();
        }

        private void OperatorBlock()//Блок операторов
        {
            Operator();

            if (currentToken.Type != Token.TokenType.ENTER)
                Exception(currentToken.Type);

            Next();

            if (currentToken.Type != Token.TokenType.LOOP && currentToken.Type != Token.TokenType.ENTER)
                ListOfOperators();
            
            
        }

        private void Assignment() //Присвоение
        {
            if (currentToken.Type != Token.TokenType.IDENTIFIER)
                Exception(Token.TokenType.IDENTIFIER, currentToken.Type);

            Next();

            if (currentToken.Type != Token.TokenType.EQUALLY)
                Exception(Token.TokenType.EQUALLY, currentToken.Type);

            Next();

            Operand();

            U();
        }

        private void U()
        {
            if (
                currentToken.Type == Token.TokenType.PLUS ||
                currentToken.Type == Token.TokenType.MINUS ||
                currentToken.Type == Token.TokenType.MULTIPLICATION ||
                currentToken.Type == Token.TokenType.DIVISION
               )
            {
                Sign();
                Operand();
            }
            else if (currentToken.Type == Token.TokenType.ENTER)
                Next();
            else
                Exception(currentToken.Type);
        }

        private void Sign()//Знак
        {
            if (currentToken.Type == Token.TokenType.PLUS)
                Next();
            else if (currentToken.Type == Token.TokenType.MINUS)
                Next();
            else if (currentToken.Type == Token.TokenType.MULTIPLICATION)
                Next();
            else if (currentToken.Type == Token.TokenType.DIVISION)
                Next();
            else
                Exception(Token.TokenType.PLUS, Token.TokenType.MINUS, Token.TokenType.MULTIPLICATION, Token.TokenType.DIVISION, currentToken.Type);
        }

        private void Operand()//Операнд
        {
            if (currentToken.Type == Token.TokenType.IDENTIFIER)
                Next();
            else if (currentToken.Type == Token.TokenType.LITERAL)
                Next();
            else
                Exception(Token.TokenType.IDENTIFIER, Token.TokenType.LITERAL, currentToken.Type);
        }
    }
}