namespace QuickServe.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }
    //Rehases the user's input and checks if it matches the hashed password in the database
    public static bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
    {
        var parts = hashedPasswordWithSalt.Split('.', 2);
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hashedPassword = parts[1];

        var hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: passwordToCheck,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashOfInput == hashedPassword;
    }
}