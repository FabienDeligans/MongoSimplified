using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    /// <summary>
    /// All the entities that must be registered in the database
    /// must implement this class
    /// </summary>
    public class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}