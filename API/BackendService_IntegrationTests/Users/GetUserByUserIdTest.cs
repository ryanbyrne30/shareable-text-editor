using System.Net;
using BackendService_IntegrationTests.Utils;
using BackendService_IntegrationTests.Utils.Mocks;
using DocumentService.Users.Endpoints.GetUserByUserId;

namespace BackendService_IntegrationTests.Users;

public class GetUserByUserIdTest
{
   private readonly CustomWebApplicationFactory _factory = new();
   
   [TearDown]
   public void TearDown()
   {
     _factory.ClearData();
   }
   
   [Test]
   public async Task validRequest_shouldReturnUser()
   {
      var user = UserMock.GenerateUser();
      _factory.SeedUserData(context =>
      {
         context.Users.Add(user);
         context.SaveChanges();
      });
      
      var client = _factory.CreateClient();

      var getResponse = await client.GetAsync($"/api/v1/users/{user.Id}");
      
      Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      
      var received = RequestUtils.ParseResponse<GetUserByUserIdResponse>(getResponse);
      Assert.That(received, Is.Not.Null);
      Assert.That(received.Username, Is.EqualTo(user.Username));
      TestUtils.AssertDatesAreEqual(user.CreatedAt, received.CreatedAt);
      TestUtils.AssertDatesAreEqual(user.UpdatedAt, received.UpdatedAt);
   }
   
   [Test]
   public async Task nonExistingUser_shouldReturnNotFound()
   {
      var client = _factory.CreateClient();
      var getResponse = await client.GetAsync($"/api/v1/users/doesnotexist");
      Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
   }
   
   [Test]
   public async Task idTooLong_shouldReturnBadRequest()
   {
      var client = _factory.CreateClient();
      var getResponse = await client.GetAsync($"/api/v1/users/0123456789012345678901234567890123456789");
      Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
   }
}