using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace DependencyElimination
{
    internal class HttpNotSuccessfullException: Exception
    {
        internal HttpNotSuccessfullException(string s) : base(s)
        {
        }
    }

    internal class Program
    {
        private static IEnumerable<string> GetAvailableAddresses()
        {
            for (var page = 1; page <= 5; page++)
                yield return "http://habrahabr.ru/top/page" + page;
        }

        private static string HttpCrawl(string url)
        {
            string result;
            using (var http = new HttpClient())
            {
                var habrResponse = http.GetAsync(url).Result;

                if (!habrResponse.IsSuccessStatusCode)
                {
                    throw new HttpNotSuccessfullException(habrResponse.StatusCode + " " + habrResponse.ReasonPhrase);
                }

                result = habrResponse.Content.ReadAsStringAsync().Result;
            }

            return result;
        }
      
        private static IEnumerable<string> GetHrefs(string input)
		{
			var matches = Regex.Matches(input, @"\Whref=[""'](.*?)[""'\s>]").Cast<Match>();
            return matches.Select(match => match.Groups[1].Value).ToList();
		}

        private static void WriteToFile(IEnumerable<string> input, TextWriter output)
        {
            using (output)
            {
                foreach (var item in input)
                {
                    output.WriteLine(item);
                }
            }
        }

        private static void Main()
        {
            var hrefs = new List<string>();
            var totalCount = 0;

            var sw = Stopwatch.StartNew();

            foreach (var input in GetAvailableAddresses())
            {
                Console.WriteLine(input);
                try
                {
                    var contents = HttpCrawl(input);
                    var foundHrefs = GetHrefs(contents).ToList();
                    Console.WriteLine("found {0} links", foundHrefs.Count());
                    hrefs.AddRange(foundHrefs);
                    totalCount += foundHrefs.Count();
                }
                catch (HttpNotSuccessfullException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Total links found: {0}", totalCount);
            WriteToFile(hrefs, new StreamWriter("links.txt", false));
            Console.WriteLine("Finished");
            Console.WriteLine(sw.Elapsed);
        }
	}
}