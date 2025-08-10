using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace Presentation.Common.Services
{
    public static class KeyValidatorService
    {
        public static async Task<bool> ValidateKeyAsync(string endpointUrl, string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync($"{endpointUrl}?key={key}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao validar a key: {ex.Message}");
                return false;
            }
        }
    }
}
