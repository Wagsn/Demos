using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.TankerGps
{
    /// <summary>
    /// 罐车GPS信息，包含正反转状态
    /// </summary>
    [BsonIgnoreExtraElements]
    [Serializable]
    public class TankerGpsInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// sim卡号（6位）
        /// </summary>
        public string simNumber { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// 高程
        /// </summary>
        public decimal height { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int speed { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public decimal direction { get; set; }


        /// <summary>
        /// 时间
        /// </summary>
        public string datetime { get; set; }

        /// <summary>
        /// 卸料开关（0：正转，1: 反转，2:停转）
        /// </summary>
        public int discharge { get; set; }

        [BsonIgnore]
        /// <summary>
        /// 车号
        /// </summary>
        public string cheNumber { get; set; }

    }
}


