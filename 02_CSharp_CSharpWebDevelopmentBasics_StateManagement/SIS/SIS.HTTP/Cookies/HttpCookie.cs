using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays)
        {
            if(string.IsNullOrEmpty(key)||string.IsNullOrEmpty(value)) throw new ArgumentNullException();

            this.Key = key;
            this.Value = value;
            this.isNew = true;
            this.Expires = DateTime.UtcNow.AddDays(expires);
        }
        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays) : this(key,value, expires)
        {
            this.isNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; private set; }

        public bool isNew { get; }

        public bool HttpOnly { get; set; } = true;

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            string result = $"{this.Key}={this.Value}; Expires={this.Expires.ToLongTimeString()}";
            if (this.HttpOnly)
            {
                result += "; HttpOnly";
            }

            return result;
        }
    }
}
