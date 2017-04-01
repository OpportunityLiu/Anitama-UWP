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

namespace AnitamaClient.Views
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.lv.ItemsSource = new Timeline();
            var o = JsonConvert.DeserializeObject<Bangumi>(@"{
            ""mid"": 28641,
            ""bid"": 249,
            ""cover"": ""http://img.animetamashi.cn/guide/e94c7e"",
            ""wantedList"": [
                ""23""
            ],
            ""verticalCover"": ""http://img.animetamashi.cn/bangumi/249/vertical"",
            ""playUrl"": ""http://bangumi.bilibili.com/anime/5809"",
            ""title"": ""黑白来看守所"",
            ""episodeList"": [
                ""23"",
                ""22"",
                ""21"",
                ""20"",
                ""19"",
                ""18"",
                ""17"",
                ""16"",
                ""15"",
                ""14"",
                ""13"",
                ""12"",
                ""11"",
                ""10"",
                ""9"",
                ""8"",
                ""7"",
                ""6"",
                ""5"",
                ""4"",
                ""3"",
                ""2"",
                ""1""
            ],
            ""seenList"": [
                """"
            ],
            ""watch"": true,
            ""episode"": ""第23集"",
            ""playDate"": ""20170308"",
            ""originWeekday"": ""每周三"",
            ""playWeekday"": ""每周三"",
            ""playSite"": ""bilibili"",
            ""originStation"": ""d-anime store"",
            ""playTime"": ""11:00"",
            ""originTime"": ""11:00""
        }", Client.jsonSettings);
        }

        private void lv_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch(((FeedItem)e.ClickedItem).Item)
            {
            case Article article:
                this.Frame.Navigate(typeof(ArticlePage), article);
                break;
            default:
                break;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var c = await AuthClient.FetchAsync();
            c.WeChatAuth(this.Frame);
        }
    }
}
