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
            foreach (var genderGroup in catsByOwnerGender)
            {
                Console.WriteLine(genderGroup.Gender);
                foreach (var pet in genderGroup.Pets)
                    Console.WriteLine($"  - {pet.Name}");
            }
        }
    }
}