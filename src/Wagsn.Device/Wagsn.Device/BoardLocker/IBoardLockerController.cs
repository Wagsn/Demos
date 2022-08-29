using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.BoardLocker
{
    /// <summary>
    /// 板控锁控制器
    /// </summary>
    public interface IBoardLockerController
    {
        /// <summary>
        /// 板切换锁数量，单个锁不用关心
        /// </summary>
        int ControlBoardSwitchCount { get; }

        /// <summary>
        /// 开锁
        /// </summary>
        /// <param name="lockerIndex">锁索引，1开始</param>
        /// <returns>true: 成功 false: 失败</returns>
        void Open(int lockerIndex);

        /// <summary>
        /// 关锁
        /// </summary>
        /// <param name="lockerIndex">锁索引，1开始</param>
        /// <param name="outMsg">说明信息</param>
        /// <returns>true: 成功 false: 失败</returns>
        void Close(int lockerIndex);

        /// <summary>
        /// 命令
        /// </summary>
        /// <param name="lockerIndex">锁索引：正整数</param>
        /// <param name="command">命令：1-开锁，0-关锁</param>
        void Command(int lockerIndex, int command);
    }
}
