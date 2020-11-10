namespace PrimarSql.Internal
{
    public interface IDdlStatementContext : INode
    {
        ICreateTableContext CreateTable { get; }
        
        IAlterTableContext AlterTable { get; }
        
        IDropIndexContext DropIndex { get; }
        
        IDropTableContext DropTable { get; }
        
        ITruncateTableContext TruncateTable { get; }
    }
}
