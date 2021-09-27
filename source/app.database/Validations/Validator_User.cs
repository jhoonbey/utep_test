using app.domain.Model.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void UserCreateValidation(User model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            //role
            if (model.Role == 0) throw new Exception("Incorrect role");

            //Username
            Regex rxUsername = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(model.Username) || model.Username.Count() < 3 || model.Username.Count() > 50 || !rxUsername.IsMatch(model.Username))
                throw new Exception("Incorrect username. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //Password
            Regex rxPassword = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(model.Password) || model.Password.Count() < 3 || model.Password.Count() > 50 || !rxPassword.IsMatch(model.Password))
                throw new Exception("Incorrect password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");
        }
        public static void UserEditValidation(User model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            //role
            if (model.Role == 0) throw new Exception("Incorrect role");

            //Username
            Regex rxUsername = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(model.Username) || model.Username.Count() < 3 || model.Username.Count() > 50 || !rxUsername.IsMatch(model.Username))
                throw new Exception("Incorrect username. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");
        }
        public static void UserChangePasswordValidation(string oldPass, string newPass, string newPass2)
        {
            //oldPass
            Regex rxOld = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(oldPass) || oldPass.Count() < 3 || oldPass.Count() > 50 || !rxOld.IsMatch(oldPass))
                throw new Exception("Incorrect old password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //newPass
            Regex rxNew = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(newPass) || newPass.Count() < 3 || newPass.Count() > 50 || !rxNew.IsMatch(newPass))
                throw new Exception("Incorrect new password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //newPass2
            Regex rxNew2 = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(newPass2) || newPass2.Count() < 3 || newPass2.Count() > 50 || !rxNew2.IsMatch(newPass2))
                throw new Exception("Incorrect repeated new password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //matching...
            if (newPass.ToLower() != newPass2.ToLower()) throw new Exception("New passwords are not match");
        }
        public static void UserIdentificationValidation(string username, string password, int role)
        {
            //Username
            Regex rxUsername = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(username) || username.Count() < 3 || username.Count() > 50 || !rxUsername.IsMatch(username))
                throw new Exception("Incorrect username. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //Password
            Regex rxPassword = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(password) || password.Count() < 3 || password.Count() > 50 || !rxPassword.IsMatch(password))
                throw new Exception("Incorrect password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //role
            if (role < 1) throw new Exception("Incorrect role");
        }
        public static void UserLoginValidation(string username, string password)
        {
            //Username
            Regex rxUsername = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(username) || username.Count() < 3 || username.Count() > 50 || !rxUsername.IsMatch(username))
                throw new Exception("Incorrect username. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");

            //Password
            Regex rxPassword = new Regex(@"^[a-zA-Z0-9_?!-&]+$");
            if (string.IsNullOrEmpty(password) || password.Count() < 3 || password.Count() > 50 || !rxPassword.IsMatch(password))
                throw new Exception("Incorrect password. Use a-z, A-Z, 0-9 and _ ? ! - & symbols only. Minimum 3, maximum 50 length");
        }
    }
}
