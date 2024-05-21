using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.Models;

namespace PublicationsAPI.DTO.Mappers
{

    public static class UsersDTOMappers
    {

        //(FOR REQUESTS) Returns a NEW User of type Users based on information based from UserRequest DTO
        public static Users UserRequestToUsers(this UserRequest user)
        {
            return new Users{
                Name = user.Name,
                UserName = user.UserName,
                Bio = user.Bio,
                Email = user.Email,
                ImageUrl = user.ImageUrl
            };
        }

        //(FOR RESPONSE) Returns a NEW UserRequest DTO based on information based from User of type Users
        public static UserRequest UsersToUserRequest(this Users user)
        {
            return new UserRequest{
                Name = user.Name,
                UserName = user.UserName,
                Bio = user.Bio,
                Email = user.Email,
                ImageUrl = user.ImageUrl
            };
        }

        //(FOR RESPONSE) Returns a NEW LoggedOutUserResponse DTO based on information based from User of type Users
        public static LoggedOutUserResponse? UsersToLoggedOutUser(this Users user)
        {

            if(user == null)
                return null;

            return new LoggedOutUserResponse{
                Uuid = user.Uuid,
                Name = user.Name,
                UserName = user.UserName,
                Bio = user.Bio,
                ImageUrl = user.ImageUrl,
            };
        }

        //(FOR RESPONSE) Returns a NEW LoggedInUserResponse DTO based on information based from User of type Users
        public static LoggedInUserResponse? UsersToLoggedInUser(this Users user)
        {   
            if(user == null)
                return null;

            return new LoggedInUserResponse{
                Uuid = user.Uuid,
                Email = user.Email,
                Name = user.Name,
                UserName = user.UserName,
                Bio = user.Bio,
                ImageUrl = user.ImageUrl,
            };
        }
    }
}