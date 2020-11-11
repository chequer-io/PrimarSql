using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Antlr4.Runtime.Tree;
using PrimarSql.Internal;

namespace PrimarSql.Sample
{
    class Program
    {
 private static void Main(string[] args)
        {
            var defaultColor = Console.ForegroundColor;

            while (true)
            {
                try
                {
                    Console.ForegroundColor = defaultColor;
                    Console.Write("SQL: ");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    var sql = Console.ReadLine();

                    var sw = Stopwatch.StartNew();
                    var statement = PrimarSqlParser.Parse(sql);
                    sw.Stop();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Parsed in {sw.Elapsed.TotalMilliseconds:0.00} ms");

                    Console.WriteLine();

                    Print(statement);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                }

                Console.WriteLine();
            }
        }

        private static void Print(object v, int depth = 0)
        {
            if (depth > 0)
                Console.Write(new string(' ', depth * 4));

            if (v is ITerminalNode terminalNode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Terminal: {terminalNode.GetText()}");
            }
            else if (v is INode node)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(v.GetType().Name);

                foreach (var n in node.Children)
                {
                    Print(n, depth + 1);
                }
            }
        }
    }
}
