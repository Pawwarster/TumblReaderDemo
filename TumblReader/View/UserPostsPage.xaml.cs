using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TumblReader.Model;

namespace TumblReader.View
{
    public partial class UserPostsPage : PhoneApplicationPage
    {
        private ScrollViewer scrollViewer;
        public ScrollViewer ScrollViewer
        {
            get
            {
                return scrollViewer;
            }
        }

        public UserPostsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.ViewModelLocator.Main.CancelTumblrDownloadIfNeed();
            await ViewModel.ViewModelLocator.Main.DownloadTumblrData();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = (ScrollViewer)sender;
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.ViewModelLocator.Main.BorderWidth = e.NewSize.Width - 100;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Post tempPost = btn.DataContext as Post;
            if (tempPost != null)
            {
                ViewModel.ViewModelLocator.Main.SelectedPost = tempPost;
                NavigationService.Navigate(new Uri("/View/PostDetailsPage.xaml", UriKind.Relative));
            }
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (scrollViewer != null)
                scrollViewer.ScrollToVerticalOffset(0.0d);
            ViewModel.ViewModelLocator.Main.CancelTumblrDownloadIfNeed();
            await ViewModel.ViewModelLocator.Main.DownloadTumblrData();
        }
    }
}