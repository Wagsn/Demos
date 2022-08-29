using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Domain;
using TCZNGB.Core.Domain.Response;
using TCZNGB.Core.Utilities;
using TCZNGB.Core.ZNGB_Helper;
using TCZNGB.Service.TCP_SOCKET;

namespace TCZNGB.Service.ZNGB_ControlLock
{
    public class ZNGB_ControlLockService : IZNGB_ControlLockService
    {
        public ResponseContext ControlLock(ControlLockModel model)
        {
            var response = new ResponseContext();
            bool sendSuccess = false;
            string cmd = string.Empty;
            try
            {
                cmd = LockHelper.Getcommand(model.lockBoardID, model.lockID, model.lockCMD);
                byte[] sendmessage = new byte[cmd.Length];
                sendmessage = StrchangeHelper.strToHexByte(cmd);
                TCP_SOCKET_Client socket = new TCP_SOCKET_Client(model.lockIP, model.lockPort);
                sendSuccess = socket.Send(sendmessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            response.Code = sendSuccess ? ("0") : ("-1");
            response.Message = sendSuccess ? ("开锁成功") : ("开锁失败");
            return response;
        }
    }
}
