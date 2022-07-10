using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooly.Util;

namespace Tooly.Models
{
    public class Field
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Length { get; set; }
        public bool Nullable { get; set; }
        public bool IsArrayType { get; set; }

        public string ToCSString()
        {
            var csType = "";
            var csNullable = "";
            var csArrOpen = "";
            var csArrClose = "";

            switch (Type)
            {
                case "serial":
                case "int":
                    csType = "int";
                    break;

                case "bigserial":
                case "bigint":
                    csType = "long";
                    break;

                case "bool":
                    csType = "bool";
                    break;

                case "string":
                    csType = "string";
                    break;

                case "tz":
                case "timestamp":
                    csType = "DateTime";
                    break;

                case "tzz":
                case "timestampz":
                    csType = "DateTimeOffset";
                    break;

                case "float":
                    csType = "decimal";
                    break;

                case "text":
                    csType = "string";
                    break;

                case "jsonb":
                    csType = "JsonbString";
                    break;

                case "blob":
                    csType = "byte[]";
                    break;

                default:
                    throw new NotImplementedException($"that cs type {Type} is not implemented yet");
            }

            if (Nullable)
            {
                csNullable = "?";
            }
            else
            {
                csNullable = "";
            }

            if (IsArrayType)
            {
                csArrOpen = "List<";
                csArrClose = ">";
            }

            return $"public {csArrOpen}{csType}{csArrClose}{csNullable} {Name.Capitalize()} {{ get; set; }}";
        }

        public override string ToString()
        {
            var pgType = "";
            var pgLen = "";
            var pgNullable = "";
            var pgArr = "";

            switch (Type)
            {
                case "serial":
                    pgType = "serial";
                    break;

                case "bigserial":
                    pgType = "bigserial";
                    break;

                case "int":
                    pgType = "integer";
                    break;

                case "bigint":
                    pgType = "bigint";
                    break;

                case "bool":
                    pgType = "bool";
                    break;

                case "string":
                    pgType = "character varying";
                    if (!IsArrayType)
                    {
                        pgLen = $"({Length})";
                    }
                    break;

                case "tz":
                case "timestamp":
                    pgType = "timestamp without time zone";
                    break;

                case "tzz":
                case "timestampz":
                    pgType = "timestamp with time zone";
                    break;

                case "float":
                    pgType = "decimal";
                    pgLen = $"({Length})";
                    break;

                case "text":
                    pgType = "text";
                    break;

                case "jsonb":
                    pgType = "jsonb";
                    break;

                case "blob":
                    pgType = "bytea";
                    break;

                default:
                    throw new NotImplementedException($"that type {Type} is not implemented yet");
            }

            if (Nullable)
            {
                pgNullable = "NULL";
            }
            else
            {
                pgNullable = "NOT NULL";
            }

            if (IsArrayType)
            {
                pgArr = "[]";
            }

            return $"{Name.ToLower()} {pgType} {pgArr} {pgLen} {pgNullable}";
        }
    }
}
