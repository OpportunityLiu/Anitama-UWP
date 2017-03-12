using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace AnitamaClient.Api
{
    public class Client
    {
        public static Client Current { get; } = new Client();

        public static Uri MainUri { get; } = new Uri("https://m.anitama.cn/");
        public static Uri ApiUri { get; } = new Uri("https://app.anitama.net/");

        private Client()
        {
            var f = new HttpBaseProtocolFilter
            {
                AllowAutoRedirect = false
            };
            this.Cookies = f.CookieManager;
            this.HttpClient = new HttpClient(f);
            var headers = this.HttpClient.DefaultRequestHeaders;
            //Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 52.0.2743.116 Safari / 537.36 Edge / 15.15055
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("Mozilla", "5.0"));
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("Windows NT 10.0; Win64; x64"));
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("Edge", "15.15055"));
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("AppleWebKit", "537.36"));
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("Chrome", "52.0.2743.116"));
            headers.UserAgent.Add(new HttpProductInfoHeaderValue("Safari", "537.36"));
            //headers.UserAgent.Add(new HttpProductInfoHeaderValue("Anitama", "0.4.10"));
            headers.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("*/*"));
            headers.AcceptLanguage.Add(new HttpLanguageRangeWithQualityHeaderValue("zh-Hans-CN"));
            headers.Connection.Add(new HttpConnectionOptionHeaderValue("Keep-Alive"));
            headers["X-Agent"] = "application/0.4.10";


            //headers["X-User"] = "";
            //headers["X-Token"] = "";
        }

        internal HttpClient HttpClient { get; }

        internal HttpCookieManager Cookies { get; }

        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new JsonConverters.DateTimeConverter(),
                new JsonConverters.TimeSpanConverter(),
                new JsonConverters.ColorConverter()
            }
        };

        internal bool SetLogOnData(string uId, string token, DateTimeOffset expires)
        {
            var headers = this.HttpClient.DefaultRequestHeaders;
            headers["X-User"] = uId;
            headers["X-Token"] = token;
            return true;
        }

        internal bool SetLogOnData()
        {
            var token = this.Cookies.GetCookies(MainUri).SingleOrDefault(c => c.Name == "token");
            var uid = this.Cookies.GetCookies(ApiUri).SingleOrDefault(c => c.Name == "uid");
            if(token == null || uid == null)
                return false;
            var r1 = uid.Expires ?? DateTimeOffset.MaxValue;
            var r2 = token.Expires ?? DateTimeOffset.MaxValue;
            var r = default(DateTimeOffset);
            if(DateTimeOffset.Compare(r1, r2) < 0)
            {
                r = r1;
            }
            else
            {
                r = r2;
            }
            return SetLogOnData(uid.Value, token.Value, r);
        }

        public List<HttpCookie> Get(Uri uri)
        {
            return this.Cookies.GetCookies(uri).ToList();
        }

        private static void refineUri(ref Uri uri)
        {
            if(!uri.IsAbsoluteUri)
                uri = new Uri(ApiUri, uri);
        }

        public IAsyncOperation<TData> GetAsync<TData>(Uri uri, TData obj)
            where TData : class
        {
            refineUri(ref uri);
            return Run(async token =>
            {
                var resT = this.HttpClient.GetAsync(uri);
                token.Register(resT.Cancel);
                var resp = await resT;
                var str = await resp.Content.ReadAsStringAsync();
                var res = new Response<TData>(obj);
                JsonConvert.PopulateObject(str, res, jsonSettings);
                if(!res.Success)
                    throw AnitamaServerException.Create(res);
                return obj;
            });
        }

        public IAsyncOperation<TData> GetAsync<TData>(string uri, TData obj)
            where TData : class
        {
            return GetAsync(new Uri(uri, UriKind.RelativeOrAbsolute), obj);
        }

        public IAsyncOperation<TData> GetAsync<TData>(Uri uri)
        {
            refineUri(ref uri);
            return Run(async token =>
            {
                var resT = this.HttpClient.GetAsync(uri);
                token.Register(resT.Cancel);
                var resp = await resT;
                var str = await resp.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Response<TData>>(str, jsonSettings);
                if(!res.Success)
                    throw AnitamaServerException.Create(res);
                return res.Data;
            });
        }

        public IAsyncOperation<TData> GetAsync<TData>(string uri)
        {
            return GetAsync<TData>(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        public IAsyncOperation<TData> PostAsync<TData>(Uri uri, IHttpContent content, TData obj)
            where TData : class
        {
            refineUri(ref uri);
            return Run(async token =>
            {
                var resT = this.HttpClient.PostAsync(uri, content);
                token.Register(resT.Cancel);
                var resp = await resT;
                var str = await resp.Content.ReadAsStringAsync();
                var res = new Response<TData>(obj);
                JsonConvert.PopulateObject(str, res, jsonSettings);
                if(!res.Success)
                    throw AnitamaServerException.Create(res);
                return obj;
            });
        }

        public IAsyncOperation<TData> PostAsync<TData>(string uri, IHttpContent content, TData obj)
            where TData : class
        {
            return PostAsync(new Uri(uri, UriKind.RelativeOrAbsolute), content, obj);
        }

        public IAsyncOperation<TData> PostAsync<TData>(Uri uri, IHttpContent content)
        {
            refineUri(ref uri);
            return Run(async token =>
            {
                var resT = this.HttpClient.PostAsync(uri, content);
                token.Register(resT.Cancel);
                var resp = await resT;
                var str = await resp.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Response<TData>>(str, jsonSettings);
                if(!res.Success)
                    throw AnitamaServerException.Create(res);
                return res.Data;
            });
        }

        public IAsyncOperation<TData> PostAsync<TData>(string uri, IHttpContent content)
        {
            return PostAsync<TData>(new Uri(uri, UriKind.RelativeOrAbsolute), content);
        }
    }
}
