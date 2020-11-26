using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.CustomAttribute;
using Core.Models;

namespace CoreTests.InitTests.ModelsTest
{
    public class Parent : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string Cp { get; set; }
        public string City { get; set; }

        [FK(typeof(Family))]
        public string FamilyId { get; set; }
        public Family Family { get; set; }

    }
}
