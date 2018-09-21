using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebUtility;

namespace Validate_URL
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = Console.ReadLine();
            string decodedURL = UrlDecode(url);
            Regex ValidURlRegex = new Regex(@"([a-z]+):\/\/([a-zA-Z.]+):?([0-9]*)(\/[a-zA-Z]*)\??([^#]*)#?([a-zA-Z]*)");
            if (ValidURlRegex.IsMatch(decodedURL))
            {
                GroupCollection groups = ValidURlRegex.Match(decodedURL).Groups;
                if (groups[1].Value == "http" && (groups[3].Value != "80" && groups[3].Value != "")) Console.WriteLine("Invalid URL");
                else if (groups[1].Value == "https" && (groups[3].Value != "443" && groups[3].Value != "")) Console.WriteLine("Invalid URL");
                else
                {
                    Console.WriteLine($"Protocol: {groups[1].Value}");
                    Console.WriteLine($"Host: {groups[2].Value}");
                    if (groups[1].Value == "http") Console.WriteLine($"Port: 80");
                    else if (groups[1].Value == "https") Console.WriteLine($"Port: 443");
                    else Console.WriteLine($"Port: {groups[3].Value}");
                    Console.WriteLine($"Path: {groups[4].Value}");
                    if (!string.IsNullOrEmpty(groups[5].Value)) Console.WriteLine($"Query: {groups[5].Value}");
                    if (!string.IsNullOrEmpty(groups[6].Value)) Console.WriteLine($"Fragment: {groups[6].Value}");
                }
            }
            else Console.WriteLine("Invalid URL");

        }
    }
}
