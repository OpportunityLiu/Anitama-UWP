using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AnitamaClient.Api;
using Newtonsoft.Json;
using MicroMsg.sdk;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AnitamaClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var f = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///test.json"));
            var s = await Windows.Storage.FileIO.ReadTextAsync(f);
            var c = JsonConvert.DeserializeObject<List<Bangumi>>(s,Client.jsonSettings);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = await Api.AuthClient.FetchAsync();
            a.WeiboAuth(this.Frame);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var a = await Api.AuthClient.FetchAsync();
            a.WeChatAuth(this.Frame);
        }
    }
}
