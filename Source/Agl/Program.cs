using System;
using System.Configuration;
using Agl.Services;

namespace Agl
{
    public class Program
    {
        private static void Main()
        {
            var peopleStoreUrl = ConfigurationManager.AppSettings["PeopleUrl"];
            var personService = new PersonService(new HttpClientFactory(), peopleStoreUrl);
            var catsByOwnerGender = personService.GetCatsByOwnerGender().GetAwaiter().GetResult();
            foreach (var ownerGenderGroup in catsByOwnerGender)
            {
                Console.WriteLine(ownerGenderGroup.Gender);
                foreach (var pet in ownerGenderGroup.Pets)
                    Console.WriteLine($"  - {pet.Name}");
            }
        }
    }
}