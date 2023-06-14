using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Laba1_Token_
{
    public class Expression
    {
        List<Token> logicExpressionStack = new List<Token>();
        Stack<string> stackOfOperations = new Stack<string>();
        Stack<int> priorityStack = new Stack<int>();
        int index = 0;
        string output = null;
        Dictionary<string,int> priority = new Dictionary<string, int>() //Список приоритетов для операций
        {
            {"(", 0},
            {")", 1},
            {"or", 2},
            {"and", 3},
            {"<=", 4}, {">=", 4}, {"<", 4}, {">", 4}, {"=",4}, {"<>", 4},
            {"+", 5}, {"-", 5},
            {"*", 6}, {"/", 6}
        };
        public void TakeToken(Token token)//Получение из анализатора логического выражения
        { 
            logicExpressionStack.Add(token);
        }
        public void Start()//Запуск программы
        {
            Decstra();
            ReversePolishNotationInMatrixView();
        }
        private void PushesOutOperationsHighestPriority(string operation)//Вспомогательная функция для метода Дейкстры  
        {
            int count = stackOfOperations.Count();
            Stack<string> temp = new Stack<string>();//Вспомогательный стек для выполнения выталкивания операций с большим приоритетом 
            Stack<int> priorityTemp = new Stack<int>();//Вспомогательный стек приоритетов для выполнения выталкивания операций с большим приоритетом
            for (int i = 0; i < count; i++)//Цикл выталкивания
            {
                if(priorityStack.Peek() >= priority[operation])
                {
                    output += stackOfOperations.Pop();
                    priorityStack.Pop();
                }
                else 
                {
                    temp.Push(stackOfOperations.Pop());
                    priorityTemp.Push(priorityStack.Pop());
                }
            }
            temp.Reverse();
            priorityTemp.Reverse();
            int countTemp = temp.Count();//Цикл возврата операций, которые не были вытолкнуты
            for(int i = 0; i < countTemp; i++)
            {
                stackOfOperations.Push(temp.Pop());
                priorityStack.Push(priorityTemp.Pop());
            }
            stackOfOperations.Push(logicExpressionStack[index].Value);
            priorityStack.Push(priority[operation]);  
        }

        private void Decstra()//Метод Дейкстры
        {
            if (logicExpressionStack[index].Type == Token.TokenType.LPAR)//Начало работы(Первая открывающаяся скобочка просто записывается)
            {
                stackOfOperations.Push(logicExpressionStack[index].Value);
                priorityStack.Push(0);
                index++;
                while(index != logicExpressionStack.Count())//Цикл выполняющий метод дейкстры
                {
                    if(logicExpressionStack[index].Type == Token.TokenType.LITERAL || logicExpressionStack[index].Type == Token.TokenType.IDENTIFIER)
                    {
                        output += logicExpressionStack[index].Value + " ";
                        index++;
                    }

                    else if(logicExpressionStack[index].Type == Token.TokenType.LESS && logicExpressionStack[index + 1].Type == Token.TokenType.EQUALLY)//less or equally
                    {
                        string operation = "<=";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value + logicExpressionStack[index + 1].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.MORE && logicExpressionStack[index + 1].Type == Token.TokenType.EQUALLY)//more or equally
                    {
                        string operation = ">=";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value + logicExpressionStack[index + 1].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.LESS && logicExpressionStack[index + 1].Type == Token.TokenType.MORE)//not equally
                    {
                        string operation = "<>";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value + logicExpressionStack[index + 1].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.LESS)//less
                    {
                        string operation = "<";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.MORE)//more
                    {
                        string operation = ">";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.EQUALLY)//equally
                    {
                        string operation = "=";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.OR)//or
                    {
                        string operation = "or";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.AND)//and
                    {
                        string operation = "and";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.PLUS)//plus
                    {
                        string operation = "+";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    
                    else if (logicExpressionStack[index].Type == Token.TokenType.MINUS)//minus
                    {
                        string operation = "-";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.MULTIPLICATION)//multiplication
                    {
                        string operation = "*";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }
                    
                    else if (logicExpressionStack[index].Type == Token.TokenType.DIVISION)//division
                    {
                        string operation = "/";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            PushesOutOperationsHighestPriority(operation);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.LPAR)//LPAR
                    {
                        string operation = "(";

                        if((priority[operation] > priorityStack.Peek()) || stackOfOperations.Count() == 0)
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else
                        {
                            stackOfOperations.Push(operation);
                            priorityStack.Push(priority[operation]);
                        }
                        index++;
                    }

                    else if (logicExpressionStack[index].Type == Token.TokenType.RPAR)//RPAR
                    {
                        string operation = ")";

                        if((priority[operation] > priorityStack.Peek()))
                        {
                            stackOfOperations.Push(logicExpressionStack[index].Value);
                            priorityStack.Push(priority[operation]);
                        }
                        else if(priority[operation] > priorityStack.Peek() && stackOfOperations.Count() == 0)
                        {
                            PushesOutOperationsHighestPriority(operation);
                            stackOfOperations.Pop();
                            stackOfOperations.Pop();
                            priorityStack.Pop();
                            priorityStack.Pop();
                        }
                        index++;
                    }
                    else
                    {
                        throw new Exception($"Недопустимый символ {stackOfOperations.Peek()}");
                    }
                }
                int countOperations = stackOfOperations.Count();
                for (int i = 0; i < countOperations; i++)//Выталкивание всех оставшихся операций в стеке
                {
                    output += stackOfOperations.Pop();
                } 
            }
        }
        public void ReversePolishNotationInMatrixView()//Метод выполняющий преобразование обратную польскую нотацию в матречный вид
        {
            Dictionary<int, string> M = new Dictionary<int, string>(); 
            Stack<string> stackOperand = new Stack<string>();
            int key = 1;
            for (int i = 0; i < output.Count(); i++)
            {
                char currentChar = output[i];
                switch(currentChar)
                {
                    case('<'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            if (output[i + 1] == '=')
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "<=");
                                stackOperand.Push(M + key.ToString());
                                key++;
                                i++;
                            }
                            else if (output[i + 1] == '>')
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "<>");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                                i++;
                            }
                            else
                            {

                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "<");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                            }
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ: <");
                        }
                        break;
                    }
                    case('>'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            if (output[i + 1] == '=')
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + ">=");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                                i++;
                            }
                            else
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + ">");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                            }
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ: >");
                        }
                        break;
                    }

                    case('='):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "=");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                                
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ: =");
                        }
                        break;
                    }

                    case('o'):
                    {
                        
                        if (output[i + 1] == 'r')
                        {
                            if (stackOperand.Count >= 2)
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "or");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                                i++;
                            }
                            else
                            {
                                throw new Exception($"Недопустимый символ: or");
                            }
                        }
                        else 
                        {
                            stackOperand.Push(currentChar.ToString());
                        }
                        break;
                    }

                    case('a'):
                    {
                        
                        if (output[i + 1] == 'n' && output[i + 2] == 'd')
                        {
                            if (stackOperand.Count >= 2)
                            {
                                M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "and");
                                stackOperand.Push("M" + key.ToString());
                                key++;
                                i++;
                                i++;
                            }
                            else
                            {
                                throw new Exception($"Недопустимый символ: and");
                            }
                        }
                        else 
                        {
                            stackOperand.Push(currentChar.ToString());
                        }
                        
                        break;
                    }

                    case('+'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "+");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ +");
                        }
                        break;
                    }

                    case('-'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "-");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ -");
                        }
                        break;
                    }

                    case('*'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "*");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ: *");
                        }
                        break;
                    }

                    case('/'):
                    {
                        if (stackOperand.Count >= 2)
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "/");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                        }
                        else
                        {
                            throw new Exception($"Недопустимый символ /");
                        }
                        break;
                    }

                    default:
                    {
                        if(Regex.IsMatch(currentChar.ToString(), "^[a-zA-Z]+$") || Regex.IsMatch(currentChar.ToString(), "^[0-9]+$"))
                        {
                            string temp = null;
                            while (output[i] != ' ')
                            {
                                temp += output[i].ToString();
                                i++;
                            }
                            stackOperand.Push(temp);
                        }
                        else if(currentChar == ' ')
                        {
                        }
                        else 
                        {
                            throw new System.Exception($"Недопустимый символ{output[i]}");
                        }
                        break;
                    }
                }
            }
            //Вывод в текст бокс
            Form1._Form1.update("Обратная польская нотация:");
            Form1._Form1.update(output);
            
            Form1._Form1.update("Матречный вид:");
            int countOutput = stackOperand.Count;
            for (int i = 0; i < countOutput; i++)
            {
                Form1._Form1.update(stackOperand.Pop());
            }
            Form1._Form1.update(" ");
            int countM = M.Count;
            for (int i = 1; i < countM + 1; i++)
            {
                Form1._Form1.update("M" + i + ":" + M[i]);
            }
        }
    }
}