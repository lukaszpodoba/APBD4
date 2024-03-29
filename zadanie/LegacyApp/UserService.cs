using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsNameCorrect(firstName) || IsSurnameCorrect(lastName))
            {
                return false;
            }

            if (IsEmailAddressIncorrect(email))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = CreatingNewUser(firstName, lastName, email, dateOfBirth, client);

            SettingCreditLimit(client, user);

            if (UserHasSufficientCredit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        public static User CreatingNewUser(string firstName, string lastName, string email, DateTime dateOfBirth,
            Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return user;
        }

        private static bool UserHasSufficientCredit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        public static void SettingCreditLimit(Client client, User user)
        {
            if (IsVeryImportantClient(client))
            {
                SettingCreditLimitOfVeryImportantClient(user);
            }
            else if (IsImportantClient(client))
            {
                SettingCreditLimitOfImportantClient(user);
            }
            else
            {
                SettingCreditLimitOfNormalClient(user);
            }
        }

        public static void SettingCreditLimitOfVeryImportantClient(User user)
        {
            user.HasCreditLimit = false;
        }

        public static void SettingCreditLimitOfImportantClient(User user)
        {
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
        }

        public static void SettingCreditLimitOfNormalClient(User user)
        {
            user.HasCreditLimit = true;
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
        }
        

        public static bool IsImportantClient(Client client)
        {
            return client.Type == "ImportantClient";
        }

        public static bool IsVeryImportantClient(Client client)
        {
            return client.Type == "VeryImportantClient";
        }

        private static bool IsEmailAddressIncorrect(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private static bool IsSurnameCorrect(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }

        private static bool IsNameCorrect(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }
    }
}
