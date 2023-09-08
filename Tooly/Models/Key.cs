using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooly.Models
{
    public class Key
    {
        public string TableName { get; private set; }
        public string KeyType { get; set; }
        public string PKColName { get; set; }
        public string FKFromColName { get; set; }
        public string FKToTableName { get; set; }
        public string FKToColName { get; set; }
        public string UKColName { get; set; }

        public Key(string tableName)
        {
            TableName = tableName;
        }

        public override string ToString()
        {
            var constraintName = "";
            var constraintDetails = "";

            if (KeyType.ToUpper() == "PK")
            {
                constraintName = $"{TableName.ToLower()}_pkey";
                constraintDetails = $"PRIMARY KEY ({PKColName.ToLower()})";
            }
            else if (KeyType.ToUpper() == "FK")
            {
                constraintName = $"{TableName.ToLower()}_{FKToTableName.ToLower().Replace(".", "_")}_fk";
                constraintDetails = $"FOREIGN KEY ({FKFromColName.ToLower()}) REFERENCES {FKToTableName.ToLower()}({FKToColName.ToLower()})";
            }
            else if (KeyType.ToUpper() == "UK")
            {
                constraintName = $"{TableName.ToLower()}_{UKColName.ToLower()}_ukey";
                constraintDetails = $"UNIQUE ({UKColName})";
            }

            return $"CONSTRAINT {constraintName} {constraintDetails}";
        }
    }
}
