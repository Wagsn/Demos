/*
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 *                                                                                 *
 * Copyright (c) 2020                                                              *
 *                                                                                 *
 * Author 16934                                                               *
 * Email   1693476459@qq.com                                                       *
 * Time 2020-03-13 13:13:23                                                                     *
 *                                                                                 *
 * Describe $Used to do something$                                                 *
 *                                                                                 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonUtils
{
    public static class HttpUtil
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T GetResponse<T>(string url) where T : class, new()
        {
            string returnValue = string.Empty;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webReq.Method = "GET";
            webReq.ContentType = "application/json";
      
            T result = default(T);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            returnValue = streamReader.ReadToEnd();

                            result = JsonConvert.DeserializeObject<T>(returnValue);

                        }
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求的Api地址</param>
        /// <param name="jsonParam">json参数</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string jsonParam) where T : class, new()
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
   
            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            T result = default(T);
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            
            return result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">后端的url</param>
        /// <param name="jsonParam">请求的参数</param>
        /// <returns></returns>
        public static ResponseContext Post_ZNGB(string url, string jsonParam) 
        {
            ResponseContext responseContext = new ResponseContext();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";

            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    responseContext = JsonConvert.DeserializeObject<ResponseContext>(responseString);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return responseContext;
        }

        public static ResponseContext Post_ZNGB_OpenRoadgate(string url, string jsonParam)
        {
            ResponseContext responseContext = new ResponseContext();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.ContentType = "application/json;charset=UTF-8";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    responseContext = JsonConvert.DeserializeObject<ResponseContext>(responseString);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return responseContext;
        }

        public static OperateResult Post_ZNGB_OpenRoadgate1(string url, string jsonParam)
        {
            OperateResult responseContext = new OperateResult();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.ContentType = "application/json;charset=UTF-8";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    responseContext = JsonConvert.DeserializeObject<OperateResult>(responseString);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return responseContext;
        }
    }
}
