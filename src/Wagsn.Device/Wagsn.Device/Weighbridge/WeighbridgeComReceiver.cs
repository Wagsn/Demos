using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wagsn.Device.Weighbridge
{
    /// <summary>
    /// 地磅数据读取
    /// </summary>
    public class WeighbridgeComReceiver: DeviceComReceiver<decimal?>
    {
        private WeighbridgeSetting _setting;

        public WeighbridgeComReceiver(WeighbridgeSetting setting): base(setting)
        {
            _setting = setting;
        }

        private decimal? Decode(byte[] data)
        {
            if (data == null || data.Length == 0 || data.Length != _setting.PacketLength)
            {
                Console.WriteLine("数据帧长度不符合配置！");
                return null;
            }
            if ((!string.IsNullOrWhiteSpace(_setting.Prefix)) && NotEquals(data, 0, _setting.Prefix))
            {
                Console.WriteLine("数据帧前缀不符合配置！");
                return null;
            }
            if ((!string.IsNullOrWhiteSpace(_setting.Suffix)) && NotEquals(data, data.Length - _setting.Suffix.Length - 1, _setting.Suffix))
            {
                Console.WriteLine("数据帧后缀不符合配置！");
                return null;
            }
            var fixLength = _setting.Prefix.Length + _setting.Suffix.Length;
            var dest = new byte[data.Length - fixLength];
            Array.Copy(data, _setting.Prefix.Length, dest, 0, dest.Length);
            if (_setting.Reverse)
            {
                dest = data.Reverse().ToArray();
            }
            var weightStr = Encoding.ASCII.GetString(dest);
            weightStr = Regex.Replace(weightStr, @"\D", "");
            weightStr = Regex.Replace(weightStr, @"^(0+)", "");
            return decimal.Parse(weightStr) / _setting.TonnageConversionFactor;
        }

        public override decimal? ReceiveConvert(object frame)
        {
            var data = (byte[])frame;
            if (data == null || data.Length == 0 || data.Length != _setting.PacketLength)
            {
                Console.WriteLine("数据帧长度不符合配置！");
                return null;
            }
            if ((!string.IsNullOrWhiteSpace(_setting.Prefix)) && NotEquals(data, 0, _setting.Prefix))
            {
                Console.WriteLine("数据帧前缀不符合配置！");
                return null;
            }
            if ((!string.IsNullOrWhiteSpace(_setting.Suffix)) && NotEquals(data, data.Length - _setting.Suffix.Length - 1, _setting.Suffix))
            {
                Console.WriteLine("数据帧后缀不符合配置！");
                return null;
            }
            var fixLength = _setting.Prefix.Length + _setting.Suffix.Length;
            var dest = new byte[data.Length - fixLength];
            Array.Copy(data, _setting.Prefix.Length, dest, 0, dest.Length);
            if (_setting.Reverse)
            {
                dest = data.Reverse().ToArray();
            }
            var weightStr = Encoding.ASCII.GetString(dest);
            weightStr = Regex.Replace(weightStr, @"\D", "");
            weightStr = Regex.Replace(weightStr, @"^(0+)", "");
            return decimal.Parse(weightStr) / _setting.TonnageConversionFactor;
        }

        private bool NotEquals(byte[] buff, int startIdx, string str)
        {
            if (buff.Length - startIdx != str.Length) return true;

            for (int i = 0; i < str.Length; i++)
            {
                if (buff[startIdx + i] != str[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
