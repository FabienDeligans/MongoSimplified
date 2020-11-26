using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreTests.InitTests.ModelsTest
{
    public class Inscription : Entity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DayChoose { get; set; }

        public string ChildId { get; set; }

        public bool M { get; set; }
        public bool R { get; set; }
        public bool Am { get; set; }
    }
}
