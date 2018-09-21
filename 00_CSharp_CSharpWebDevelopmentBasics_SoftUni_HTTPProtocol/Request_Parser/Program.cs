using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebUtility;

namespace Request_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string NotFoundOutput = "HTTP/1.1 404 NotFound\n" +
                                    "Content-Length: 9\n" +
                                    "Content-Type: text/plain\n" +
                                    "\n" +
                                    "NotFound";

            string OKOutput = "HTTP/1.1 200 OK\n" +
                                    "Content-Length: 2\n" +
                                    "Content-Type: text/plain\n" +
                                    "\n" +
                                    "OK";

            Regex outputSplitter = new Regex(@"([a-zA-Z\/]+)\/([a-zA-Z\/]+)");
            Dictionary<string, List<string>> MethodPathDictionary = new Dictionary<string, List<string>>();
            string input = string.Empty;
            while ((input = Console.ReadLine()) != "END")
            {
                string path = outputSplitter.Match(input).Groups[1].Value;
                string method = outputSplitter.Match(input).Groups[2].Value;
                Console.WriteLine(path + " " + method);
                if (!MethodPathDictionary.ContainsKey(method))
                {

                    MethodPathDictionary.Add(method, new List<string>());

                }
                MethodPathDictionary[method].Add(path);
            }
            input = Console.ReadLine();
            string[] outputTokens = input.Split();
            if (MethodPathDictionary.ContainsKey(outputTokens[0].ToLower())
                && MethodPathDictionary[outputTokens[0].ToLower()].Contains(outputTokens[1])) { Console.WriteLine(OKOutput); }
            else Console.WriteLine(NotFoundOutput);
        }
    }
}
