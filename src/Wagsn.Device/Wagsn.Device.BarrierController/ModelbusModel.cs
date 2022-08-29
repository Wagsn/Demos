using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Core.Domain.Response
{
    public class ModelbusModel
    {
        /// <summary>
        /// 从站地址
        /// </summary>
        public byte SlaveId { get; set; }
        /// <summary>
        /// 起始地址
        /// </summary>
        public ushort StartAddress { get; set; }

        /// <summary>
        /// 寄存器的数量
        /// </summary>
        public ushort NumInputs { get; set; }
        /// <summary>
        /// 写入值
        /// </summary>

        ushort[] InputValue;
        public bool SetInputValue(ushort[] value)
        {
            InputValue = new ushort[value.Length];
            value.CopyTo(InputValue, 0);
            return true;
        }
        public ushort[] GetInputValue()
        {
            return InputValue;
        }

        /// <summary>
        /// 写线圈
        /// </summary>
        bool[] CoilsBuffer;
        public bool SetInputCoils(bool[] Coils)
        {
            CoilsBuffer = new bool[Coils.Length];
            Coils.CopyTo(CoilsBuffer, 0);
            return true;
        }
        public bool[] GetInputCoils()
        {
            return CoilsBuffer;
        }
        /// <summary>
        /// 功能码
        /// </summary>
        public int FuncTionCode { get; set; }
    }
}
