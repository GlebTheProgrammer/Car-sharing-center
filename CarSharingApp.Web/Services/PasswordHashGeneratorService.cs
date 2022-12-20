namespace CarSharingApp.Services
{
    public static class PasswordHashGeneratorService
    {

        public static string GenerateNewPasswordHash(string password)
        {
            if (password == null)
                throw new Exception("Password can't be null");

            return GetPasswordHashRepresentation(password);
        }

        private static string GetPasswordHashRepresentation(string password, string salt = "")
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

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
