using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace AstolfoBot.Modules.MathM
{
    public class MathParser
    {
        public static Dictionary<string, Func<double, double>> Functions = new Dictionary<string, Func<double, double>>()
        {
            { "sin", Math.Sin },
            { "cos", Math.Cos },
            { "tan", Math.Tan },
            { "asin", Math.Asin },
            { "acos", Math.Acos },
            { "atan", Math.Atan },
            { "sqrt", Math.Sqrt },
            { "abs", Math.Abs },
            { "log", Math.Log },
            { "exp", Math.Exp },
            { "floor", Math.Floor },
            { "ceil", Math.Ceiling },
            { "round", Math.Round }
        };

        public static double Parse(string input)
        {
            var tokens = Tokenize(input);
            var rpn = ShuntingYard(tokens);
            return Evaluate(rpn, Functions);
        }

        private static Token[] Tokenize(string input)
        {
            var tokens = new List<Token>();
            var sb = new StringBuilder();
            var fsb = new StringBuilder();
            foreach (var c in input)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    sb.Append(c);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.Number, sb.ToString()));
                        sb.Clear();
                    }

                    if (c == '(')
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.LeftParenthesis, c.ToString()));
                    }
                    else if (c == ')')
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.RightParenthesis, c.ToString()));
                    }
                    else if (c == '+' || c == '-')
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    }
                    else if (c == '*' || c == '/')
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    }
                    else if (c == ',')
                    {
                        if (fsb.Length > 0)
                        {
                            tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                            fsb.Clear();
                        }
                        tokens.Add(new Token(TokenType.Comma, c.ToString()));
                    }
                    else if (char.IsLetter(c))
                    {
                        fsb.Append(c);
                    }
                }
            }

            if (sb.Length > 0)
            {
                if (fsb.Length > 0)
                {
                    tokens.Add(new Token(TokenType.Function, fsb.ToString()));
                    fsb.Clear();
                }
                tokens.Add(new Token(TokenType.Number, sb.ToString()));
            }

            return tokens.ToArray();
        }

        private static Token[] ShuntingYard(Token[] tokens)
        {
            var output = new List<Token>();
            var stack = new Stack<Token>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    output.Add(token);
                }
                else if (token.Type == TokenType.Operator)
                {
                    while (stack.Count > 0 && stack.Peek().Type == TokenType.Operator)
                    {
                        if (stack.Peek().Value == "*" || stack.Peek().Value == "/")
                        {
                            output.Add(stack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }

                    stack.Push(token);
                }
                else if (token.Type == TokenType.LeftParenthesis)
                {
                    stack.Push(token);
                }
                else if (token.Type == TokenType.RightParenthesis)
                {
                    while (stack.Count > 0 && stack.Peek().Type != TokenType.LeftParenthesis)
                    {
                        output.Add(stack.Pop());
                    }

                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else if (token.Type == TokenType.Comma)
                {
                    while (stack.Count > 0 && stack.Peek().Type != TokenType.LeftParenthesis)
                    {
                        output.Add(stack.Pop());
                    }
                }
                else if (token.Type == TokenType.Number)
                {
                    output.Add(token);
                }
                else if (token.Type == TokenType.Function)
                {
                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output.ToArray();
        }

        private static double Evaluate(Token[] tokens, Dictionary<string, Func<double, double>> functions)
        {
            var stack = new Stack<double>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    stack.Push(double.Parse(token.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (token.Type == TokenType.Operator)
                {
                    var b = stack.Pop();
                    var a = stack.Pop();
                    switch (token.Value)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;
                        case "-":
                            stack.Push(a - b);
                            break;
                        case "*":
                            stack.Push(a * b);
                            break;
                        case "/":
                            stack.Push(a / b);
                            break;
                    }
                }
                else if (token.Type == TokenType.Function)
                {
                    var a = stack.Pop();
                    stack.Push(functions[token.Value](a));
                }
            }

            return stack.Pop();
        }
    }

    public class Token
    {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; }
        public string Value { get; }
    }

    public enum TokenType
    {
        None,
        Number,
        Operator,
        LeftParenthesis,
        RightParenthesis,
        Comma,
        Function
    }

}