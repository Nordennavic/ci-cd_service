using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver.Core.Authentication;

namespace RedisService.Models
{
    public class Account
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        private Guid UserId => Guid.NewGuid();
        [BsonElement("nickname")]
        public string Nickname { get; set; }
        [BsonElement("library")]
        public List<string> Library { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("birthday")]
        public DateTime Birthday { get; set; }
        [BsonIgnore]
        private bool IsAdult => DateTime.Today.Year - Birthday.Year == 18;
    }
}
