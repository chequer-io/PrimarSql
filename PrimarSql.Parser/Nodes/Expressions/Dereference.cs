using System.Collections.Generic;
using System.Linq;
using PrimarSql.Common;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class Dereference : Expression
{
    public NodeValue<Expression> Expr { get; }

    public NodeValue<Identifier> Target { get; }

    public Dereference(Expression expr, Identifier target)
    {
        Expr = new NodeValue<Expression>(expr);
        Target = new NodeValue<Identifier>(target);
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(Expr, Target);

    public QualifiedName GetQualifiedName()
    {
        return new QualifiedName(GetIdentifiers().ToArray());
    }

    private IEnumerable<Identifier> GetIdentifiers()
    {
        switch (Expr.Value)
        {
            case Identifier identifier:
                yield return identifier;

                break;

            case Dereference dereference:
                foreach (var i in dereference.GetIdentifiers())
                    yield return i;

                break;

            default:
                throw new NotSupportedFeatureException($"GetIdentifiers from '{Expr.Value.GetType().Name}' Node");
        }

        yield return Target.Value;
    }
}
