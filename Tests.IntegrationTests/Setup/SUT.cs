using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests.IntegrationTests
{
	public static class SUT
	{
		internal static async Task<(HttpResponseMessage message, T content)> SendHttpRequest<T>(HttpRequestMessage request, object data = null) where T : class
		{
			T content = null;
			if (data != null)
			{
				var json = JsonSerializer.Serialize(data, SerializerOptions);
				request.Content = new StringContent(json, Encoding.UTF8, "application/json");
			}

			var response = await OneTimeTestServerSetup.Client.SendAsync(request);

			var contentString = await response.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(contentString))
			{
				var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contentString));
				content = await JsonSerializer.DeserializeAsync<T>(contentStream, SerializerOptions);
			}

			return (response, content);
		}

		internal static async Task<(HttpResponseMessage message, string content)> SendHttpRequest(HttpRequestMessage request, object data = null)
		{
			if (data != null)
			{
				var json = JsonSerializer.Serialize(data, SerializerOptions);
				request.Content = new StringContent(json, Encoding.UTF8, "application/json");
			}

			var response = await OneTimeTestServerSetup.Client.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			return (response, content);
		}

		private static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};
	}
}
