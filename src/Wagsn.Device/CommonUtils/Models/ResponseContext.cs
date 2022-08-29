using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{

        [Serializable]
        public class ResponseContext
       {
            /// <summary>
            /// 标准Code代码定义 
            /// 0: 正确 
            /// -1: 请求参数错误 
            /// -2: 签名校验错误 
            /// -3: 无API访问权限 
            /// -4: IP校验错误 
            /// -5: 访问超过限制 
            /// 注：标准错误码小于0，大于0的错误码由各接口根据接口语义自行定义
            /// 
            /// </summary>
            public string Code { get; set; }


            public string Message { get; set; }


            public object Data { get; set; }
        }

     




    
}
