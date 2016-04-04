using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblReader.Model
{
    public class TumblrDataModel
    {
        public Tumblelog tumblelog { get; set; }
        [JsonProperty("posts-start")]
        public int postsstart { get; set; }
        [JsonProperty("posts-total")]
        public int poststotal { get; set; }
        [JsonProperty("posts-type")]
        public bool poststype { get; set; }
        public ObservableCollection<Post> posts { get; set; }
    }


    public class Tumblelog
    {
        public string title { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string timezone { get; set; }
        public bool cname { get; set; }
        public ObservableCollection<object> feeds { get; set; }
    }

    public class Post
    {
        public string id { get; set; }
        public string url { get; set; }
        [JsonProperty("url-with-slug")]
        public string urlwithslug { get; set; }
        public string type { get; set; }
        [JsonProperty("date-gmt")]
        public string dategmt { get; set; }
        public string date { get; set; }
        public int bookmarklet { get; set; }
        public int mobile { get; set; }
        [JsonProperty("feed-item")]
        public string feeditem { get; set; }
        [JsonProperty("from-feed-id")]
        public int fromfeedid { get; set; }
        [JsonProperty("unix-timestamp")]
        public int unixtimestamp { get; set; }
        public string format { get; set; }
        [JsonProperty("reblog-key")]
        public string reblogkey { get; set; }
        public string slug { get; set; }
        [JsonProperty("quote-text")]
        public string quotetext { get; set; }
        [JsonProperty("quote-source")]
        public string quotesource { get; set; }
        public ObservableCollection<string> tags { get; set; }
        [JsonProperty("photo-caption")]
        public string photocaption { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        [JsonProperty("photo-url-1280")]
        public string photourl1280 { get; set; }
        [JsonProperty("photo-url-500")]
        public string photourl500 { get; set; }
        [JsonProperty("photo-url-400")]
        public string photourl400 { get; set; }
        [JsonProperty("photo-url-250")]
        public string photourl250 { get; set; }
        [JsonProperty("photo-url-100")]
        public string photourl100 { get; set; }
        [JsonProperty("photo-url-75")]
        public string photourl75 { get; set; }
        public ObservableCollection<string> photos { get; set; }
        [JsonProperty("link-text")]
        public string linktext { get; set; }
        [JsonProperty("link-url")]
        public string linkurl { get; set; }
        [JsonProperty("link-description")]
        public string linkdescription { get; set; }
        [JsonProperty("conversation-title")]
        public string conversationtitle { get; set; }
        [JsonProperty("conversation-text")]
        public string conversationtext { get; set; }
        public ObservableCollection<Conversation> conversation { get; set; }
        [JsonProperty("id3-artist")]
        public string id3artist { get; set; }
        [JsonProperty("id3-album")]
        public string id3album { get; set; }
        [JsonProperty("id3-year")]
        public string id3year { get; set; }
        [JsonProperty("id3-track")]
        public string id3track { get; set; }
        [JsonProperty("id3-title")]
        public string id3title { get; set; }
        [JsonProperty("audio-caption")]
        public string audiocaption { get; set; }
        [JsonProperty("audio-player")]
        public string audioplayer { get; set; }
        [JsonProperty("audio-embed")]
        public string audioembed { get; set; }
        [JsonProperty("audio-plays")]
        public int audioplays { get; set; }
        [JsonProperty("regular-title")]
        public string regulartitle { get; set; }
        [JsonProperty("regular-body")]
        public string regularbody { get; set; }
        [JsonProperty("video-source")]
        public string videosource { get; set; }
        [JsonProperty("video-caption")]
        public string videocaption { get; set; }
        [JsonProperty("video-player")]
        public string videoplayer { get; set; }
        [JsonProperty("video-player-500")]
        public string videoplayer500 { get; set; }
        [JsonProperty("video-player-250")]
        public string videoplayer250 { get; set; }
    }

    public class Conversation
    {
        public string name { get; set; }
        public string label { get; set; }
        public string phrase { get; set; }
    }

}
