namespace PrimarSql.Engine.Planners;

public struct PlanId
{
    private static int _id;

    public int Id { get; }

    private PlanId(int id)
    {
        Id = id;
    }

    public static PlanId Create()
    {
        return new PlanId(++_id);
    }
}
