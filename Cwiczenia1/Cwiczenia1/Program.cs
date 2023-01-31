using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0) throw new ArgumentNullException("args");
            Uri createdUri = null;
            bool isValid = Uri.TryCreate(args[0], UriKind.Absolute, out createdUri);
            if (!isValid) throw new ArgumentException("Nieprawidłowy url: ", args[0]);
            using HttpClient httpClient = new HttpClient();
            string content = "";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(createdUri);
                content = await response.Content.ReadAsStringAsync();
                response.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            Regex regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            MatchCollection matchCollection = regex.Matches(content);

            if (matchCollection.Count > 0)
            {
                HashSet<string> uniqueMatches = new HashSet<string>();
                foreach (object match in matchCollection)
                {
                    uniqueMatches.Add(Convert.ToString(match));
                }
                foreach (var match in uniqueMatches)
                {
                    Console.WriteLine(match);
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
        }
    }
}