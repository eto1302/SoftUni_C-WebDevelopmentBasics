using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> Cookies;

        private const string HttpCookieStringSeparator = "; ";

        public HttpCookieCollection()
        {
            this.Cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException();
            }
            Cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }
            return Cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            return Cookies[key];
        }

        public bool HasCookies()
        {
            return Cookies.Count != 0;
        }

        public override string ToString()
        {
            return string.Join(HttpCookieStringSeparator, this.Cookies.Values);
        }
    }
}
