using HtmlAgilityPack;
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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static AnitamaClient.Helpers.DocumentHelper;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AnitamaClient.Controls
{
    public sealed partial class HtmlViewer : UserControl
    {
        public HtmlViewer()
        {
            this.InitializeComponent();
        }

        public string HtmlContent
        {
            get => (string)GetValue(HtmlContentProperty);
            set => SetValue(HtmlContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for HtmlContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.Register("HtmlContent", typeof(string), typeof(HtmlViewer), new PropertyMetadata(null, HtmlContentPropertyChanged));

        public static void HtmlContentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if((string)e.OldValue == (string)e.NewValue)
                return;
            var s = (HtmlViewer)sender;
            s.reload();
        }

        private async void reload()
        {
            this.spPresenter.Children.Clear();
            if(this.HtmlContent == null)
                return;
            var htmlnode = HtmlNode.CreateNode(this.HtmlContent);
            if(htmlnode == null)
                return;
            if(htmlnode.ParentNode != null)
                htmlnode = htmlnode.ParentNode;
            await loadHtmlAsync(this.spPresenter, htmlnode);
        }

        private async Task loadHtmlAsync(StackPanel target, HtmlNode content)
        {
            foreach(var node in content.ChildNodes)
            {
                var tbNode = createNode(node);
                switch(tbNode)
                {
                case Inline i:
                    var bh = target.Children.LastOrDefault() as TextBlock;
                    if(bh == null)
                        bh = new TextBlock();
                    else
                    {

                    }
                    bh.Inlines.Add(i);
                    target.Children.Add(bh);
                    break;
                case UIElement u:
                    target.Children.Add(u);
                    break;
                default:
                    break;
                }
                await Task.Yield();
            }
        }

        private DependencyObject createNode(HtmlNode node)
        {
            if(node is HtmlTextNode)
            {
                if(node.InnerText == "\n")
                    return null;
                return CreateRun(HtmlEntity.DeEntitize(node.InnerText));
            }
            switch(node.Name)
            {
            case "br":
                return new LineBreak();
            case "a":
                var container = (Span)null;
                var target = (Uri)null;
                var title = HtmlEntity.DeEntitize(node.GetAttributeValue("title", default(string)));
                var name = HtmlEntity.DeEntitize(node.GetAttributeValue("name", default(string)));
                var tooltip = title ?? name;
                try
                {
                    target = new Uri(HtmlEntity.DeEntitize(node.GetAttributeValue("href", "")));
                    container = CreateHyperlink(null, tooltip, target);
                }
                catch(UriFormatException)
                {
                    container = new Span();
                }
                try
                {
                    foreach(var item in createChildNodes(node))
                    {
                        if(item is Inline l)
                            container.Inlines.Add(l);
                        else
                        {
                            container = null;
                            break;
                        }
                    }
                    if(container != null)
                        return container;
                }
                catch(ArgumentException)// has InlineUIContainer in childnodes
                {
                }
                var aBtnContent = new StackPanel();
                var ignore = loadHtmlAsync(aBtnContent, node);
                var aBtn = CreateHyperlinkButton(aBtnContent, tooltip, target);
                return aBtn;
            case "img":
                var image = new Image
                {
                    Source = new BitmapImage
                    {
                        UriSource = new Uri(node.GetAttributeValue("src", ""))
                    }
                };
                return image;
            case "video":
                try
                {
                    var pstr = HtmlEntity.DeEntitize(node.GetAttributeValue("poster", ""));
                    var poster = pstr != null ? new Uri(pstr) : null;
                    var sstr = HtmlEntity.DeEntitize(node.GetAttributeValue("src", ""));
                    var src = sstr != null ? new Uri(sstr) : null;
                    var video = new MediaPlayerElement();
                    if(poster != null)
                        video.PosterSource = new BitmapImage(poster);
                    if(src != null)
                        video.Source = Windows.Media.Core.MediaSource.CreateFromUri(src);
                    video.AreTransportControlsEnabled = true;
                    return video;
                }
                catch(UriFormatException)
                {
                    return null;
                }
            case "p":
                return CreateTextBlock(createChildNodes(node).Cast<Inline>());
            case "h1":
                var h1 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h1.FontSize = 36;
                return h1;
            case "h2":
                var h2 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h2.FontSize = 36;
                return h2;
            case "h3":
                var h3 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h3.FontSize = 36;
                return h3;
            case "h4":
                var h4 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h4.FontSize = 36;
                return h4;
            case "h5":
                var h5 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h5.FontSize = 36;
                return h5;
            case "h6":
                var h6 = CreateTextBlock(createChildNodes(node).Cast<Inline>());
                h6.FontSize = 36;
                return h6;
            case "hr":
                return CreateTextBlock(CreateRun("---------------"));
            default:
                return new Run
                {
                    Text = node.InnerHtml
                };
            }
        }

        private static readonly string eof = " ";

        private IEnumerable<DependencyObject> createChildNodes(HtmlNode node)
        {
            return node.ChildNodes.Select(n => createNode(n)).Where(n => n != null);
        }

        protected override void OnDisconnectVisualChildren()
        {
            ClearValue(HtmlContentProperty);
            base.OnDisconnectVisualChildren();
        }
    }
}
