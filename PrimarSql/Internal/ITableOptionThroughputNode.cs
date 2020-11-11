namespace PrimarSql.Internal
{
    public interface ITableOptionThroughputNode : INode
    {
        IDecimalLiteralNode ReadCapacity { get; }
        
        IDecimalLiteralNode WriteCapacity { get; }
    }
}
