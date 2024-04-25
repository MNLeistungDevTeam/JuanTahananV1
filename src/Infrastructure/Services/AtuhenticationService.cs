using AutoMapper;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.Authentication;
using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shyjus.BrowserDetection;
using System.Security.Cryptography;
using System.Text;

namespace DMS.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IBrowserDetector _browserDetector;
    private readonly IMapper _mapper;
    private readonly IOptions<AuthenticationConfig> _authenticationConfig;
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(
        IUserRepository userRepository,
        IOptions<AuthenticationConfig> authenticationConfig,
        IMapper mapper,
        IUserActivityRepository userActivityRepository,
        IBrowserDetector browserDetector)
    {
        _userRepository = userRepository;
        _authenticationConfig = authenticationConfig;
        _mapper = mapper;
        _userActivityRepository = userActivityRepository;
        _browserDetector = browserDetector;
    }

    public async Task<User> RegisterUser(UserModel user)
    {
        // Check if the user already exists
        var existingUser = await _userRepository.GetByUserNameAsync(user.UserName);
        if (existingUser != null)
        {
            throw new Exception("User with the same username already exists");
        }

        if (!string.IsNullOrEmpty(user.PagibigNumber))
        {
            if (user.PagibigNumber.Length < 12)
            {
                throw new Exception("Pagibig Number minimum length must 12");
            }

            var existingPagibigNumber = await _userRepository.GetByPagibigNumberAsync(user.PagibigNumber);

            if (existingPagibigNumber != null)
            {
                throw new Exception("User with the same Pag-Ibig Number already exists");
            }
        }

        await _userRepository.ValidateEmailAsync(user);

        var userRepo = _mapper.Map<User>(user);

        // Create a new user entity
        userRepo.PasswordSalt = _authenticationConfig.Value.PasswordSalt;
        userRepo.Password = HashPassword(userRepo.Password, userRepo.PasswordSalt);// Hash the password before storing it

        // Save the user in the repository
        await _userRepository.CreateAsync(userRepo, 0);

        return userRepo;
    }

    public async Task<User> ResetCredential(UserModel user)
    {
        var userRepo = _mapper.Map<User>(user);

        // Create a new user entity
        userRepo.PasswordSalt = _authenticationConfig.Value.PasswordSalt;
        userRepo.Password = HashPassword(userRepo.Password, userRepo.PasswordSalt);// Hash the password before storing it

        // Save the user in the repository
        await _userRepository.UpdateNoExclusionAsync(userRepo, 0);

        return userRepo;
    }

    public async Task<User> Authenticate(AuthRequest authRequest)
    {
        // Find the user by the provided username
        var user = await _userRepository.GetByUserNameAsync(authRequest.UserName)
            ?? throw new Exception("Username not found.");

        await UserLockedStatus(user.UserName);

        // Verify the password by comparing the hashed password
        if (!VerifyPassword(authRequest.Password, user.Password))
        {
            await UpdateFailedAttempts(user.UserName);
            await InsertUserActivity(user.Id, "Failed Login");
            throw new Exception("Invalid Username/Password");
        }

        await InsertUserActivity(user.Id, "Successful Login");
        return user;
    }

    public string HashPassword(string password, string salt)
    {
        byte[] saltByte = Encoding.ASCII.GetBytes(salt);
        string hashedPassword = GenerateHashedPassword(password, saltByte);
        return hashedPassword;
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] salt = Encoding.ASCII.GetBytes(_authenticationConfig.Value.PasswordSalt);
        string hashedInput = GenerateHashedPassword(password, salt);
        return hashedInput == hashedPassword;
    }

    private static string GenerateHashedPassword(string password, byte[] salt)
    {
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashedPassword;
    }

    public async Task UpdateUserRefreshToken(int userId, string refreshToken, DateTime refreshTokenExpiry)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new Exception("User not found!");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiry;

        await _userRepository.UpdateAsync(user, userId);
    }

    public async Task UpdateOnlineStatus(int id, bool status)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return;
        }

        user.IsOnline = status;
        user.LastOnlineTime = DateTime.Now;

        await _userRepository.UpdateAsync(user, id);
    }

    public async Task<UserModel> UpdateFailedAttempts(string userName)
    {
        var userModel = new UserModel
        {
            Id = 0,
            Message = "User not found"
        };

        var user = await _userRepository.GetByUserNameAsync(userName);

        if (user is null)
        {
            return userModel;
        }

        user.FailedAttempts++;

        if (user.LastFailedAttempt is not null)
        {
            var totalMinutes = Convert.ToInt32((DateTime.UtcNow - user.LastFailedAttempt.GetValueOrDefault()).TotalMinutes);
            if (totalMinutes > 5)
            {
                user.FailedAttempts = 1;
            }
        }

        user.LastFailedAttempt = DateTime.UtcNow;

        if (user.FailedAttempts == 3)
        {
            user.LockedTime = DateTime.UtcNow;
        }

        user = await _userRepository.UpdateAsync(user, user.Id);

        userModel.Id = user.Id;
        userModel.UserName = user.UserName;

        userModel.Message = user.FailedAttempts < 3
           ? $"LOG IN FAILED: attempt {user.FailedAttempts}/3"
           : "Account is LOCKED due to 3 consecutive failed Log In attempts. Your account will automatically unlock after 30 minutes or contact your System Administrator.";

        return userModel;
    }

    public async Task UnlockUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return;
        }

        user.LockedTime = null;
        user.FailedAttempts = 0;

        await _userRepository.UpdateAsync(user, id);
    }

    public async Task UserLockedStatus(string userName)
    {
        try
        {
            var user = await _userRepository.GetByUserNameAsync(userName);

            if (user is null)
            {
                return;
            }
            if (user.LockedTime is null)
            {
                return;
            }

            var totalMinutes = Convert.ToInt32((DateTime.Now - user.LockedTime.GetValueOrDefault()).TotalMinutes);

            if (totalMinutes < 30)
            {
                throw new Exception("Account is locked due to 3 consecutive failed login attempts, your account will automatically unlock after " + (30 - totalMinutes) + " minutes");
            }
            else
            {
                await UnlockUser(user.Id);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task InsertUserActivity(int userId, string action)
    {
        //Save Activity
        UserActivity userActivity = new()
        {
            Date = DateTime.Now,
            UserId = userId,
            Browser = _browserDetector.Browser?.Name + " " + _browserDetector.Browser?.Version,
            Action = action,
            Device = _browserDetector.Browser?.DeviceType + " (" + _browserDetector.Browser?.OS + ")",
            ActivityTypeId = 1
        };

        await _userActivityRepository.CreateAsync(userActivity);
    }

    private async Task<bool> UsernameExistsAsync(string username)
    {
        return (await _userRepository.GetAllAsync()).Any(x => x.UserName == username);
    }

    public string GenerateTemporaryPasswordAsync(string name)
    {
        name = name.Replace(" ", "");

        // Generate a GUID with 16 characters
        string guid = Guid.NewGuid().ToString("N").Substring(0, 16);

        // Concatenate GUID with name
        string combinedString = guid + name;

        // Use a random seed based on the current time
        Random rand = new Random();

        // Hash the combined string using SHA-256
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(combinedString);
            byte[] hash = sha256.ComputeHash(bytes);

            // Introduce additional randomness
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= (byte)rand.Next(256);
            }

            // Convert the byte array to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            // Ensure a fixed length of 14 characters for the output password
            string hashedString = sb.ToString();
            string outputPassword = name + hashedString.Substring(0, 10);

            return outputPassword;
        }
    }

    public string GenerateRandomPassword()
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";
        Random rand = new Random();

        // Generate random password length between 10 and 12 characters
        int passwordLength = rand.Next(10, 13); // Returns a value between 10 and 12 (exclusive)

        // Hash the current time to introduce some randomness
        string timeStamp = DateTime.Now.Ticks.ToString();

        // Concatenate the GUID with the current time
        string combinedString = Guid.NewGuid().ToString("N").Substring(0, 16) + timeStamp;

        // Use SHA-256 to hash the combined string
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(combinedString);
            byte[] hash = sha256.ComputeHash(bytes);

            // Convert the byte array to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            // Ensure the password is within the desired length range
            string hashedString = sb.ToString();
            if (hashedString.Length < passwordLength)
            {
                // If the hashed string is shorter than the desired length, pad it with additional characters
                while (hashedString.Length < passwordLength)
                {
                    hashedString += allowedChars[rand.Next(allowedChars.Length)];
                }
            }
            else if (hashedString.Length > passwordLength)
            {
                // If the hashed string is longer than the desired length, truncate it
                hashedString = hashedString.Substring(0, passwordLength);
            }

            return hashedString;
        }
    }

    public async Task<string> GenerateTemporaryUsernameAsync()
    {
        string temporaryUsername;
        int index = 1;

        do
        {
            temporaryUsername = $"Beneficiary-{index++}";
        } while (await UsernameExistsAsync(temporaryUsername));

        return temporaryUsername;
    }

    public async Task<bool> ChangePassword(ChangePasswordModel changePassword)
    {
        string username = changePassword.Username;
        string currentPassword = changePassword.CurrentPassword;
        string newPassword = changePassword.NewPassword;
        int userId = changePassword.UserId;

        // Find the user by username
        var existingUser = await _userRepository.GetByUserNameAsync(username)
            ?? throw new Exception("User not found");

        // Authenticate the user's current password
        if (!IsPasswordValid(currentPassword, existingUser.Password, existingUser.PasswordSalt))
        {
            throw new Exception("Current password is incorrect");
        }

        // Hash the new password before storing it
        string newPasswordHash = HashPassword(newPassword, existingUser.PasswordSalt);

        // Update the user's password
        existingUser.Password = newPasswordHash;

        // Save the user with the new password in the repository
        await _userRepository.UpdateNoExclusionAsync(existingUser, userId);

        return true; // Password successfully changed
    }

    private bool IsPasswordValid(string currentPassword, string storedPasswordHash, string salt)
    {
        try
        {
            // Hash the current password with the same salt and compare it to the stored password hash
            string currentPasswordHash = HashPassword(currentPassword, salt);
            return currentPasswordHash == storedPasswordHash;
        }
        catch (Exception)
        {
            throw;
        }
    }
}