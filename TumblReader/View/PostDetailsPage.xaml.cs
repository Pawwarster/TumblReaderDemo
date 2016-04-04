using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using TumblReader.Model;

namespace TumblReader.View
{
    public partial class PostDetailsPage : PhoneApplicationPage
    {
        public PostDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //---można by to zrobić o niebo lepiej, ale nie miałem zbytnio czasu na implementację lepszego rozwiązania, liczył się czas.
            ViewModel.ViewModelLocator.Main.DeterminePostType(ViewModel.ViewModelLocator.Main.SelectedPost);
            descContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.regularbody != null ? ViewModel.ViewModelLocator.Main.SelectedPost.regularbody : "");
            quoteDescContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.quotetext != null ? ViewModel.ViewModelLocator.Main.SelectedPost.quotetext : "");
            quoteSourceContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.quotesource != null ? ViewModel.ViewModelLocator.Main.SelectedPost.quotesource : "");
            photoCaptionContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.photocaption != null ? ViewModel.ViewModelLocator.Main.SelectedPost.photocaption : "");
            linkDescContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.linkdescription != null ? ViewModel.ViewModelLocator.Main.SelectedPost.linkdescription : "");
            videoDescContent.Content = ViewModel.ViewModelLocator.Main.ParseHtmlIntoControls(ViewModel.ViewModelLocator.Main.SelectedPost.videocaption != null ? ViewModel.ViewModelLocator.Main.SelectedPost.videocaption : "");
            if (!string.IsNullOrWhiteSpace(ViewModel.ViewModelLocator.Main.SelectedPost.audioembed))
                audioBrowser.NavigateToString(ViewModel.ViewModelLocator.Main.SelectedPost.audioembed);
            if (!string.IsNullOrWhiteSpace(ViewModel.ViewModelLocator.Main.SelectedPost.videoplayer250))
                videoBrowser.NavigateToString(ViewModel.ViewModelLocator.Main.SelectedPost.videoplayer250);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.ViewModelLocator.Main.PostBorderWidth = e.NewSize.Width - 48;
        }

        private void hyperButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.ViewModelLocator.Main.SelectedPost.linkurl) && ViewModel.ViewModelLocator.Main.SelectedPost.linkurl != "http://")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(ViewModel.ViewModelLocator.Main.SelectedPost.linkurl, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }

        private void hyperVidButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.ViewModelLocator.Main.SelectedPost.videosource) && ViewModel.ViewModelLocator.Main.SelectedPost.videosource != "http://")
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(ViewModel.ViewModelLocator.Main.SelectedPost.videosource, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }
    }
}