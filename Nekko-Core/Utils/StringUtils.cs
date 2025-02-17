using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_Core
{
    public class StringUtils
    {
        public static string ConvertSecondsToTime(int totalSeconds)
        {
            int hours = totalSeconds / 3600; // 计算总小时数
            int minutes = (totalSeconds % 3600) / 60; // 计算剩余分钟数
            int seconds = totalSeconds % 60; // 计算剩余秒数

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}"; // 使用D2格式化符确保两位数显示
        }
    }
}
