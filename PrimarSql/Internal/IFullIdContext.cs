namespace PrimarSql.Internal
{
    public interface IFullIdContext : INode
    {
        IUidContext Uid { get; }
        
        IDottedIdContext DottedId { get; }
    }
}
