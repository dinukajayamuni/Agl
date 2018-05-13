using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Agl.Models;

namespace Agl.Services
{
    internal class PersonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _peopleStoreUrl;

        /// <summary>
        ///     Creates people service
        /// </summary>
        /// <param name="httpClientFactory">The http client factory</param>
        /// <param name="peopleStoreUrl">The full url of the people store</param>
        public PersonService(IHttpClientFactory httpClientFactory, string peopleStoreUrl)
        {
            _httpClientFactory = httpClientFactory;
            _peopleStoreUrl = peopleStoreUrl;
        }

        /// <summary>
        ///     Gets all the people from the people store
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Person>> GetAllPeople()
        {
            using (var httpClient = _httpClientFactory.Create())
            {
                using (var response = await httpClient.GetAsync(_peopleStoreUrl))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpException((int) response.StatusCode, response.ReasonPhrase);
                    return await response.Content.ReadAsAsync<IEnumerable<Person>>();
                }
            }
        }

        /// <summary>
        ///     Gets all the people from the people store. Filters the poeple with cats and group cat's by their owner's
        ///     gender. The gender groups are odered by cat names
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PetsByOwnerGenderGroup>> GetCatsByOwnerGender()
        {
            var people = await GetAllPeople();

            return from person in people
                where person.Pets != null
                from pet in person.Pets
                where pet.Type == PetType.Cat
                group pet by person.Gender
                into genderGroup
                select new PetsByOwnerGenderGroup(genderGroup.Key, genderGroup.OrderBy(p => p.Name));
        }
    }
}