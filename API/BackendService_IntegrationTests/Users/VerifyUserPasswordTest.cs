using System.Net;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils.Mocks;

namespace BackendService_IntegrationTests.Users;

public class VerifyUserPasswordTest
{
   private readonly CustomWebApplicationFactory _factory = new();
   
   [TearDown]
   public void TearDown()
   {
     _factory.ClearData();
   }

   [Test]
   public async Task validRequest_shouldReturnOk()
   {
      var user = UserMock.GenerateUser();
      _factory.SeedUserData(context =>
      {
         context.Users.Add(user);
         context.SaveChanges();
      });
      var client = _factory.CreateClient();
      var request = new
      {
         username = user.Username,
         password = UserMock.Password 
      };
      
      var response = await client.PostAsJsonAsync("/api/v1/users/validate-password", request);
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
   }
   
   [Test]
   public async Task invalidRequest_shouldReturnUnauthorized()
   {
      var user = UserMock.GenerateUser();
      _factory.SeedUserData(context =>
      {
         context.Users.Add(user);
         context.SaveChanges();
      });
      var client = _factory.CreateClient();
      var request = new
      {
         username = user.Username,
         password = "RandomPassword" 
      };
      
      var response = await client.PostAsJsonAsync("/api/v1/users/validate-password", request);
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
   }
   
   [Test]
   public async Task nonExistingUser_shouldReturnNotFound()
   {
      var client = _factory.CreateClient();
      var request = new
      {
         username = "RandomUser",
         password = "RandomPassword" 
      };
      
      var response = await client.PostAsJsonAsync("/api/v1/users/validate-password", request);
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
   }
}