using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public class QuerySpecification : Relation
{
    public Select Select { get; }

    public Relation? Relation { get; }

    public QuerySpecification(NodePosition position, Select select, Relation? relation) : base(position)
    {
        Select = select;
        Relation = relation;
    }

    public override IEnumerable<T> GetChildrens<T>()
    {
        yield break;
    }
}
