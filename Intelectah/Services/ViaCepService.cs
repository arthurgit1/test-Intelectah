using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Intelectah.Services
{
    public class ViaCepResponse
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Erro { get; set; }
    }

    public class ViaCepService
    {
        private readonly HttpClient _httpClient;
        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepResponse> BuscarEnderecoPorCepAsync(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ViaCepResponse>(json);
            return result;
        }
    }
}
