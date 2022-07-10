using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooly.Util;

namespace Tooly.Models
{
    public class Table
    {
        public string SchemaName { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public List<Key> Keys { get; set; }

        public Table()
        {
            Fields = new List<Field>();
            Keys = new List<Key>();
        }

        public void AddField(string def)
        {
            def = def.Trim();

            var isArrayType = false;
            if (def.StartsWith("[") &&
            def.Contains("] "))
            {
                isArrayType = true;
                def = def
                .Replace("[", "")
                .Replace("]", "");
            }

            var parts = def.Split(' ');

            var type = parts[0];
            var name = parts[1];
            var length = "";

            if (parts.Length == 3)
            {
                length = parts[2];
            }

            var nullable = name.EndsWith("?");
            name = name.Replace("?", "");

            Fields.Add(new Field
                {
                    Type = type,
                    Name = name,
                    Nullable = nullable,
                    Length = length,
                    IsArrayType = isArrayType
                });
        }

        public void AddKey(string def)
        {
            def = def.Trim().Replace("$", "");

            var parts = def.Split(' ');

            var keyType = "";
            var pkColName = "";
            var fkFromColName = "";
            var fkToTableName = "";
            var fkToColName = "";
            var ukColName = "";

            if (def.StartsWith("PK"))
            {
                keyType = "PK";

                pkColName = parts[1].Trim();
            }
            else if (def.StartsWith("FK"))
            {
                keyType = "FK";

                fkFromColName = parts[1].Trim();

                var fkToParts = parts[2].Split(',');

                fkToTableName = fkToParts[0].Trim();
                fkToColName = fkToParts[1].Trim();
            }
            else if (def.StartsWith("UK"))
            {
                keyType = "UK";

                ukColName = parts[1].Trim();
            }
            else
            {
                throw new ArgumentException($"Don't understand line '{def}'");
            }

            Keys.Add(new Key(Name)
                {
                    KeyType = keyType,
                    PKColName = pkColName,
                    FKFromColName = fkFromColName,
                    FKToTableName = fkToTableName,
                    FKToColName = fkToColName,
                    UKColName = ukColName
                });
        }

        public override string ToString()
        {
            var fullyQualifiedSchemaName = "";
            if (!SchemaName.IsEmpty())
            {
                fullyQualifiedSchemaName = SchemaName + ".";
            }

            var prefix = $"CREATE TABLE {fullyQualifiedSchemaName.ToLower()}{Name.ToLower()} (";
            var suffix = ");";

            var fieldSQLs = Fields.Select(f => "\t" + f.ToString());
            var keySQLs = Keys.Select(k => "\t" + k.ToString());

            return
            prefix +
            "\n" +
            string.Join(",\n", fieldSQLs) + "," +
            "\n\n" +
            string.Join(",\n", keySQLs) +
            "\n" +
            suffix;
        }

        public string ToCSString()
        {
            return $@"
[DAO]
[Validatable]
public partial class {Name}
{{
{string.Join("\n", Fields.Select(f => "    " + f.ToCSString()))}
}}".Trim();
        }

    }
}
