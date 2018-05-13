using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Agl.Models;
using Agl.Services;
using NSubstitute;
using TestStack.BDDfy;
using Xunit;

namespace Agl.Tests
{
    public class PersonServiceTests
    {
        private IHttp _http;
        private PersonService _personService;
        private IEnumerable<PetsByOwnerGenderGroup> _catsByOwnerGender;
        private Exception _error;

        private void AInstanceOfPersonService()
        {
            _http = Substitute.For<IHttp>();
            _personService = new PersonService(new FakeHttpClientFactory(_http), "http://test.com");
        }

        private void AValidResponseIsReceivedFromThePeopleStore()
        {
            var json = File.ReadAllText("Data/valid-response.json");
            SetupHttpResponseMessage(json);
        }

        private void AnUnsuccessfulResponseIsReceivedFromThePeopleStore()
        {
            SetupHttpResponseMessage("[]", HttpStatusCode.BadRequest);
        }

        private void APeopleListWithPeopleHasNoPetsIsReceivedFromThePeopleStore()
        {
            var json = File.ReadAllText("Data/person-with-no-pets.json");
            SetupHttpResponseMessage(json);
        }

        private void AnEmptyPeopleListIsReceivedFromThePeopleStore()
        {
            SetupHttpResponseMessage("[]");
        }

        private void SetupHttpResponseMessage(string json, HttpStatusCode status = HttpStatusCode.OK)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = status,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            _http.Send(Arg.Any<HttpRequestMessage>()).Returns(response);
        }

        private async Task CatsByOwnerGenderIsRetreived()
        {
            try
            {
                _catsByOwnerGender = await _personService.GetCatsByOwnerGender();
            }
            catch (Exception exception)
            {
                _error = exception;
            }
        }

        private void TheGenderGroupsHasDistinctValues()
        {
            Assert.All(Enum.GetValues(typeof(Gender)).Cast<Gender>(),
                gender => { Assert.True(_catsByOwnerGender.Count(g => g.Gender == gender) <= 1); });
        }

        private void ThePetsAreOrderedByName()
        {
            Assert.All(_catsByOwnerGender, group =>
            {
                var orderedPetNames = group.Pets.OrderBy(p => p.Name).Select(p => p.Name);
                var petNames = group.Pets.Select(p => p.Name);
                Assert.Equal(orderedPetNames, petNames);
            });
        }

        private void ThePetsByOwnerGenderGroupsAreEmpty()
        {
            Assert.True(!_catsByOwnerGender.Any());
        }

        private void ThePetsByOwnerGenderGroupsAreNotEmpty()
        {
            Assert.True(_catsByOwnerGender.Any());
        }

        private void ThePersonServiceThrewHttpException()
        {
            Assert.NotNull(_error is HttpException);
        }

        [Fact]
        public void EmptyPetsByOwnerGenderGroupsAreReturnedWhenThePeopleStoreReturnsAnEmptyPeopleList()
        {
            this.Given(s => s.AInstanceOfPersonService())
                .And(s => s.AnEmptyPeopleListIsReceivedFromThePeopleStore())
                .When(s => s.CatsByOwnerGenderIsRetreived())
                .Then(s => s.ThePetsByOwnerGenderGroupsAreEmpty())
                .BDDfy();
        }

        [Fact]
        public void HttpExceptionIsThrownWhenThePeopleStoreReturnsUnsuccessfulResponse()
        {
            this.Given(s => s.AInstanceOfPersonService())
                .And(s => s.AnUnsuccessfulResponseIsReceivedFromThePeopleStore())
                .When(s => s.CatsByOwnerGenderIsRetreived())
                .Then(s => s.ThePersonServiceThrewHttpException())
                .BDDfy();
        }

        [Fact]
        public void PetsByOwnerGenderGroupsAreReturnedWhenThePeopleStoreReturnsAValidPeopleList()
        {
            this.Given(s => s.AInstanceOfPersonService())
                .And(s => s.AValidResponseIsReceivedFromThePeopleStore())
                .When(s => s.CatsByOwnerGenderIsRetreived())
                .Then(s => s.ThePetsByOwnerGenderGroupsAreNotEmpty())
                .BDDfy();
        }

        [Fact]
        public void PetsByOwnerGenderGroupsAreReturnedWhenThePeopleStoreReturnsPeopleWithPeopleHasNoPets()
        {
            this.Given(s => s.AInstanceOfPersonService())
                .And(s => s.APeopleListWithPeopleHasNoPetsIsReceivedFromThePeopleStore())
                .When(s => s.CatsByOwnerGenderIsRetreived())
                .Then(s => s.TheGenderGroupsHasDistinctValues())
                .And(s => s.ThePetsAreOrderedByName())
                .BDDfy();
        }
    }
}