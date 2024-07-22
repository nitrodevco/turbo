using System;

namespace Turbo.Database.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DefaultValueSqlAttribute : Attribute
{
    public DefaultValueSqlAttribute(string defaultValueSql)
    {
        DefaultValueSql = defaultValueSql;
    }

    public string DefaultValueSql { get; private set; }
}