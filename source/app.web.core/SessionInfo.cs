//using JhoonHelper;
using System;
using System.DataManager;
using System.Web;

namespace app.web.core
{
    public class SessionInfo
    {
        public SessionInfo(BaseController controller, string cookieName)
        {
            this.Controller = controller;
            _cookieName = cookieName;
            LoadValues();
        }

        public BaseController Controller { get; set; }

        private HttpCookie _cookie;
        private string _cookieName;
        private HttpCookie Cookie
        {
            get
            {
                if (_cookie == null)
                {
                    _cookie = Controller.Request.Cookies.Get(_cookieName);
                    if (_cookie == null)
                    {
                        _cookie = new HttpCookie(_cookieName);
                        _cookie.Expires = DateTime.Now.AddMinutes(10);
                    }
                    Controller.Response.Cookies.Add(_cookie);
                }
                return _cookie;
            }
        }

        public bool IsAuthorized { get; set; }
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        private void LoadValues()
        {
            // is authorized
            var isAuthorizedValue = Cookie.Values.Get("IsAuthorized");
            if (!string.IsNullOrEmpty(isAuthorizedValue))
            {
                this.IsAuthorized = Convert.ToBoolean(isAuthorizedValue);
            }

            if (this.IsAuthorized)
            {
                this.Id = Convert.ToInt32(Cookie.Values.Get("Id"));
                string pass = Convert.ToString(Cookie.Values.Get("Password"));
                if (!string.IsNullOrEmpty(pass))
                {
                    this.Password = Reverse.D2(Convert.ToString(Cookie.Values.Get("Password")));
                }
                this.Username = Convert.ToString(Cookie.Values.Get("Username"));
                this.Fullname = Convert.ToString(Cookie.Values.Get("Fullname"));
                this.Role = Convert.ToInt32(Cookie.Values.Get("Role"));

                Controller.ViewBag.IsAuthorized = IsAuthorized;
                Controller.ViewBag.Username = Username;
                Controller.ViewBag.Password = Password;
                Controller.ViewBag.Id = Id;
                Controller.ViewBag.Fullname = Fullname;
                Controller.ViewBag.Role = Role;
            }
        }
        public void SaveValues()
        {
            Cookie.Values.Set("IsAuthorized", this.IsAuthorized.ToString());
            Cookie.Values.Set("Id", this.Id.ToString());
            Cookie.Values.Set("Username", this.Username.ToString());
            Cookie.Values.Set("Password", this.Password.ToString());
            Cookie.Values.Set("Fullname", this.Fullname);
            Cookie.Values.Set("Role", this.Role.ToString());

            Controller.Response.Cookies.Add(Cookie);
        }
        public void ChangePassword(string newPass)
        {
            this.Password = Reverse.E2(newPass);

            // password
            Cookie.Values.Set("Password", this.Password.ToString());

            // save cookies
            Controller.Response.Cookies.Add(Cookie);
        }
        public void ClearValues()
        {
            Cookie.Values.Set("IsAuthorized", null);
            Cookie.Values.Set("Id", null);
            Cookie.Values.Set("Username", null);
            Cookie.Values.Set("Password", null);
            Cookie.Values.Set("Fullname", null);
            Cookie.Values.Set("Login", null);
            Cookie.Values.Set("Role", null);

            // save cookies
            Controller.Response.Cookies.Add(Cookie);
        }
    }
}
