namespace PrimarSql.Internal
{
    public interface ITableNameNode : INode
    {
        IUidNode Uid { get; }
    }
}
