using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        public string Id { get; }

        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public object GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            return this.sessionParameters.GetValueOrDefault(name, null);
        }

        public bool ContainsParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }
            return this.sessionParameters.ContainsKey(name);
        }

        public void AddParameter(string name, object parameter)
        {
            if (string.IsNullOrEmpty(name) || parameter == null)
            {
                throw new ArgumentNullException();
            }
            this.sessionParameters.Add(name, parameter);
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }
    }
}
