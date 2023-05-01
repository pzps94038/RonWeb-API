using System;
namespace RonWeb.Database.Mongo.MongoAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoAttribute: Attribute
	{
        public string TableName { get; }

        public MongoAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}

