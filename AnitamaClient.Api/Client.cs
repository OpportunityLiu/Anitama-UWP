using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace AnitamaClient.Api
{
    public class Client
    {
        public static Client Current { get; } = new Client();

        private Client()
        {
            var f = new HttpBaseProtocolFilter
            {
                AllowAutoRedirect = false
            };
            this.Cookies = f.CookieManager;
            this.HttpClient = new HttpClient(f);
        }

        internal HttpClient HttpClient { get; }

        internal HttpCookieManager Cookies { get; }

        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new JsonConverters.DateTimeConverter(),
                new JsonConverters.TimeSpanConverter(),
                new JsonConverters.ColorConverter()
            }
        };

        public List<HttpCookie> Get(Uri uri)
        {
            return this.Cookies.GetCookies(uri).ToList();
        }

        public IAsyncOperation<TData> GetAsync<TData>(Uri uri)
        {
            return Run(async token =>
            {
                var str = await this.HttpClient.GetStringAsync(uri);
                var res = JsonConvert.DeserializeObject<Response<TData>>(str, jsonSettings);
                if(!res.Success)
                    throw new Exception(res.Info)
                    {
                        Data =
                        {
                            ["Info"]=res.Info,
                            ["Status"]=res.Status
                        }
                    };
                return res.Data;
            });
        }

        public IAsyncOperation<TData> PostAsync<TData>(Uri uri, IHttpContent content)
        {
            return Run(async token =>
            {
                var resp = await this.HttpClient.PostAsync(uri, content);
                var str = await resp.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Response<TData>>(str, jsonSettings);
                if(!res.Success)
                    throw AnitamaServerException.Create(res);
                return res.Data;
            });
        }
    }
}
