using System.Net;
using BackendService.Common;
using BackendService.Common.Exceptions;
using BackendService.Services.Users.Domain;
using BackendService.Services.Users.Repository;
using BackendService.Services.Users.Utils;

namespace BackendService.Services.Users.UseCases;

public class CreateUserService(UserRepository repository)
{
   public sealed record Request(string Username, string Password);
   
   public async Task<string> CreateUser(Request request)
   {
      var user = CreateUserEntity(request);
      await SaveUser(user);
      return user.Id;
   }

   private static User CreateUserEntity(Request request)
   {
      var id = DatabaseUtil.GenerateId(User.IdPrefix);
      var user = new User
      {
         Id = id, 
         Username = request.Username,
         PasswordHash = "",
      };

      var hash = CryptUtil.HashPassword(user, request.Password); 
      user.PasswordHash = hash;
      return user;
   }
   
   private async Task SaveUser(User user)
   {
      var existingUser = repository.Users.FirstOrDefault(u => u.Username == user.Username);
      if (existingUser != null) throw new BadRequestException("User already exists", HttpStatusCode.Conflict);
      repository.Users.Add(user);
      await repository.SaveChangesAsync();
   }
}