using System.Collections.Generic;

namespace Agl.Models
{
    internal class PetsByOwnerGenderGroup
    {
        public PetsByOwnerGenderGroup(Gender gender, IEnumerable<Pet> pets)
        {
            Gender = gender;
            Pets = pets;
        }

        public Gender Gender { get; }
        public IEnumerable<Pet> Pets { get; }
    }
}