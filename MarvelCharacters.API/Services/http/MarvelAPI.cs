using MarvelCharacters.Api.Services.Http.Marvel.Models;
using MarvelCharacters.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.API.Services.Http
{
    public class MarvelApi
    {

        private readonly HttpClient _client;

        public MarvelApi(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://gateway.marvel.com/");
        }

        #region Authorization
        string GetAuthorizationString(string privateKey, string publicKey, int ts = 0)
        {
            var hash = ComputeHash($"{ts}{privateKey}{publicKey}");
            return $"ts={ts}&apikey={publicKey}&hash={hash}";
        }

        static string ComputeHash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new StringBuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
        #endregion

        public async Task<IReadOnlyCollection<Character>> GetCharacters(string searchString)
        {
            string authorizationQuery = GetAuthorizationString("d01b50da5c701311ed036ae0a885c94f13e843d9", "809cd7c6b557803e35b5503b819a306f", 1);

            string uri = $"/v1/public/characters?limit=10&{authorizationQuery}";

            if (!string.IsNullOrEmpty(searchString))
                uri += $"&nameStartsWith={searchString}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var responseStream = await _client.SendAsync(request);

                    if (responseStream.IsSuccessStatusCode)
                    {
                        var responseText = await responseStream.Content.ReadAsStringAsync();
                        //introspecção 
                        var response = JsonConvert.DeserializeObject<ServiceResult<Character>>(responseText);

                        return response.Data.Results;
                    }
                    return Array.Empty<Character>();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
