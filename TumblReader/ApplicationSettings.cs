using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblReader
{
    public static class ApplicationSettings
    {
        public static string ApiBaseUrl = "http://{1}.tumblr.com/api/read/json?";
        public static string itemLimit = "10";
        public static string ApiParams = "start={1}&num=" + itemLimit + "filter=none&debug=1";
    }
}
