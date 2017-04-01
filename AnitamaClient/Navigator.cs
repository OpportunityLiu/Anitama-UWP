using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace AnitamaClient
{
    internal static class Navigator
    {
        private static SystemNavigationManager manager;

        private static Frame frame;

        public static Frame Frame
        {
            get => frame;
            set
            {
                if(value == frame)
                    return;
                if(frame != null)
                {
                    frame.Navigating -= Frame_Navigating;
                    frame.Navigated -= Frame_Navigated;
                    frame.NavigationFailed -= Frame_NavigationFailed;
                    frame.NavigationStopped -= Frame_NavigationStopped;
                }
                if(value != null)
                {
                    value.Navigating += Frame_Navigating;
                    value.Navigated += Frame_Navigated;
                    value.NavigationFailed += Frame_NavigationFailed;
                    value.NavigationStopped += Frame_NavigationStopped;
                }
                frame = value;
                if(frame == null)
                    return;
                if(manager != null)
                    manager.BackRequested -= Manager_BackRequested;
                manager = SystemNavigationManager.GetForCurrentView();
                if(manager != null)
                    manager.BackRequested += Manager_BackRequested;
                updateAppViewBackButtonVisibility();
            }
        }

        private static void Manager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(frame != null)
            {
                if(frame.CanGoBack)
                {
                    frame.GoBack();
                    e.Handled = true;
                }
                updateAppViewBackButtonVisibility();
            }
        }

        private static void updateAppViewBackButtonVisibility()
        {
            if(frame.CanGoBack)
                manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private static void Frame_NavigationStopped(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
        }

        private static void Frame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
        }

        private static void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            updateAppViewBackButtonVisibility();
        }

        private static void Frame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
}
