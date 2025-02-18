using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_Core
{
    public static class TimerUtils
    {
        private static DateTime? run_start = null;
        private static DateTime? run_end = null;
        private static Dictionary<string, DateTime> methodNameRunStartDict = new Dictionary<string, DateTime>();

        public static void Start(string func_name)
        {
            // 清空run_start和run_end，并将run_start设为当前时间
            run_start = DateTime.Now;
            run_end = null;
            methodNameRunStartDict[func_name] = (DateTime)run_start;

            Debug.WriteLine("[" + func_name + $"] started at: {run_start} ");
        }

        public static void End(string func_name = "")
        {
            if (!run_start.HasValue)
            {
                Debug.WriteLine("Timer has not been started. Call Start() first.");
                return;
            }

            // 将run_end设为当前时间
            run_end = DateTime.Now;

            if (string.IsNullOrEmpty(func_name))
            {
                // 计算时间差
                TimeSpan time_diff = (DateTime)run_end - (DateTime)run_start;

                // 打印时间差
                Debug.WriteLine($"last function time elapsed: {time_diff} ");
            }
            else
            {
                DateTime start_time;
                if (methodNameRunStartDict.TryGetValue(func_name, out start_time))
                {
                    TimeSpan time_diff = (DateTime)run_end - start_time;

                    // 打印时间差
                    Debug.WriteLine("[" + func_name + $"] time elapsed: {time_diff} ");
                }
                else
                {
                    Debug.WriteLine($"No start time found for method: {func_name}");
                }
            }

            // 更新run_start为当前时间
            run_start = run_end;
        }
    }
}
