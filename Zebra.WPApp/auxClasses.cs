using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace Zebra.WPApp
{
    public class bindingCategory
    {
        public Category category { get; set; }
        public List<Place> lstPlaces { get; set; }
    }

    public class staticClasses
    { 
        public static Place selectedPlace{ get; set; }
        public static Category selectedCategory{ get; set; }
        public static Problem selectedEvent { get; set; }
    }

    public static class ShareContent
    {
        public static string message { get; set; }
        public static string title { get; set; }
        public static Uri link { get; set; }
    }
}
namespace Zebra.Utilities
{
    public static class Internet
    {
        public static Task<string> UploadStringAsyncUsingPUT(Uri uri, string data)
        {
            WebClient client = new SharpGIS.GZipWebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers["Accept"] = "*/*";
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            client.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
            client.Headers["Accept-Language"] = "en-US,en;q=0.8";
            var resultFromUpload = new TaskCompletionSource<string>();

            client.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                    resultFromUpload.SetException(e.Error);
                else
                    resultFromUpload.SetResult(e.Result);
            };

            client.UploadStringAsync(uri, "POST", data);

            return resultFromUpload.Task;
        }
    }
}