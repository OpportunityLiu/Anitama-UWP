using MicroMsg.sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnitamaClient.Api.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    sealed partial class AuthPage : WXEntryBasePage
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.para = (Uri)e.Parameter;
            this.wv.Navigate(this.para);
        }

        private Uri para;

        private async void wv_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if(args.Uri.Host.Contains("anitama"))
            {
                this.bdProgress.Visibility = Visibility.Visible;
                args.Cancel = true;
                await GetAsync(args.Uri);
                Client.Current.SetLogOnData();
                await Client.Current.GetAsync<WechatAuthData>("member/specialGift");
                this.Frame.GoBack();
                this.bdProgress.Visibility = Visibility.Collapsed;
            }
        }

        public override async void OnSendAuthResponse(SendAuth.Resp response)
        {
            this.bdProgress.Visibility = Visibility.Visible;
            if(response.ErrCode != 0)
                throw new WXException(response.ErrCode, response.ErrStr);
            IEnumerable<KeyValuePair<string, string>> getParam()
            {
                yield return new KeyValuePair<string, string>("code", response.Code);
            }
            var data = await Client.Current.PostAsync<WechatAuthData>(new Uri("https://app.anitama.net/auth/wechat"), new HttpFormUrlEncodedContent(getParam()));
            Client.Current.SetLogOnData(data.UId, data.Token, data.ExpireAt);
            await Client.Current.GetAsync<WechatAuthData>("member/specialGift");
            this.Frame?.GoBack();
            this.bdProgress.Visibility = Visibility.Collapsed;
        }

        private static async Task GetAsync(Uri uri)
        {
            var res = await Client.Current.HttpClient.GetAsync(uri);
            while(res.Headers.Location != null)
            {
                res = await Client.Current.HttpClient.GetAsync(new Uri(res.RequestMessage.RequestUri, res.Headers.Location));
            }
        }
    }
}
