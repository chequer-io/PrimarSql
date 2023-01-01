using PrimarSql.Common.Data;

namespace PrimarSql.Engine;

public class ExecuteResult
{
    public DataPageReader Reader { get; }

    // TODO: Elapsed time (execute, fetch)

    internal ExecuteResult(DataPageReader reader)
    {
        Reader = reader;
    }
}
