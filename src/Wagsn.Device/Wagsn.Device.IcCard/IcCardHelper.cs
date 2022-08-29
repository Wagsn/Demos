using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Device.IcCard
{
    /// <summary>
    /// IC卡读写助手
    /// </summary>
    public class IcCardHelper
    {
        private int icdev;
        private int st;
        private static Task RedCardTask;
        private readonly byte[] snr = new byte[5];

        private static IcCardHelper instance = null;
        private static CancellationTokenSource CTS = null;
        private static readonly object obj = new();

        public enum State : int
        {
            ConnectSuccessed = 1,
            LoadKeySuccessed,
            CardSelectSuccessed,
            AuthenSuccessed,
            ReadSuccessed,
            WriteSuccessed,

            WriteFailed = -6,
            ReadFailed,
            AuthenFailed,
            CardSelectFailed,
            LoadKeyFailed,
            ConnectFailed,
        }

        public static IcCardHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new IcCardHelper();
                        }
                    }
                }
                return instance;
            }
        }


        public IcCardHelper()
        {
            _ = Connectcard();
        }
        public OperateResult DoCard()
        {
            // rc = Connectcard();
            // if (rc.IsSuccess) rlt = readcard();

            OperateResult rlt = Readcard();
            return rlt;
        }

        public OperateResult DoCard(int str)
        {
            OperateResult rlt = new() { IsSuccess = false };
            try
            {
                rlt = Writecard(str);
            }
            catch (Exception) { }


            return rlt;
        }


        public OperateResult Connectcard()
        {

            OperateResult rlt = new() { IsSuccess = false };

            icdev = Mwrf32.rf_init(1, 115200);
            if (icdev > 0)
            {
                /*                byte[] status = new byte[30];
        st = mwrf32.rf_get_status(icdev, status);
        string _status = Encoding.UTF8.GetString(status);
        Console.WriteLine(_status);*/
                //System.Text.Encoding.ASCII.GetString(status);
                _ = Mwrf32.rf_beep(icdev, 3);

            }
            else
            {
                rlt.Message = GetMessage(State.ConnectFailed);
                return rlt;
            }

            byte[] key = { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            int mode = 0;
            int sector = 0;
            for (int i = 0; i < 16; i++)
            {
                st = Mwrf32.rf_load_key(icdev, mode, sector, key);
                if (st != 0)
                {
                    //string s1 = Convert.ToString(sector);
                    rlt.Message = GetMessage(State.LoadKeyFailed) + sector;
                    return rlt;
                }
                sector++;
            }
            rlt.Message = GetMessage(State.ConnectSuccessed);
            rlt.IsSuccess = true;
            return rlt;
        }
        public OperateResult Readcard()
        {
            OperateResult rlt = new() { IsSuccess = false };
            int sector = 1;

            st = Mwrf32.rf_card(icdev, 0, snr);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.CardSelectFailed);
                return rlt;
            }
            else
            {
                byte[] snr1 = new byte[8];
                Mwrf32.hex_a(snr, snr1, 4);
                Console.WriteLine(Encoding.ASCII.GetString(snr1));
            }
            st = Mwrf32.rf_authentication(icdev, 0, sector);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.AuthenFailed);
                return rlt;
            }

            byte[] databuffer = new byte[32];
            st = Mwrf32.rf_read(icdev, sector * 4 + 1, databuffer);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.ReadFailed);
                return rlt;
            }
            else
            {
                string str = Encoding.UTF8.GetString(databuffer);
                rlt.Model = str.Substring(0, str.IndexOf('\0'));
                rlt.IsSuccess = true;
                rlt.Message = GetMessage(State.ReadSuccessed);
            }

            Mwrf32.rf_halt(icdev);
            Mwrf32.rf_beep(icdev, 3);
            return rlt;
        }

        public OperateResult Writecard(int carid)
        {
            OperateResult rlt = new() { IsSuccess = false };
            int sector = 1;
            st = Mwrf32.rf_card(icdev, 0, snr);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.CardSelectFailed);
                return rlt;
            }
            else
            {
                byte[] snr1 = new byte[8];
                Mwrf32.hex_a(snr, snr1, 4);
            }
            st = Mwrf32.rf_authentication(icdev, 0, sector);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.AuthenFailed);
                return rlt;
            }

            byte[] data = new byte[16];
            data = Encoding.UTF8.GetBytes(carid.ToString());
            st = Mwrf32.rf_write(icdev, sector * 4 + 1, data);
            if (st != 0)
            {
                rlt.Message = GetMessage(State.WriteFailed);
                return rlt;
            }
            else
            {
                rlt.IsSuccess = true;
                rlt.Message = GetMessage(State.WriteSuccessed);
            }
            Mwrf32.rf_halt(icdev);
            Mwrf32.rf_beep(icdev, 3);

            return rlt;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {

                st = Mwrf32.rf_exit(icdev);
            }
        }

        public static void Dispose()
        {
            try
            {
                if (CTS is not null)
                {
                    if (CTS.Token.CanBeCanceled)
                    {
                        CTS.Cancel();
                    }
                    CTS.Dispose();
                }
            }
            catch (Exception e)
            {
            }

        }

        private static string GetMessage(State state)
        {
            string message = null;
            switch (state)
            {
                case State.ConnectSuccessed: message = "连接成功"; break;
                case State.LoadKeySuccessed: message = "加载密码成功"; break;
                case State.CardSelectSuccessed: message = "寻卡成功"; break;
                case State.AuthenSuccessed: message = "认证成功"; break;
                case State.ReadSuccessed: message = "读卡成功"; break;
                case State.WriteSuccessed: message = "写卡成功"; break;
                case State.ConnectFailed: message = "连接失败"; break;
                case State.LoadKeyFailed: message = "加载密码失败"; break;
                case State.CardSelectFailed: message = "寻卡失败"; break;
                case State.AuthenFailed: message = "认证失败"; break;
                case State.ReadFailed: message = "读卡失败"; break;
                case State.WriteFailed: message = "写卡失败"; break;
                default: break;
            }
            return message;
        }

        public bool ReadAllTime()
        {
            OperateResult rlt = new() { IsSuccess = false };
            try
            {
                if (RedCardTask is not null)
                {
                    if (RedCardTask.Status is TaskStatus.Running)
                    {
                        RedCardTask.Wait(1000);
                        Dispose();
                    }
                    if (RedCardTask.Status is TaskStatus.RanToCompletion)
                    {
                        RedCardTask.Dispose();
                    }
                    if (RedCardTask.Status is TaskStatus.Canceled)
                    {
                        RedCardTask.Dispose();
                    }
                }
            }
            catch (Exception E){ }

            try
            {  
                _ = Mwrf32.rf_beep(icdev, 3);
                CTS = new();

                RedCardTask = Task.Factory.StartNew(() =>
                {
                    do
                    {
                        Thread.Sleep(1000);
                        rlt = DoCard();
                        if (!rlt.IsSuccess)
                        {
                            continue;
                        }
                        // TODO 处理业务逻辑
                        Dispose();
                    }
                    while (!CTS.IsCancellationRequested);
                },
                CTS.Token);
            }
            catch (OperationCanceledException oce)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {oce.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

    }
}
