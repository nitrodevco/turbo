using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueSqlAttribute : Attribute
    {
        public string DefaultValueSql { get; private set; }

        public DefaultValueSqlAttribute(string defaultValueSql)
        {
            DefaultValueSql = defaultValueSql;
        }
    }
}
