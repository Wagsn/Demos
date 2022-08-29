using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.TankerGps
{
    /// <summary>
    /// GPS数据转换成地图经纬度
    /// </summary>
    public class GpsToAmap
    {
        static double a = 6378245.0;
        static double ee = 0.00669342162296594323;

        public static LatLng transformFromWGSToGCJ(LatLng wgLoc)
        {

            //如果在国外，则默认不进行转换
            if (outOfChina(wgLoc.Latitude, wgLoc.Longitude))
            {
                return new LatLng(wgLoc.Latitude, wgLoc.Longitude);
            }
            double dLat = transformLat(wgLoc.Longitude - 105.0,
                            wgLoc.Latitude - 35.0);
            double dLon = transformLon(wgLoc.Longitude - 105.0,
                            wgLoc.Latitude - 35.0);
            double radLat = wgLoc.Latitude / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);

            return new LatLng(wgLoc.Latitude + dLat, wgLoc.Longitude + dLon);
        }

        public static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                            + 0.2 * Math.Sqrt(x > 0 ? x : -x);
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x
                            * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0
                            * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y
                            * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                            * Math.Sqrt(x > 0 ? x : -x);
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x
                            * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0
                            * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x
                            / 30.0 * Math.PI)) * 2.0 / 3.0;
            return ret;
        }

        public static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }
    }

    public class LatLng
    {
        private double latitude;

        /// <summary>
        /// 维度
        /// </summary>
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public LatLng(double _latitude, double _longitude)
        {
            latitude = _latitude;
            longitude = _longitude;
        }
    }
}
