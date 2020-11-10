namespace PrimarSql.Internal
{
    public interface ISqlStatementContext : INode
    {
        IDdlStatementContext DdlStatement { get; }
        
        IDmlStatementContext DmlStatement { get; }
        
        IShowStatementContext ShowStatement { get; }
    }
}
