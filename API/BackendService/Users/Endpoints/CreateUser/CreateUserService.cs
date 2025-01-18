using DocumentService.Common;
using DocumentService.Users.Domain;
using DocumentService.Users.Repository;
using DocumentService.Users.Utils;

namespace DocumentService.Users.Endpoints.CreateUser;

public class CreateUserService(UserRepository repository)
{
   public async Task<string> CreateUser(CreateUserRequest request)
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
      
      repository.Users.Add(user);
      await repository.SaveChangesAsync();

      return id;
   } 
}