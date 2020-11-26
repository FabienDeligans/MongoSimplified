using System.Collections.Generic;
using System.ComponentModel;
using Core.Models;

namespace CoreTests.InitTests.ModelsTest
{
    public class Family : Entity
    {
        public string FamilyName { get; set; }

        public ICollection<Parent> Parents { get; set; }
        public ICollection<Child> Children { get; set; }
    }
}
