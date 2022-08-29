using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Wagsn.Device.BoardLocker.Boshuo
{
    /// <summary>
    /// 博硕粉仓门禁
    /// </summary>
    public class BoshuoHttpLockerController : IBoardLockerController
    {
        public int ControlBoardSwitchCount { get; }

        public void Close(int lockerIndex)
        {
            Command(lockerIndex, 0);
        }

        public void Open(int lockerIndex)
        {
            Command(lockerIndex, 1);
        }

        private string BaseUrl { get; set; }

        private HttpClient client { get; set; }

        public BoshuoHttpLockerController(string baseUrl = "http://127.0.0.1:8096/soap/IMJKZService", int switchCount = 16)
        {
            ControlBoardSwitchCount = switchCount;
            BaseUrl = baseUrl;

            client = new HttpClient();
        }

        public void Command(int lockerIndex, int command)
        {
            InMsg request = new InMsg();
            request.StatCode = "生产线" + (lockerIndex / ControlBoardSwitchCount + 1); // NOTE 生产线是博硕配置好的 一般不需要动
            request.Channel = lockerIndex % ControlBoardSwitchCount;
            request.Command = command;

            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri(BaseUrl);
            httpRequest.Method = HttpMethod.Post;
            httpRequest.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpResult = client.SendAsync(httpRequest).Result;
            var httpContent = httpResult.Content.ReadAsStringAsync().Result;
            var commandReult = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResult>(httpContent);
            if (commandReult == null || commandReult.Result != 0)
            {
                throw new Exception($"博硕电子门禁控制命令响应失败，响应码({commandReult?.Result})，信息({commandReult?.Message})");
            }
        }
    }
}
