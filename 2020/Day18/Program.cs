using System;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            var calculator = new Calculator();
            var sum = 0L;
            foreach (var l in data)
            {
                var index = 0;
                var expr = calculator.Parse(l, ref index);
                var val =  calculator.Evaluate(expr);
                sum += val;
            }
            System.Console.WriteLine(sum);
        }

        private void SecondStar()
        {
            var data = GetData();
            var calculator = new Calculator();
            var sum = 0L;
            foreach (var l in data)
            {
                var index = 0;
                var expr = calculator.Parse2(l, ref index);
                var val =  calculator.Evaluate(expr);
                sum += val;
            }
            System.Console.WriteLine(sum);
        }

        System.Collections.Generic.List<string> GetData() => File.ReadAllLines("data.txt").ToList();
    }


    public class Calculator
    {

        public long Evaluate(Expression expression)
        {
            if (expression is ConstExpression constExpr)
            {
                return constExpr.Value;
            }
            if (expression is AddExpression addExpr)
            {
                return Evaluate(addExpr.Left) + Evaluate(addExpr.Right);
            }
            if (expression is MultExpression multExpr)
            {
                return Evaluate(multExpr.Left) * Evaluate(multExpr.Right);
            }
            throw new InvalidOperationException();
        }

        public Expression Parse(string data, ref int index)
        {
            Expression currentExpression = null;
            while (index < data.Length)
            {
                if (data[index] == ' ')
                {
                    index++;
                    continue;
                }
                else if (char.IsDigit(data[index]))
                {
                    currentExpression = SubParse(data, ref index);
                }
                else if (data[index] == '(')
                {
                    index++;
                    currentExpression = Parse(data, ref index);
                }
                else if (data[index] == ')')
                {
                    index++;
                    return currentExpression;
                }
                else if (data[index] == '+')
                {
                    index++;
                    currentExpression = new AddExpression(currentExpression, SubParse(data, ref index));
                }
                else if (data[index] == '*')
                {
                    index++;
                    currentExpression = new MultExpression(currentExpression, SubParse(data, ref index));
                }
            }
            return currentExpression;
        }

        public Expression Parse2(string data, ref int index)
        {
            Expression currentExpression = null;
            while (index < data.Length)
            {
                if (data[index] == ' ')
                {
                    index++;
                    continue;
                }
                else if (char.IsDigit(data[index]))
                {
                    currentExpression = SubParse2(data, ref index);
                }
                else if (data[index] == '(')
                {
                    index++;
                    currentExpression = Parse2(data, ref index);
                }
                else if (data[index] == ')')
                {
                    index++;
                    return currentExpression;
                }
                else if (data[index] == '+')
                {
                    index++;
                    currentExpression = new AddExpression(currentExpression, SubParse2(data, ref index));
                }
                else if (data[index] == '*')
                {
                    index++;
                    currentExpression = new MultExpression(currentExpression, Parse2(data, ref index));
                    if (data[index - 1] == ')') {
                        return currentExpression;
                    }
                }
            }
            return currentExpression;
        }

         Expression SubParse2(string data, ref int index)
        {
            while (data[index] == ' ')
                index++;

            if (char.IsDigit(data[index]))
            {
                return new ConstExpression(long.Parse(data[index++].ToString()));
            }
            if (data[index] == '(')
            {
                index++;
                return Parse2(data, ref index);
            }
            throw new InvalidOperationException();
        }

        Expression SubParse(string data, ref int index)
        {
            while (data[index] == ' ')
                index++;

            if (char.IsDigit(data[index]))
            {
                return new ConstExpression(long.Parse(data[index++].ToString()));
            }
            if (data[index] == '(')
            {
                index++;
                return Parse(data, ref index);
            }
            throw new InvalidOperationException();
        }

    }

    public record MultExpression(Expression Left, Expression Right) : Expression;

    public record AddExpression(Expression Left, Expression Right) : Expression;

    public record ConstExpression(long Value) : Expression;

    public record Expression();



}
