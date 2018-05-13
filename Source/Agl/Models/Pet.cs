namespace Agl.Models
{
    internal class Pet
    {
        public Pet(string name, PetType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public PetType Type { get; }
    }
}