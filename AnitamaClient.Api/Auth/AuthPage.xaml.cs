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
                this.Frame.GoBack();
                this.bdProgress.Visibility = Visibility.Collapsed;
            }
        }

        public override async void OnSendAuthResponse(SendAuth.Resp response)
        {
            this.bdProgress.Visibility = Visibility.Visible;
            //app.anitama.net/auth/web/wechat?code=0213r0141RB3XN1WeRZ31R9T0413r01V&state=debug:false,from:m,b5871053ad90a323
            if(response.ErrCode != 0)
                throw new WXException(response.ErrCode, response.ErrStr);
            var uri = new Uri($"https://app.anitama.net/auth/web/wechat?code={response.Code}&state={response.State}");
            await GetAsync(uri);
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
