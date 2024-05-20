using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.Models;

namespace PublicationsAPI.DTO.Mappers
{

    public static class UserMappers
    {

        //(FOR REQUESTS) Returns a NEW User of type Users based on information based from UserRequest DTO
        public static Users UserRequestToUsers(this UserRequest user)
        {
            return new Users{
                Name = user.Name,
                Username = user.Username,
                Bio = user.Bio,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                PasswordHash = user.PasswordHash
            };
        }

        //(FOR RESPONSE) Returns a NEW UserRequest DTO based on information based from User of type Users
        public static UserRequest UsersToUserRequest(this Users user)
        {
            return new UserRequest{
                Name = user.Name,
                Username = user.Username,
                Bio = user.Bio,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                PasswordHash = user.PasswordHash
            };
        }

        //(FOR RESPONSE) Returns a NEW LoggedOutUserResponse DTO based on information based from User of type Users
        public static LoggedOutUserResponse? UserToLoggedOutUser(this Users user)
        {

            if(user == null)
                return null;

            return new LoggedOutUserResponse{
                Uuid = user.Uuid,
                Name = user.Name,
                Username = user.Username,
                Bio = user.Bio,
                ImageUrl = user.ImageUrl,
            };
        }

        //(FOR RESPONSE) Returns a NEW LoggedInUserResponse DTO based on information based from User of type Users
        public static LoggedInUserResponse? UserToLoggedInUser(this Users user)
        {   
            if(user == null)
                return null;

            return new LoggedInUserResponse{
                Uuid = user.Uuid,
                Name = user.Name,
                Username = user.Username,
                Bio = user.Bio,
                ImageUrl = user.ImageUrl,
            };
        }
    }

}