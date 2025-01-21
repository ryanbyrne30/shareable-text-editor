using System.Net;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.SignInUser;

namespace BackendService_IntegrationTests.Gateway;

public class SignInUserTest
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

      var request = new SignInUserRequest
      {
         Username = user.Username,
         Password = UserMock.Password
      };
      
      var response = await client.PostAsJsonAsync(SignInUserController.Endpoint, request);
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

      var received = RequestUtils.ParseResponse<SignInUserResponse>(response);
      Assert.Multiple(() =>
      {
         Assert.That(received.AccessToken, Is.Not.Null);
         Assert.That(received.RefreshToken, Is.Not.Null);
      });
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
      
      var response = await client.PostAsJsonAsync(SignInUserController.Endpoint, request);
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
      
      var response = await client.PostAsJsonAsync(SignInUserController.Endpoint, request);
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
   }
}