using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Utilities;

namespace TCZNGB.Core.ZNGB_Helper
{
    /// <summary>
    /// 仓位锁
    /// </summary>
    public class LockHelper
    {
        /// <summary>
        /// 16进制
        /// </summary>
        private static string head1 = "9B";
        private static string head2 = "9A";
        private static string open = "11";
        private static string close = "11";

        public static string Getcommand(int BoardID, int lockID, string cmd)
        {
            string command = string.Empty;
            string check = string.Empty;



            if (cmd == "open")
            {
                string mes = head1 + StrchangeHelper.dec2hex(BoardID) + StrchangeHelper.dec2hex(lockID) + open;
                check = StrchangeHelper.CRC(mes);
                command = mes + check;
            }
            else if (cmd == "close")
            {
                string mes = head2 + StrchangeHelper.dec2hex(BoardID) + StrchangeHelper.dec2hex(lockID) + close;
                check = StrchangeHelper.CRC(mes);
                command = mes + check;
            }
            return command;
        }
    }
}
