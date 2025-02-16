using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_LCU
{
    public class RerollPoints
    {
        [JsonProperty("currentPoints")]
        public int CurrentPoints { get; set; } = 0;

        [JsonProperty("maxRolls")]
        public int MaxRolls { get; set; } = 0;

        [JsonProperty("numberOfRolls")]
        public int NumberOfRolls { get; set; } = 0;

        [JsonProperty("pointsCostToRoll")]
        public int PointsCostToRoll { get; set; } = 0;

        [JsonProperty("pointsToReroll")]
        public int PointsToReroll { get; set; } = 0;
    }

    public class SummonerInfo
    {
        [JsonProperty("accountId")]
        public ulong AccountId { get; set; } = 0;

        [JsonProperty("displayName")]
        public string DisplayName { get; set; } = "";

        [JsonProperty("gameName")]
        public string GameName { get; set; } = "";

        [JsonProperty("internalName")]
        public string InternalName { get; set; } = "";

        [JsonProperty("nameChangeFlag")]
        public bool NameChangeFlag { get; set; } = false;

        [JsonProperty("percentCompleteForNextLevel")]
        public int PercentCompleteForNextLevel { get; set; } = 0;

        [JsonProperty("privacy")]
        public string Privacy { get; set; } = "";

        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; } = 0;

        [JsonProperty("puuid")]
        public string Puuid { get; set; } = "";

        [JsonProperty("summonerId")]
        public ulong SummonerId { get; set; } = 0;

        [JsonProperty("summonerLevel")]
        public int SummonerLevel { get; set; } = 0;

        [JsonProperty("tagLine")]
        public string TagLine { get; set; } = "";

        [JsonProperty("unnamed")]
        public bool Unnamed { get; set; } = false;

        [JsonProperty("xpSinceLastLevel")]
        public int XpSinceLastLevel { get; set; } = 0;

        [JsonProperty("xpUntilNextLevel")]
        public int XpUntilNextLevel { get; set; } = 0;

        [JsonProperty("rerollPoints")]
        public RerollPoints RerollPoints { get; set; }

        public SummonerInfo()
        {

        }

        public SummonerInfo(string jsonString)
        {
            JsonConvert.PopulateObject(jsonString, this);

            //var summonerInfo = JsonConvert.DeserializeObject<SummonerInfo>(jsonString);
            //AccountId = summonerInfo.AccountId;
            //DisplayName = summonerInfo.DisplayName;
            //GameName = summonerInfo.GameName;
            //InternalName = summonerInfo.InternalName;
            //NameChangeFlag = summonerInfo.NameChangeFlag;
            //PercentCompleteForNextLevel = summonerInfo.PercentCompleteForNextLevel;
            //Privacy = summonerInfo.Privacy;
            //ProfileIconId = summonerInfo.ProfileIconId;
            //Puuid = summonerInfo.Puuid;
            //SummonerId = summonerInfo.SummonerId;
            //SummonerLevel = summonerInfo.SummonerLevel;
            //TagLine = summonerInfo.TagLine;
            //Unnamed = summonerInfo.Unnamed;
            //XpSinceLastLevel = summonerInfo.XpSinceLastLevel;
            //XpUntilNextLevel = summonerInfo.XpUntilNextLevel;
            //RerollPoints = summonerInfo.RerollPoints;
        }
    }
}
