using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Core.Extensions.Validations;
using System;

namespace CryptoInvestor.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Gender { get; protected set; }
        public long CreatedAt { get; protected set; }
        public long UpdatedAt { get; protected set; }

        protected User()
        {
        }

        public User(string email, string username, string password, string salt)
        {
            Id = Guid.NewGuid();
            SetUsername(username);
            SetEmail(email);
            SetPassword(password, salt);

            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void Update(string firstName, string lastName, string gender)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetGender(gender);
        }

        public void SetEmail(string email)
        {
            email = email.ToLowerInvariant();
            ValidateEmailOrFail(email);
            if (Email == email) return;

            Email = email;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetUsername(string username)
        {
            ValidateUsernameOrFail(username);
            if (Username == username) return;

            Username = username;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetPassword(string password, string salt)
        {
            ValidatePasswordOrFail(password, salt);
            if (Password == password) return;

            Password = password;
            Salt = salt;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetFirstName(string firstName)
        {
            ValidateFirstNameOrFail(firstName);
            if (FirstName == firstName) return;

            FirstName = firstName;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetLastName(string lastName)
        {
            ValidateLastNameOrFail(lastName);
            if (LastName == lastName) return;

            LastName = lastName;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetGender(string gender)
        {
            gender = gender.ToLowerInvariant();

            if (gender.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidGender,
                    "Gender can not be empty.");
            }

            Gender = gender;
            UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private void ValidateUsernameOrFail(string username)
        {
            if (username.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidUsername,
                    "User name can not be empty.");
            }
            if (username.Length < 3)
            {
                throw new DomainException(ErrorCodes.InvalidUsername,
                    "User name is too short.");
            }
            if (username.Length > 50)
            {
                throw new DomainException(ErrorCodes.InvalidUsername,
                    "User name is too long.");
            }
            if (username.HasInvalidCharacters())
            {
                throw new DomainException(ErrorCodes.InvalidUsername,
                    "User name can be consists of only letters and numbers.");
            }
        }

        private void ValidateEmailOrFail(string email)
        {
            if (email.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidEmail,
                    "Email can not be empty.");
            }
            if (!email.IsValidEmail())
            {
                throw new DomainException(ErrorCodes.InvalidEmail,
                    "Email address is incorrect.");
            }
        }

        private void ValidatePasswordOrFail(string password, string salt)
        {
            if (password.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password can not be empty.");
            }
            if (salt.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Salt can not be empty.");
            }
            if (password.Length < 4)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password must contain at least 4 characters.");
            }
            if (password.Length > 100)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password can not contain more than 100 characters.");
            }
        }

        private void ValidateFirstNameOrFail(string firstName)
        {
            if (firstName.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidFirstName,
                    "First name can not be empty.");
            }
            if (firstName.Length < 2)
            {
                throw new DomainException(ErrorCodes.InvalidFirstName,
                    "First name is too short.");
            }
            if (firstName.Length > 25)
            {
                throw new DomainException(ErrorCodes.InvalidFirstName,
                    "First name is too long.");
            }
            if (firstName.HasInvalidCharacters())
            {
                throw new DomainException(ErrorCodes.InvalidFirstName,
                    "First name is incorrect.");
            }
        }

        private void ValidateLastNameOrFail(string lastName)
        {
            if (lastName.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidLastName,
                    "Last name can not be empty.");
            }
            if (lastName.Length < 2)
            {
                throw new DomainException(ErrorCodes.InvalidLastName,
                    "Last name is too short.");
            }
            if (lastName.Length > 25)
            {
                throw new DomainException(ErrorCodes.InvalidLastName,
                    "Last name is too long.");
            }
            if (lastName.HasInvalidCharacters())
            {
                throw new DomainException(ErrorCodes.InvalidLastName,
                    "Last Name is incorrect.");
            }
        }
    }
}