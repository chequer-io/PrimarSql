namespace PrimarSql.Internal
{
    public interface IDdlStatementNode : INode
    {
        ICreateTableNode CreateTable { get; }
        
        IAlterTableNode AlterTable { get; }
        
        IDropIndexNode DropIndex { get; }
        
        IDropTableNode DropTable { get; }
        
        ITruncateTableNode TruncateTable { get; }
    }
}
