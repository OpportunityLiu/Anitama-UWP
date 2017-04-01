using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace AnitamaClient.Helpers
{
    public static class DocumentHelper
    {
        public static TextBlock CreateTextBlock(Inline inline)
        {
            var p = new TextBlock();
            p.Inlines.Add(inline);
            return p;
        }

        public static TextBlock CreateTextBlock(IEnumerable<Inline> inlines)
        {
            var p = new TextBlock();
            foreach(var item in inlines)
            {
                p.Inlines.Add(item);
            }
            return p;
        }

        public static Run CreateRun(string text)
        {
            return new Run { Text = text };
        }

        public static Bold CreateBold(string text)
        {
            var b = new Bold();
            b.Inlines.Add(new Run { Text = text });
            return b;
        }

        public static Italic CreateItalic(string text)
        {
            var i = new Italic();
            i.Inlines.Add(new Run { Text = text });
            return i;
        }

        public static Underline CreateUnderline(string text)
        {
            var u = new Underline();
            u.Inlines.Add(new Run { Text = text });
            return u;
        }

        public static Hyperlink CreateHyperlink(string text, string tooltip, Uri navigateUri)
        {
            var u = new Hyperlink();
            if(navigateUri != null)
                u.NavigateUri = navigateUri;
            if(tooltip != null)
                ToolTipService.SetToolTip(u, tooltip);
            if(text != null)
                u.Inlines.Add(new Run { Text = text });
            return u;
        }

        public static HyperlinkButton CreateHyperlinkButton(object content, string tooltip, Uri navigateUri)
        {
            var aBtn = new HyperlinkButton()
            {
                Content = content,
                Padding = new Thickness()
            };
            if(navigateUri != null)
                aBtn.NavigateUri = navigateUri;
            if(tooltip != null)
                ToolTipService.SetToolTip(aBtn, tooltip);
            return aBtn;
        }

        public static InlineUIContainer CreateInlineUIContainer(UIElement child)
        {
            return new InlineUIContainer { Child = child };
        }
    }
}
