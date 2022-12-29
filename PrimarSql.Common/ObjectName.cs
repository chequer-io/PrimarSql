namespace PrimarSql.Common;

public class ObjectName
{
    public ObjectName(string name)
    {
        Name = name;
    }

    public ObjectName(string schema, string name)
    {
        Schema = schema;
        Name = name;
    }

    public ObjectName(string database, string schema, string name)
    {
        Database = database;
        Schema = schema;
        Name = name;
    }

    public string? Database { get; set; }

    public string? Schema { get; set; }

    public string Name { get; set; }
}
