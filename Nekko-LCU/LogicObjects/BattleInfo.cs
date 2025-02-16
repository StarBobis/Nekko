using Nekko_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_LCU
{
    public class BattleInfo
    {
        public string ChampionImage { get; set; } = ""; //英雄头像
        public string Win { get; set; } = ""; //游戏结果
        public string GameType { get; set; } = ""; //游戏类型
        public string StartTime { get; set; } = ""; //游戏开始时间
        public string CostTime { get; set; } = ""; //游戏花费时间

        public string ChampionId { get; set; } = ""; //英雄ID，用于获取该英雄头像

        public string KDAString { get; set; } = ""; //K/D/A
        public string KDANumber { get; set; } = ""; //KDA具体数值
        public string RealKD { get; set; } = ""; //性价比
        public string TeamKD { get; set; } = ""; //团队性
        public string Location { get; set; } = ""; //位置

        public string NiuMaTag { get; set; } = ""; //牛马标签
        public string KuaKuaTag { get; set; } = ""; //夸夸标签

        public BattleInfo()
        {

        }

        public BattleInfo(GameObject gameobj)
        {
            Participant participant = gameobj.Participants[0];
            ParticipantStats participantStats = participant.ParticipantStats;


            if (participantStats.Win)
            {
                this.Win = "胜利";
            }
            else
            {
                this.Win = "失败";
            }


            string ChampionIcon = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/" + participant.ChampionId.ToString() + ".png";
            this.ChampionImage = ChampionIcon;
            this.ChampionId = participant.ChampionId.ToString();
            //是否完成游戏
            //battleInfo.GameType = battleInfo.GameType + gameobj.EndOfGameResult;

            this.CostTime = StringUtils.ConvertSecondsToTime((int)gameobj.GameDuration);
            this.StartTime = gameobj.GameCreationDate.ToString();

            this.KDAString = participantStats.Kills.ToString() + "/" + participantStats.Deaths.ToString() + "/" + participantStats.Assists.ToString();

            double _kda = (double)(participantStats.Kills + participantStats.Assists) / participantStats.Deaths;
            double _realkda = (double)(participantStats.Kills) / participantStats.Deaths;
            double _teamkda = (double)(participantStats.Assists) / participantStats.Deaths;

            this.KDANumber = _kda.ToString("F1");
            this.RealKD = _realkda.ToString("F1");
            this.TeamKD = _teamkda.ToString("F1");

            if (participant.ParticipantTimeline.Role == "SUPPORT")
            {
                this.Location = "辅助";
            }
            else if (participant.ParticipantTimeline.Role == "SOLO")
            {
                this.Location = "上单";
            }
            else if (participant.ParticipantTimeline.Role == "NONE")
            {
                this.Location = "到处溜达";
            }
            else
            {
                this.Location = participant.ParticipantTimeline.Role;
            }

            if (_teamkda < 1.0)
            {
                this.NiuMaTag = this.NiuMaTag + " 对团队没用";
            }
            else
            {
                this.KuaKuaTag = this.KuaKuaTag + " 为团队奉献";
            }


            if (_realkda <= 0.5)
            {
                if (_teamkda < 1.0)
                {
                    this.NiuMaTag = this.NiuMaTag + " 一直送人头";
                }
                else
                {
                    this.KuaKuaTag = this.KuaKuaTag + " 为团队牺牲";
                }
            }
            else if (_realkda > 0.5 && _realkda <= 1)
            {
                if (_teamkda < 1.0)
                {
                    this.NiuMaTag = this.NiuMaTag + " 无脑换人头";
                }
                else
                {
                    this.KuaKuaTag = this.KuaKuaTag + " 舍身开团";
                }
            }
            else if (_realkda > 1 && _realkda < 2)
            {
                this.KuaKuaTag = this.KuaKuaTag + " 高性价比";
            }
            else
            {
                if (this.Location == "辅助")
                {
                    this.NiuMaTag = this.NiuMaTag + " 喜欢K头";
                }
                else
                {
                    this.KuaKuaTag = this.KuaKuaTag + " 不死魔王";
                }
            }








            //最后总结是什么局
            if (participantStats.GameEndedInSurrender)
            {
                if (participantStats.Win)
                {
                    this.GameType = this.GameType + " 碾压局";
                }
                else
                {
                    this.GameType = this.GameType + " 投降局";
                }
            }
            else if (participantStats.GameEndedInEarlySurrender)
            {
                this.GameType = this.GameType + " 未战先降";
            }
            else
            {
                if (participantStats.Win)
                {
                    if (this.NiuMaTag == "")
                    {
                        this.GameType = this.GameType + " 正常对局";
                    }
                    else
                    {
                        this.GameType = this.GameType + " 躺赢局";
                    }
                }
                else
                {
                    if (this.NiuMaTag == "")
                    {
                        this.GameType = this.GameType + " 甩锅局";
                    }
                    else
                    {
                        this.GameType = this.GameType + " 正常对局";
                    }
                }
            }
        }

    }
}
