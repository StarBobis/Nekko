using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_LCU
{
    public class ParticipantPlayer
    {
        [JsonProperty("accountId")]
        public ulong AccountId { get; set; } = 0;

        [JsonProperty("currentAccountId")]
        public ulong CurrentAccountId { get; set; } = 0;

        [JsonProperty("currentPlatformId")]
        public string CurrentPlatformId { get; set; } = "";

        [JsonProperty("gameName")]
        public string GameName { get; set; } = "";

        [JsonProperty("matchHistoryUri")]
        public string MatchHistoryUri { get; set; } = "";

        [JsonProperty("platformId")]
        public string PlatformId { get; set; } = "";

        [JsonProperty("profileIcon")]
        public ulong ProfileIcon { get; set; } = 0;

        [JsonProperty("puuid")]
        public string Puuid { get; set; } = "";

        [JsonProperty("summonerId")]
        public ulong SummonerId { get; set; } = 0;

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; } = "";

        [JsonProperty("tagLine")]
        public string TagLine { get; set; } = "";
    }

    public class ParticipantIdentity
    {
        [JsonProperty("participantId")]
        public ulong ParticipantId { get; set; } = 0;

        [JsonProperty("player")]
        public ParticipantPlayer ParticipantPlayer { get; set; }
    }

    public class GameObject
    {
        [JsonProperty("endOfGameResult")]
        public string EndOfGameResult { get; set; } = "";

        [JsonProperty("gameCreation")]
        public ulong GameCreation { get; set; } = 0;

        [JsonProperty("gameDuration")]
        public ulong GameDuration { get; set; } = 0;

        [JsonProperty("gameCreationDate")]
        public string GameCreationDate { get; set; } = "";

        [JsonProperty("gameId")]
        public ulong GameId { get; set; } = 0;

        [JsonProperty("mapId")]
        public ulong mapId { get; set; } = 0;

        [JsonProperty("gameMode")]
        public string GameMode { get; set; } = "";

        [JsonProperty("gameType")]
        public string GameType { get; set; } = "";

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; } = "";

        [JsonProperty("platformId")]
        public string PlatformId { get; set; } = "";

        [JsonProperty("queueId")]
        public ulong QueueId { get; set; } = 0;

        [JsonProperty("seasonId")]
        public ulong SeasonId { get; set; } = 0;

        [JsonProperty("participantIdentities")]
        List<ParticipantIdentity> ParticipantIdentities { get; set; }


    }

    public class GamesObject
    {
        [JsonProperty("gameBeginDate")]
        public string GameBeginDate { get; set; } = "";

        [JsonProperty("gameEndDate")]
        public string GameEndDate { get; set; } = "";

        [JsonProperty("gameCount")]
        public ulong GameCount { get; set; } = 0;

        [JsonProperty("gameIndexBegin")]
        public ulong GameIndexBegin { get; set; } = 0;

        [JsonProperty("gameIndexEnd")]
        public ulong GameIndexEnd { get; set; } = 0;

        [JsonProperty("games")]
        public GameObject GameObject { get; set; }
    }

    public class GameRecord
    {
        [JsonProperty("accountId")]
        public ulong AccountId { get; set; } = 0;

        [JsonProperty("platformId")]
        public string PlatformId { get; set; } = "";

        [JsonProperty("games")]
        public GamesObject GamesObject { get; set; }
    }
}
