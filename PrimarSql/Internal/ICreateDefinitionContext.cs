namespace PrimarSql.Internal
{
    public interface ICreateDefinitionContext : INode
    {
        IUidContext Uid { get; }

        IColumnDefinitionContext ColumnDefinition { get; }
    }
}
