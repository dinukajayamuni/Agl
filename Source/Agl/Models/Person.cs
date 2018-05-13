using System.Collections.Generic;

namespace Agl.Models
{
    internal class Person
    {
        public Person(Gender gender, IEnumerable<Pet> pets)
        {
            Gender = gender;
            Pets = pets;
        }

        public Gender Gender { get; }
        public IEnumerable<Pet> Pets { get; }
    }
}