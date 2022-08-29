using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Wagsn.Net.WebSockets
{
    public class WebsocketClientManager
    {
        private static WebsocketClientManager instance = null;
        private static object obj = new object();
        private List<WebsocketClient> clientList;

        public static WebsocketClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new WebsocketClientManager();
                        }
                    }
                }
                return instance;
            }
        }

        private WebsocketClientManager()
        {
            clientList = new List<WebsocketClient>();
        }

        public void Add(WebsocketClient client)
        {
            clientList.Add(client);
        }

        public void Remove(WebsocketClient client)
        {
            clientList.Remove(client);
        }

        public void Remove(string id)
        {
            clientList.RemoveAll(m => m.Id == id);
        }

        public WebsocketClient Get(string id)
        {
            return clientList.FirstOrDefault((t) => { return t.Id == id; });
        }

        public List<WebsocketClient> ClientList
        {
            get
            {
                return clientList;
            }
        }

    }
}
