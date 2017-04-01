using AnitamaClient.Api;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnitamaClient.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ArticlePage : Page
    {
        public ArticlePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Article ar)
            {
                this.Article = ar;
            }
        }

        public Article Article
        {
            get => (Article)GetValue(ArticleProperty);
            set => SetValue(ArticleProperty, value);
        }

        public static readonly DependencyProperty ArticleProperty =
            DependencyProperty.Register(nameof(Article), typeof(Article), typeof(ArticlePage), new PropertyMetadata(null, ArticlePropertyChangedCallback));

        private static async void ArticlePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (ArticlePage)d;
            if(e.NewValue is Article a)
            {
                if(a.NeedPopulate)
                    await a.PopulateAsync();
            }
        }
    }
}
