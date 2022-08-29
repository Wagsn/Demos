using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCZNGB.Core.Domain;
using TCZNGB.Core.Domain.Response;
using static TCZNGB.Service.ZNGB_ControlRoadgate.ModbusTcp;

namespace TCZNGB.Service.ZNGB_ControlRoadgate
{
    public class ControlRoadgateService : IControlRoadgateService
    {
        ModelbusModel modelbusModel = new ModelbusModel();
        bool open = true;
        bool close = false;

        public ResponseContext ControlRoadgate(ControlRoadgateModel model)
        {
            var response = new ResponseContext();
            ModbusTcp modbus = new ModbusTcp(model.IP, int.Parse(model.Port));
         

            if (modbus.Start())
            {
                modelbusModel.SlaveId = byte.Parse(model.SlaveId);
                switch (model.Cmd) 
                {
                    case "open1":
                        modelbusModel.StartAddress = 0;
                        modelbusModel.FuncTionCode = 5;
                       // SetWriteParameters(open, 1);                    
                        Sendcmd2Roadgate(modbus,1);
                        break;

                    case "open2":
                        modelbusModel.StartAddress = 1;
                        modelbusModel.FuncTionCode = 5;
                        // SetWriteParameters(open, 1);
                        Sendcmd2Roadgate(modbus, 1);
                        break;

                    case "open3":
                        modelbusModel.StartAddress = 2;
                        modelbusModel.FuncTionCode = 5;
                        //SetWriteParameters(open, 1);
                        Sendcmd2Roadgate(modbus, 1);
                        break;

                    case "open4":
                        modelbusModel.StartAddress = 3;
                        modelbusModel.FuncTionCode = 5;
                        //SetWriteParameters(open, 1);
                        Sendcmd2Roadgate(modbus, 1);
                        break;

                    case "openall":
                        modelbusModel.StartAddress = 0;
                        modelbusModel.FuncTionCode = 7;
                        //SetWriteParameters(open, 4);
                        Sendcmd2Roadgate(modbus, 4);
                        break;

                }
                //modbus.ExecuteFunction(modelbusModel);
            }
            return response;
        }

        private void Sendcmd2Roadgate(ModbusTcp modbus,int num)
        {  
            SetWriteParameters(open, num);
            modbus.ExecuteFunction(modelbusModel);
            var t = Task.Run(async delegate
            {
                await Task.Delay(1000);//1秒后
                SetWriteParameters(close, num);
                modbus.ExecuteFunction(modelbusModel);
                return true;
            });  
        }

        private void SetWriteParameters(bool cmd,int num )
        {                     
            bool[] buffer = new bool[num];
            for (int i = 0; i < num; i++) 
            {
                buffer[i] = cmd;
            }         
            modelbusModel.SetInputCoils(buffer);
        }

    }
}
