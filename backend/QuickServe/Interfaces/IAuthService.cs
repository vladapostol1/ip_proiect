namespace QuickServe.Interfaces;

using QuickServe.Contracts;
using QuickServe.Models;

public interface IAuthService
{
    Task<bool> RegisterUser(RegistrationContract registrationContract);
    Task<UserModel> AuthenticateUser(string username, string password);
}

