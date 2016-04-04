using GalaSoft.MvvmLight;
using Microsoft.Phone.Net.NetworkInformation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TumblReader.Helper;
using TumblReader.Model;

namespace TumblReader.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Variables
        private bool isActualization;
        private bool isActualizationMore;
        private bool isError;

        private bool textBorderVisible;
        private bool quoteBorderVisibile;
        private bool photoBorderVisible;
        private bool linkBorderVisibile;
        private bool chatBorderVisibile;
        private bool videoBorderVisibile;
        private bool audioBorderVisibile;

        protected CancellationTokenSource ctsDownload;
        public ICommand LoadMoreItemsCommand { get; set; }

        private double borderWidth;
        private double postBorderWidth;
        private TumblrDataModel tumblrData;
        private Post selectedPost;
        private string tumblrIdentifier;
        private int pageCounter;

        public bool IsActualization
        {
            get
            {
                return isActualization;
            }
            set
            {
                isActualization = value;
                RaisePropertyChanged(() => IsActualization);
            }
        }
        public bool IsActualizationMore
        {
            get
            {
                return isActualizationMore;
            }
            set
            {
                isActualizationMore = value;
                RaisePropertyChanged(() => IsActualizationMore);
            }
        }
        public bool IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                RaisePropertyChanged(() => IsError);
            }
        }

        public bool TextBorderVisible
        {
            get
            {
                return textBorderVisible;
            }
            set
            {
                textBorderVisible = value;
                RaisePropertyChanged(() => TextBorderVisible);
            }
        }
        public bool QuoteBorderVisible
        {
            get
            {
                return quoteBorderVisibile;
            }
            set
            {
                quoteBorderVisibile = value;
                RaisePropertyChanged(() => QuoteBorderVisible);
            }
        }
        public bool PhotoBorderVisible
        {
            get
            {
                return photoBorderVisible;
            }
            set
            {
                photoBorderVisible = value;
                RaisePropertyChanged(() => PhotoBorderVisible);
            }
        }
        public bool LinkBorderVisible
        {
            get
            {
                return linkBorderVisibile;
            }
            set
            {
                linkBorderVisibile = value;
                RaisePropertyChanged(() => LinkBorderVisible);
            }
        }
        public bool ChatBorderVisible
        {
            get
            {
                return chatBorderVisibile;
            }
            set
            {
                chatBorderVisibile = value;
                RaisePropertyChanged(() => ChatBorderVisible);
            }
        }
        public bool VideoBorderVisible
        {
            get
            {
                return videoBorderVisibile;
            }
            set
            {
                videoBorderVisibile = value;
                RaisePropertyChanged(() => VideoBorderVisible);
            }
        }
        public bool AudioBorderVisible
        {
            get
            {
                return audioBorderVisibile;
            }
            set
            {
                audioBorderVisibile = value;
                RaisePropertyChanged(() => AudioBorderVisible);
            }
        }

        public double BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
                RaisePropertyChanged(() => BorderWidth);
            }
        }
        public double PostBorderWidth
        {
            get
            {
                return postBorderWidth;
            }
            set
            {
                postBorderWidth = value;
                RaisePropertyChanged(() => PostBorderWidth);
            }
        }
        public TumblrDataModel TumblrData
        {
            get
            {
                return tumblrData;
            }
            set
            {
                tumblrData = value;
                RaisePropertyChanged(() => TumblrData);
            }
        }
        public Post SelectedPost
        {
            get
            {
                return selectedPost;
            }
            set
            {
                selectedPost = value;
                RaisePropertyChanged(() => SelectedPost);
            }
        }
        public string TumblrIdentifier
        {
            get
            {
                return tumblrIdentifier;
            }
            set
            {
                tumblrIdentifier = value;
                RaisePropertyChanged(() => TumblrIdentifier);
            }
        }       
        public int PageCounter
        {
            get
            {
                return pageCounter;
            }
            set
            {
                pageCounter = value;
                RaisePropertyChanged(() => PageCounter);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            LoadMoreItemsCommand = new DelegateCommand<object>(async k =>
            {
                await DownloadTumblrData(PageCounter);
            });
        }

        

        public bool IsConnection
        {
            get
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    return true;
                }
                return false;
            }
        }

        public void CancelTumblrDownloadIfNeed()
        {
            if (isActualization || isActualizationMore && ctsDownload != null)
            {
                try
                {
                    ctsDownload.Cancel();
                }
                catch
                {
                }
                IsActualization = false;
                IsActualizationMore = false;
            }
        }

        public async Task DownloadTumblrData(int pageCounter = 0)
        {
            try
            {
                IsActualization = false;
                IsActualizationMore = false;
                IsError = false;
                if (!IsConnection)
                {
                    IsError = true;
                    return;
                }
                if (pageCounter == 0)
                    IsActualization = true;
                else
                    IsActualizationMore = true;

                string apiAddress = ApplicationSettings.ApiBaseUrl.Replace("{1}", TumblrIdentifier) + ApplicationSettings.ApiParams.Replace("{1}", pageCounter.ToString());
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, new Uri(apiAddress, UriKind.Absolute));
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tempStr = await response.Content.ReadAsStringAsync();
                    TumblrDataModel tempData = JsonConvert.DeserializeObject<TumblrDataModel>(tempStr);
                    Deployment.Current.Dispatcher.BeginInvoke(() => 
                    {
                        if (pageCounter == 0)
                            TumblrData = tempData;
                        else
                            for (int i = 0; i < tempData.posts.Count; i++)
                                tumblrData.posts.Add(tempData.posts[i]);
                        PageCounter += tempData.posts.Count;
                        if (IsActualization)
                            IsActualization = false;
                        else
                            IsActualizationMore = false;
                    });
                }
                else
                {
                    ShowErrorOnUIThread();
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("Error while downloading tumblr posts: " + e.Message);
                ShowErrorOnUIThread();
            }
        }

        public StackPanel ParseHtmlIntoControls(string htmlToParse)
        {
            StackPanel stackPanel = new StackPanel();
            try
            {
                ParserTextModel parserTextModel = new ParserTextModel();
                List<UIElement> controls = parserTextModel.Parse("<div>" + htmlToParse + "</div>");
                stackPanel.Orientation = Orientation.Vertical;
                foreach (UIElement block in controls)
                    stackPanel.Children.Add(block);
            }
            catch (System.Exception e)
            {
            }
            return stackPanel;
        }

        public void DeterminePostType(Post tempPost)
        {
            switch (tempPost.type)
            {
                case "regular":
                    TextBorderVisible = true;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = false;
                    ChatBorderVisible = false;
                    VideoBorderVisible = false;
                    AudioBorderVisible = false;
                    break;
                case "quote":
                    TextBorderVisible = false;
                    QuoteBorderVisible = true;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = false;
                    ChatBorderVisible = false;
                    VideoBorderVisible = false;
                    AudioBorderVisible = false;
                    break;
                case "photo":
                    TextBorderVisible = false;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = true;
                    LinkBorderVisible = false;
                    ChatBorderVisible = false;
                    VideoBorderVisible = false;
                    AudioBorderVisible = false;
                    break;
                case "link":
                    TextBorderVisible = false;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = true;
                    ChatBorderVisible = false;
                    VideoBorderVisible = false;
                    AudioBorderVisible = false;
                    break;
                case "conversation":
                    TextBorderVisible = false;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = false;
                    ChatBorderVisible = true;
                    VideoBorderVisible = false;
                    AudioBorderVisible = false;
                    break;
                case "video":
                    TextBorderVisible = false;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = false;
                    ChatBorderVisible = false;
                    VideoBorderVisible = true;
                    AudioBorderVisible = false;
                    break;
                case "audio":
                    TextBorderVisible = false;
                    QuoteBorderVisible = false;
                    PhotoBorderVisible = false;
                    LinkBorderVisible = false;
                    ChatBorderVisible = false;
                    VideoBorderVisible = false;
                    AudioBorderVisible = true;
                    break;
                default:
                    break;
            }
        }


        private void ShowErrorOnUIThread()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (IsActualization)
                    IsActualization = false;
                else
                    IsActualizationMore = false;
                IsError = true;
            });
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}