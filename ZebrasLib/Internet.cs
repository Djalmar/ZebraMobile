using System.Net.Http;
using System.Threading.Tasks;

namespace ZebrasLib
{
    public class Internet
    {
        private static HttpClient client;
        private static HttpResponseMessage result;
        private static HttpContent content;

        public static async Task<string> UploadStringAsync(string ReportProblemUri, string data)
        {
            client = new HttpClient();
            content = new StringContent(data);
            result = await client.PostAsync(ReportProblemUri, content);
            return await result.Content.ReadAsStringAsync();
        }

        public static async Task<string> DownloadStringAsync(string uriAddress)
        {
            client = new HttpClient();
            result = await client.GetAsync(uriAddress);
            return await result.Content.ReadAsStringAsync();
        }
    }
}