using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
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
        public string Location { get; set; } = ""; //位置

        // 根据 Location 返回颜色
        public Brush LocationColor
        {
            get
            {
                // 根据 Location 的值动态设置颜色
                switch (Location)
                {
                    case "到处溜达":
                        return new SolidColorBrush(Colors.Red); // 红色
                    default:
                        return new SolidColorBrush(Colors.Green); // 默认黑色
                }
            }
        }
        public string Win { get; set; } = ""; //游戏结果
        public Brush WinColor
        {
            get
            {
                // 根据 Location 的值动态设置颜色
                switch (Win)
                {
                    case "失败":
                        return new SolidColorBrush(Colors.Red); // 红色
                    default:
                        return new SolidColorBrush(Colors.Green); // 默认黑色
                }
            }
        }


        public string ChampionImage { get; set; } = ""; //英雄头像
        public string GameType { get; set; } = ""; //游戏类型
        public string StartTime { get; set; } = ""; //游戏开始时间
        public string CostTime { get; set; } = ""; //游戏花费时间

        public string ChampionId { get; set; } = ""; //英雄ID，用于获取该英雄头像

        public string KDAString { get; set; } = ""; //K/D/A
        public string KDANumber { get; set; } = ""; //KDA具体数值
        public string RealKD { get; set; } = ""; //性价比
        public string TeamKD { get; set; } = ""; //团队性

        public string NiuMaTag { get; set; } = ""; //牛马标签
        public string KuaKuaTag { get; set; } = ""; //夸夸标签

        public GameObject gameObject { get; set; }

        public List<string> KuaKuaTagList { get; set; }= new List<string>();
        public List<string> NiuMaTagList { get; set; }= new List<string>();

        public BattleInfo()
        {

        }

        public BattleInfo(GameObject gameobj)
        {
            this.gameObject = gameobj;

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
            else if (participant.ParticipantTimeline.Role == "CARRY")
            {
                this.Location = "ADC";
            }
            else if (participant.ParticipantTimeline.Role == "DUO")
            {
                this.Location = "中单";
            }
            else if (participant.ParticipantTimeline.Role == "NONE")
            {
                this.Location = "到处溜达";
            }
            else
            {
                this.Location = participant.ParticipantTimeline.Role;
            }


            //如果团队KDA小于1.0说明对团队没用
            if (_teamkda < 1.0)
            {
                if (_realkda <= _teamkda)
                {
                    //如果团队KDA小于1.0 真实KDA还小于团队kDA的话，那说明一点用都没有
                    NiuMaTagList.Add("对团队没用");
                }
                else if (_realkda >= 2)
                {
                    KuaKuaTagList.Add("Carry全场");
                }
            }
            else
            {
                KuaKuaTagList.Add("为团队奉献");

            }


            if (_realkda <= 0.5)
            {
                if (_teamkda < 1.0)
                {
                    NiuMaTagList.Add("一直送人头");
                }
                else
                {
                    KuaKuaTagList.Add("为团队牺牲");
                }
            }
            else if (_realkda > 0.5 && _realkda <= 1.4)
            {
                if (_teamkda < 1.0)
                {
                    NiuMaTagList.Add("无脑换人头");
                }
                else
                {
                    KuaKuaTagList.Add("舍身开团");
                }
            }
            else if (_realkda > 1.4 && _realkda < 2)
            {
                KuaKuaTagList.Add("高性价比");
            }
            else
            {
                KuaKuaTagList.Add("不死魔王");
            }

            //类型
            //this.GameType = this.GameType + gameobj.GameMode + " " + gameobj.GameType 


            //排位类型
            if (gameobj.QueueId == 420)
            {
                this.GameType = this.GameType + " 单双排";
            }else if (gameobj.QueueId == 440)
            {
                this.GameType = this.GameType + " 灵活组排";

            }
            else if (gameobj.QueueId == 430)
            {
                this.GameType = this.GameType + " 匹配模式";

            }
            else if (gameobj.QueueId == 450)
            {
                this.GameType = this.GameType + " 大乱斗";

            }
            else if (gameobj.QueueId == 900)
            {
                this.GameType = this.GameType + " 无限乱斗";
            }
            else
            {
                this.GameType = this.GameType + gameobj.QueueId.ToString();
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

            //完善夸夸和牛马标签显示内容
            foreach (string NiumaString in NiuMaTagList)
            {
                NiuMaTag = NiuMaTag + " " + NiumaString;
            }
            foreach (string KuaKuaString in KuaKuaTagList)
            {
                KuaKuaTag = KuaKuaTag + " " + KuaKuaString;
            }

        }

    }
}
