using System.Net.Http;
using System.Net.Http.Headers;
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
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            ////client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("92"));
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Connection","keep-alive");
            ////client.DefaultRequestHeaders.Add("Content-Length","92");
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36");
            ////client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate,sdch");
            //client.DefaultRequestHeaders.Add("Accept-Language","en-US,en;q=0.8");
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