using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Nekko_LCU
{

    public class Tag
    {
        public string TagName { get; set; } = "";
        public Color TagColor { get; set; }

        public Tag() { }

        public Tag(string InTagName, Color InTagColor) {
            this.TagName = InTagName;
            this.TagColor = InTagColor;
        }

    }

    public class TagInfo
    {
        public List<Tag> BattleTagList = new List<Tag>();
        public List<Tag> NiuMaTagList = new List<Tag>();
        public List<Tag> KDATagList = new List<Tag>();

        public List<Tag> DescTagList = new List<Tag>();
        public TagInfo() { }

        public Color GetNumberLimitColor(double InputNumber, double limit1, double limit2, double limit3)
        {
            if (InputNumber < limit1)
            {
                return Color.FromArgb(255, 255, 0, 0); //红色
            }
            else if (InputNumber >= limit1 && InputNumber < limit2)
            {
                return Color.FromArgb(255, 255, 255, 0); //黄色
            }
            else if (InputNumber >= limit2 && InputNumber < limit3)
            {
                return Color.FromArgb(255, 0, 255, 0); // 绿色
            }
            else
            {
                return Color.FromArgb(255, 128, 0, 128); // 紫色
            }
        }

        public TagInfo(List<GameObject> gameObjectList)
        {
            KDAInfo kdaInfo = new KDAInfo(gameObjectList);

            Dictionary<string, int> KuaKuaTagMap = new Dictionary<string, int>();
            Dictionary<string, int> NiuMaTagMap = new Dictionary<string, int>();


            int totalNiumaTag = 0;
            int totalKuaKuaTag = 0;

            int totalWin = 0;
            int totalLose = 0;

            foreach (GameObject gameobj in gameObjectList)
            {

                BattleInfo battleInfo = new BattleInfo(gameobj);

                if (battleInfo.Win == "胜利")
                {
                    totalWin++;
                }
                else
                {
                    totalLose++;
                }

                foreach (string NiumaTag in battleInfo.NiuMaTagList)
                {
                    if (NiuMaTagMap.ContainsKey(NiumaTag))
                    {
                        NiuMaTagMap[NiumaTag] = NiuMaTagMap[NiumaTag] + 1;
                    }
                    else
                    {
                        NiuMaTagMap[NiumaTag] = 1;
                    }
                    totalNiumaTag++;
                }

                foreach (string KuaKuaTag in battleInfo.KuaKuaTagList)
                {
                    if (KuaKuaTagMap.ContainsKey(KuaKuaTag))
                    {
                        KuaKuaTagMap[KuaKuaTag] = KuaKuaTagMap[KuaKuaTag] + 1;
                    }
                    else
                    {
                        KuaKuaTagMap[KuaKuaTag] = 1;
                    }
                    totalKuaKuaTag++;

                }
            }

            

            foreach (var pair in KuaKuaTagMap)
            {
                string KuaKuaTag = "";
                KuaKuaTag = pair.Key + "*" + pair.Value;
                BattleTagList.Add(new Tag(KuaKuaTag, Color.FromArgb(255, 0, 255, 0)));
            }

            foreach (var pair in NiuMaTagMap)
            {
                string KuaKuaTag = "";
                KuaKuaTag = pair.Key + "*" + pair.Value;
                BattleTagList.Add(new Tag(KuaKuaTag, Color.FromArgb(255, 255, 0, 0)));
            }

            double NiumaNumber = (double)totalNiumaTag / (totalKuaKuaTag + totalNiumaTag) * 100;

            if (NiumaNumber <= 10)
            {
                NiuMaTagList.Add(new Tag("金牌大腿", Color.FromArgb(255, 0, 255, 0)));
                DescTagList.Add(new Tag("金牌大腿", Color.FromArgb(255, 0, 255, 0)));
            }
            else if (NiumaNumber > 10 && NiumaNumber <= 20)
            {
                NiuMaTagList.Add(new Tag("银牌大腿", Color.FromArgb(255, 0, 255, 0)));
                DescTagList.Add(new Tag("银牌大腿", Color.FromArgb(255, 0, 255, 0)));
            }
            else if (NiumaNumber > 20 && NiumaNumber <= 30)
            {
                NiuMaTagList.Add(new Tag("偶尔能C", Color.FromArgb(255, 0, 255, 0)));
                DescTagList.Add(new Tag("偶尔能C", Color.FromArgb(255, 0, 255, 0)));
            }
            else if (NiumaNumber > 30 && NiumaNumber <= 40)
            {
                NiuMaTagList.Add(new Tag("混子", Color.FromArgb(255, 0, 255, 0)));
                DescTagList.Add(new Tag("混子", Color.FromArgb(255, 0, 255, 0)));

            }
            else if (NiumaNumber > 40)
            {
                NiuMaTagList.Add(new Tag("峡谷鬼见愁", Color.FromArgb(255, 255, 0, 0)));
                DescTagList.Add(new Tag("峡谷鬼见愁", Color.FromArgb(255, 255, 0, 0)));
            }

            string NiumaZhiShu = "牛马含量: " + NiumaNumber.ToString("F1") + "%";

            NiuMaTagList.Add(new Tag(NiumaZhiShu, GetNumberLimitColor(100 / NiumaNumber, 2, 2.5, 5)));
            DescTagList.Add(new Tag(NiumaZhiShu, GetNumberLimitColor(100 / NiumaNumber, 2, 2.5, 5)));


            //近期胜率
            int totalBattle = totalWin + totalLose;
            double totalShenglv = (double)totalWin / totalBattle * 100;

            NiuMaTagList.Add(new Tag("近" + totalBattle .ToString() + "局单双排胜率: " + totalShenglv.ToString("F1") + "%", GetNumberLimitColor(totalShenglv / 100, 0.5, 0.55, 0.6)));
            DescTagList.Add(new Tag("近" + totalBattle .ToString() + "局单双排胜率: " + totalShenglv.ToString("F1") + "%", GetNumberLimitColor(totalShenglv / 100, 0.5, 0.55, 0.6)));

            //计算KDA

            KDATagList.Add(new Tag("KDA: " + kdaInfo.kda.ToString("F1"), GetNumberLimitColor(kdaInfo.kda, 1.0, 2.0, 3.0)));
            DescTagList.Add(new Tag("KDA: " + kdaInfo.kda.ToString("F1"), GetNumberLimitColor(kdaInfo.kda, 1.0, 2.0, 3.0)));

            KDATagList.Add(new Tag("性价比: " + kdaInfo.realkda.ToString("F1"), GetNumberLimitColor(kdaInfo.realkda, 0.5, 1.0, 2.0)));
            DescTagList.Add(new Tag("性价比: " + kdaInfo.realkda.ToString("F1"), GetNumberLimitColor(kdaInfo.realkda, 0.5, 1.0, 2.0)));


            KDATagList.Add(new Tag("团队性: " + kdaInfo.teamkda.ToString("F1"), GetNumberLimitColor(kdaInfo.teamkda, 2, 5, 10)));
            DescTagList.Add(new Tag("团队性: " + kdaInfo.teamkda.ToString("F1"), GetNumberLimitColor(kdaInfo.teamkda, 2, 5, 10)));


            KDATagList.Add(new Tag(kdaInfo.MaName, GetNumberLimitColor(kdaInfo.kda, 2, 3, 4)));
            DescTagList.Add(new Tag(kdaInfo.MaName, GetNumberLimitColor(kdaInfo.kda, 2, 3, 4)));

            KDATagList.Add(new Tag("场均击杀: " + kdaInfo.AverageKill.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageKill, 1, 8, 15)));
            DescTagList.Add(new Tag("场均击杀: " + kdaInfo.AverageKill.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageKill, 1, 8, 15)));

            KDATagList.Add(new Tag("场均助攻: " + kdaInfo.AverageAssistant.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageAssistant, 5, 10, 15)));
            DescTagList.Add(new Tag("场均助攻: " + kdaInfo.AverageAssistant.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageAssistant, 5, 10, 15)));

            KDATagList.Add(new Tag("场均死亡: " + kdaInfo.AverageDeath.ToString("F1"), GetNumberLimitColor(1.0 / kdaInfo.AverageDeath, 0.1, 0.2, 0.3)));
            DescTagList.Add(new Tag("场均死亡: " + kdaInfo.AverageDeath.ToString("F1"), GetNumberLimitColor(1.0 / kdaInfo.AverageDeath, 0.1, 0.2, 0.3)));
        }

    }
}
