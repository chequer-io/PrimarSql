using System.Diagnostics;
using PrimarSql.Csv;
using PrimarSql.Engine;

var session = new Session(new CsvService("/Users/user/Documents/primarsql_database"));

while (true)
{
    ExecuteResult? result = null;

    try
    {
        Console.Write("CQL> ");

        if (Console.ReadLine() is not { } sql)
            throw new Exception("Query is null.");

        var sw = Stopwatch.StartNew();
        result = session.Execute(sql);
        var reader = result.Reader;
        var data = new List<string[]>(10);
        int count = 0;
        int totalCount = 0;

        data.Add(reader.Columns);

        while (reader.Read())
        {
            if (reader.Current is not { })
                throw new Exception("Current data is null.");

            data.Add(reader.Current.Select(d => d.ToString()).Cast<string>().ToArray());
            count++;
            totalCount++;

            if (count == 10)
            {
                sw.Stop();

                Print(data.ToArray());
                data.RemoveRange(1, data.Count - 1);

                Console.Write("Press <enter> to continue.");
                Console.ReadLine();

                count = 0;
                sw.Start();
            }
        }

        if (data.Count > 1)
        {
            sw.Stop();

            Print(data.ToArray());
            data.RemoveRange(1, data.Count - 1);
        }

        if (reader.Affected == -1)
        {
            Console.WriteLine($"{totalCount} row in set ({sw.Elapsed:s\\.ff} sec)");
        }
        else
        {
            Console.WriteLine($"{reader.Affected} rows affected. ({sw.Elapsed:s\\.ff} sec)");
        }

        Console.WriteLine();

        reader.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        result?.Reader.Dispose();
    }
}

static void Print(string[][] data)
{
    var columnCount = data[0].Length;

    int[] length = Enumerable.Range(0, columnCount).Select(i => data.Select(r => r[i]).Max(v => v.Length)).ToArray();

    for (int i = 0; i < data.Length + 1; i++)
    {
        for (int j = 0; j < columnCount; j++)
        {
            Console.Write("+");
            Console.Write(new string('-', length[j] + 2));
        }

        Console.Write("+");
        Console.WriteLine();

        if (i == data.Length)
            break;

        for (int j = 0; j < columnCount; j++)
        {
            Console.Write("| ");

            var v = data[i][j];
            Console.Write($"{v}{new string(' ', length[j] + 1 - v.Length)}");
        }

        Console.WriteLine("|");
    }
}
