using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
	[TestFixture]
	public class HostTests
	{
		[Test]
		public async Task Host_is_running_and_accepts_http_calls()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "/");
			var (response, _) = await SUT.SendHttpRequest<object>(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}
	}
}
