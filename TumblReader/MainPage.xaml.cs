using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TumblReader.Resources;

namespace TumblReader
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void CheckFeedButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(identifierBox.Text))
            {
                ViewModel.ViewModelLocator.Main.TumblrIdentifier = identifierBox.Text;
                NavigationService.Navigate(new Uri("/View/UserPostsPage.xaml", UriKind.Relative));
            }
            else
                MessageBox.Show(TumblReader.Resources.AppResources.EnterIdentifierMonit, TumblReader.Resources.AppResources.ApplicationTitle, MessageBoxButton.OK);
        }
    }
}