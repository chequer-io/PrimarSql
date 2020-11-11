namespace PrimarSql.Internal
{
    public interface ICreateDefinitionNode : INode
    {
        IUidNode Uid { get; }

        IColumnDefinitionNode ColumnDefinition { get; }
    }
}
