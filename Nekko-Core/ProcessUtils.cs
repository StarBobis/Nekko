using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_Core
{
    public class ProcessUtils
    {

        public static int[] GetProcessIdsByName(string processName)
        {
            // 获取所有正在运行的进程
            Process[] processes = Process.GetProcessesByName(processName);

            // 提取每个进程的ID
            int[] processIds = new int[processes.Length];
            for (int i = 0; i < processes.Length; i++)
            {
                processIds[i] = processes[i].Id;
            }

            return processIds;
        }

    }
}
