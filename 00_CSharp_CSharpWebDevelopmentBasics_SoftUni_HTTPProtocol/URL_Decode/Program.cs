using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebUtility;

namespace HTTP_Protocol
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = Console.ReadLine();
            string decodedUrl = UrlDecode(url);
            Console.WriteLine(decodedUrl);
        }
    }
}
