using System;
using System.Collections.Generic;
using System.Linq;

namespace NnumerialStringHandle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(XkjStringExtension.ToArabicNumericString("二十三亿零二百万三千零四"));
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class XkjStringExtension
    {
        // 中文数字字符串转阿拉伯数字字符串策略 以四级为进 万 亿 兆 京
        static readonly string[] hanzi = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        static readonly char[] hanziC = { '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        static readonly char[] w1 = { '千', '百', '十' };
        static readonly Dictionary<string, int> a = new Dictionary<string, int>();
        static readonly Dictionary<char, int> b = new Dictionary<char, int>();

        //Numeric string
        static readonly int[] shuzi = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        /// <summary>
        /// 最大为 long.MaxValue
        /// </summary>
        /// <param name="numericStr"></param>
        /// <returns></returns>
        public static string ToArabicNumericString(string numericStr)
        {
            for (int i = 0; i < 9; i++)
            {
                a.Add(hanzi[i], shuzi[i]);
                b.Add(hanziC[i], shuzi[i]);
            }
            string yw2;
            string ww2;
            string qw2;
            if (numericStr.IndexOf("亿") != -1)
            {
                String[] spy = numericStr.Split("亿");
                if (spy.Length == 1)
                {
                    yw2 = spy[0];
                    return ""+Yw(yw2);
                }
                else
                {
                    yw2 = spy[0];
                    String[] spwq = spy[1].Split("万");
                    if (spwq.Length == 1)
                    {
                        ww2 = spwq[0];
                        return ""+(Yw(yw2) + Ww(ww2));
                    }
                    else
                    {
                        yw2 = spy[0];
                        ww2 = spwq[0];
                        qw2 = spwq[1];
                        return ""+(Yw(yw2) + Ww(ww2) + Qw(qw2));
                    }
                }
            }
            else if (numericStr.IndexOf("万") != -1)
            {
                String[] spwq = numericStr.Split("万");
                if (spwq.Length == 1)
                {
                    ww2 = spwq[0];
                    return ""+(Ww(ww2));
                }
                else
                {
                    ww2 = spwq[0];
                    qw2 = spwq[1];
                    return ""+(Ww(ww2) + Qw(qw2));
                }
            }
            else
            {
                return ""+(Qw(numericStr));
            }
        }
        //计算9999位
        public static long Qw(String str)
        {
            return Gj(str);
        }
        //计算9999万位
        public static long Ww(String str)
        {
            return Gj(str) * 10000;
        }
        //计算24亿(int最大是24亿多)
        public static long Yw(String str)
        {
            return Gj(str) * 10000 * 10000;
        }
        //工具方法，用于计算4个位,把亿，万，进行拆分
        public static long Gj(String str)
        {
            int[] sum = { 0, 0, 0, 0 };
            int[] sum1 = { 0, 0, 0, 0 };
            for (int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt(i) == w1[0])
                {
                    sum[0] = b[str.ElementAt(i - 1)];
                }
                else if (str.ElementAt(i) == w1[1])
                {
                    sum[1] = b[str.ElementAt(i - 1)];
                }
                else if (str.ElementAt(i) == w1[2])
                {
                    if (str.ElementAt(0) == '十' || (str.ElementAt(0) == '零' && b.ContainsKey(str.ElementAt(1)))
                            || str.ElementAt(i - 1) == '零')
                    {
                        sum[2] = 1;
                    }
                    else
                    {
                        sum[2] = b[str.ElementAt(i - 1)];
                    }
                }
            }
            if (a.ContainsKey(str.Substring(str.Length - 1, 1)))
            {

                sum[3] = a[str.Substring(str.Length - 1, 1)];
            }
            else
            {
                sum[3] = 0;
            }
            if (sum[0] != 0)
            {
                sum1[0] = sum[0] * 1000;
            }
            if (sum[1] != 0)
            {
                sum1[1] = sum[1] * 100;
            }
            if (sum[2] != 0)
            {
                sum1[2] = sum[2] * 10;
            }
            if (sum[3] != 0)
            {
                sum1[3] = sum[3];
            }
            return sum1[0] + sum1[1] + sum1[2] + sum1[3];
        }
    }
}
