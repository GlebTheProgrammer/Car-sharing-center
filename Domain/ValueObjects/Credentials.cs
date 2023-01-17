using CarSharingApp.Domain.Primitives;
using ErrorOr;
using CarSharingApp.Domain.ValidationErrors;
using System.Text.RegularExpressions;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Credentials : ValueObject
    {
        public const int MinLoginLength = 5;
        public const int MaxLoginLength = 15;
        public static readonly Regex EmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        public static readonly Regex StrongPasswordRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

        // Password Note
        //Has minimum 8 characters in length. Adjust it by modifying {8,}
        //At least one uppercase English letter. You can remove this condition by removing (?=.*?[A-Z])
        //At least one lowercase English letter.  You can remove this condition by removing (?=.*?[a-z])
        //At least one digit. You can remove this condition by removing (?=.*?[0-9])
        //At least one special character,  You can remove this condition by removing (?=.*?[#?!@$%^&*-])

        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        private Credentials(string login, string email, string password) 
        {
            Login = login;
            Email = email;
            Password = password;
        }

        public static ErrorOr<Credentials> Create(string login, string email, string password)
        {
            List<Error> errors = new();

            if (login.Length > MaxLoginLength || login.Length < MinLoginLength || login.Contains(' '))
            {
                errors.Add(DomainErrors.Customer.InvalidLogin);
            }
            if (!EmailRegex.IsMatch(email))
            {
                errors.Add(DomainErrors.Customer.InvalidEmail);
            }
            if (!StrongPasswordRegex.IsMatch(password))
            {
                errors.Add(DomainErrors.Customer.WeakPassword);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Credentials(login, email, GetPasswordHashRepresentation(password));
        }

        public static Credentials CreateForAuthorization(string login, string email, string password)
        {
            return new Credentials(login, email, GetPasswordHashRepresentation(password));
        }

        public static ErrorOr<Credentials> CreateForUpdate(string login, string email, string password)
        {
            List<Error> errors = new();

            if (login.Length is > MaxLoginLength or < MinLoginLength)
            {
                errors.Add(DomainErrors.Customer.InvalidLogin);
            }
            if (!EmailRegex.IsMatch(email))
            {
                errors.Add(DomainErrors.Customer.InvalidEmail);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Credentials(login, email, password);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Login;
            yield return Email; 
            yield return Password;
        }

        private static string GetPasswordHashRepresentation(string password, string salt = "")
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);

                byte[] hashBytes = sha.ComputeHash(passwordBytes);

                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hash;
            }
        }
    }
}
