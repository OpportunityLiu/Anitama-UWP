using MicroMsg.sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Controls;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;
using Windows.Web.Http;
using AnitamaClient.Api.Auth;

namespace AnitamaClient.Api
{
    public class AuthClient
    {
        private static readonly Uri uri = new Uri("https://app.anitama.net/auth/web");

        public static IAsyncOperation<AuthClient> FetchAsync()
        {
            return Client.Current.PostAsync<AuthClient>(uri, null);
        }

        public void WeiboAuth(Frame frame)
        {
            frame.Navigate(typeof(AuthPage), this.Weibo);
        }

        private static IEnumerable<KeyValuePair<string, string>> UrlToData(Uri uri)
        {
            var split = uri.Query.Split(new[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var item in split)
            {
                var s2 = item.Split(new[] { '=' }, 2);
                yield return new KeyValuePair<string, string>(s2[0], s2[1]);
            }
        }

        public async void WeChatAuth(Frame frame)
        {
            frame.Navigate(typeof(AuthPage), this.WeChat);
            currentFrame = new WeakReference<Frame>(frame);
            if(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                var req = new SendAuth.Req("snsapi_userinfo", "wechat");
                var api = WXAPIFactory.CreateWXAPI("wx2d00f7c30f52d4aa");
                var isValid = await api.SendReq(req);
            }
        }

        private static WeakReference<Frame> currentFrame;

        public static void HandleWeChatCallback(Windows.ApplicationModel.Activation.FileActivatedEventArgs e)
        {
            if(currentFrame.TryGetTarget(out var f) && f.Content is AuthPage p)
                p.Handle(e);
            else
                new AuthPage().Handle(e);
        }

        [JsonProperty("weibo")]
        public Uri Weibo { get; private set; }
        [JsonProperty("wechat")]
        public Uri WeChat { get; private set; }
    }
}
