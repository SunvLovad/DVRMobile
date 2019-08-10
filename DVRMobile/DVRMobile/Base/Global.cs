using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DVRMobile.Base
{
    public class Global
    {
        public const int MAX_SERVER_CONNECTING_SUPPORT = 1000;
        public const int MAX_SERVER_CONFIG_SUPPORT = 1000;

        public static double deviceHeight = 1280;
        public static double deviceWidth = 720;
        public static bool isHorizontalDevice = false;

        public static ENUM_DIVISIONS curDivision = ENUM_DIVISIONS.DIVISION_4;
        public static ENUM_DIVISIONS oldDivision = ENUM_DIVISIONS.NONE;
        public static Action<ENUM_DIVISIONS, int> OnChangeDivision = null;

        public static Action OnUnActiveChannel = null;
        public static Action<bool> OnUpdateStatusChannelControl = null;
        private static int _curChannelActive = -1;
        public static int curChannelActive
        {
            get { return _curChannelActive; }
            set
            {
                _curChannelActive = value;
                if(_curChannelActive == -1)
                {
                    if(OnUnActiveChannel != null)
                    {
                        OnUnActiveChannel();
                        OnUnActiveChannel = null;

                        if(OnUpdateStatusChannelControl != null)
                            OnUpdateStatusChannelControl(false);
                    }
                }
                else if (OnUpdateStatusChannelControl != null)
                {
                    OnUpdateStatusChannelControl(true);
                }
            }
        }

        public static bool isActiveSwapChannel = false;
        public static LibVLC libVlc = null;

        public static void ResetWhenLeaveLiveMode()
        {
            Global.OnChangeDivision = null;
            Global.curChannelActive = -1;
            isActiveSwapChannel = false;
        }

        /// <summary>
        /// send http post to server
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="values"></param>
        /// <param name="timeOut"></param>
        /// <returns>json data</returns>
        public static string SendHttpPostRequest(String requestUrl, Dictionary<string, string> values, int timeOut /*milliseconds*/, bool isEncryt = true)
        {
            try
            {
                string postData = string.Empty; //format key1=value1&key2=value2
                bool isTheFirst = true;
                foreach (var item in values)
                {
                    if (isTheFirst == true)
                    {
                        postData += string.Format("{0}={1}", item.Key, item.Value);
                        isTheFirst = false;
                    }
                    else
                    {
                        postData += string.Format("&{0}={1}", item.Key, item.Value);
                    }
                }

                if (isEncryt == true)
                    postData = CryptoAesAPI.Encrypt(postData);

                string ss = CryptoAesAPI.Decrypt(postData);

                byte[] data = Encoding.ASCII.GetBytes(postData);
                WebRequest request = WebRequest.Create(requestUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Timeout = timeOut;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ServerSiteConnectingManager serverSiteConnectingManager = new ServerSiteConnectingManager();


        public static void ConnectServerBeforeLoadSetting()
        {
            for (int i = 0; i < Base.Setting.Initances.maxServerId; i++)
            {
                var server = Base.Setting.Initances.FindServerById(i);
                if(server != null && server.AutoConnectWhenOpen)
                {
                    new System.Threading.Thread(() =>
                    {
                        serverSiteConnectingManager.ConnectServer(server.Id);
                    }).Start();
                }
            }
        }
    }
}
