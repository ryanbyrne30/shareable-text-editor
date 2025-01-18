using System.Net;
using DocumentService.Common;
using DocumentService.Common.Exceptions;
using DocumentService.Users.Domain;
using DocumentService.Users.Repository;
using DocumentService.Users.Utils;

namespace DocumentService.Users.Endpoints.CreateUser;

public class CreateUserService(UserRepository repository)
{
   public async Task<string> CreateUser(CreateUserRequest request)
   {
      var user = CreateUserEntity(request);
      await SaveUser(user);
      return user.Id;
   }

   private static User CreateUserEntity(CreateUserRequest request)
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