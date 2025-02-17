using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nekko_LCU
{
    public class KDAInfo
    {
        public ulong totalKill { get; set; } = 0;
        public ulong totalAssist { get; set; } = 0;
        public ulong totalDeath { get; set; } = 0;

        public double kda { get; set; } = 0;
        public double realkda { get; set; } = 0;
        public double teamkda { get; set; } = 0;

        public double AverageKill { get; set; } = 0;
        public double AverageAssistant { get; set; } = 0;
        public double AverageDeath { get; set; } = 0;

        public string MaName = "";

        public KDAInfo() { }

        public KDAInfo(List<GameObject> gameObjectList) {

            foreach (GameObject gameobj in gameObjectList)
            {

                Participant participant = gameobj.Participants[0];
                ParticipantStats participantStats = participant.ParticipantStats;

                totalKill += participantStats.Kills;
                totalAssist += participantStats.Assists;
                totalDeath += participantStats.Deaths;

            }

            kda = (double)(totalKill + totalAssist) / totalDeath;
            realkda = (double)(totalKill) / totalDeath;
            teamkda = (double)(totalAssist) / totalDeath;


            //判断是什么马
            if (kda <= 1.5)
            {
                MaName = "坑比";
            }
            else if (kda > 1.5 && kda <= 2)
            {
                MaName = "下等马";
            }
            else if (kda > 2 && kda <= 3)
            {
                MaName = "中等马";
            }
            else if (kda > 3 && kda <= 4)
            {
                MaName = "上等马";
            }
            else
            {
                MaName = "傲视群雄";
            }

             AverageKill = (double)(totalKill) / gameObjectList.Count;
             AverageAssistant = (double)(totalAssist) / gameObjectList.Count;
             AverageDeath = (double)(totalDeath) / gameObjectList.Count;

        }

    }
}
