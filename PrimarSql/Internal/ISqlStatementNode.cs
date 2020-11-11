namespace PrimarSql.Internal
{
    public interface ISqlStatementNode : INode
    {
        IDdlStatementNode DdlStatement { get; }
        
        IDmlStatementNode DmlStatement { get; }
        
        IShowStatementNode ShowStatement { get; }
    }
}
