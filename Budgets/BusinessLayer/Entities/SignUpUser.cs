using System;

namespace Budgets.BusinessLayer.Entities
{
    public class SignUpUser
    {
        public string Email { get; set; }
        public string Login { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public SignInUser NewSignInUser()
        {
            return new SignInUser(Login, Email, Password);
        }

    }
}
