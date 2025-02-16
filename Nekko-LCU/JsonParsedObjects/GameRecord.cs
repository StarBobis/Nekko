using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_LCU
{
    //ELO就是靠这些数据来调整两边队伍的。
    //综合对比两个队伍的对位总和，就能知道这把游戏的走向。
    public class ParticipantStats
    {
        [JsonProperty("assists")]
        public ulong Assists { get; set; } = 0;

        [JsonProperty("causedEarlySurrender")]
        public bool CausedEarlySurrender { get; set; } = false;

        [JsonProperty("champLevel")]
        public ulong ChampLevel { get; set; } = 0;

        [JsonProperty("combatPlayerScore")]
        public ulong CombatPlayerScore { get; set; } = 0;

        [JsonProperty("damageDealtToObjectives")]
        public ulong DamageDealtToObjectives { get; set; } = 0;

        [JsonProperty("damageDealtToTurrets")]
        public ulong DamageDealtToTurrets { get; set; } = 0;

        [JsonProperty("damageSelfMitigated")]
        public ulong DamageSelfMitigated { get; set; } = 0;

        [JsonProperty("deaths")]
        public ulong Deaths { get; set; } = 0;

        [JsonProperty("doubleKills")]
        public ulong DoubleKills { get; set; } = 0;

        [JsonProperty("earlySurrenderAccomplice")]
        public bool EarlySurrenderAccomplice { get; set; } = false;

        [JsonProperty("firstBloodAssist")]
        public bool FirstBloodAssist { get; set; } = false;

        [JsonProperty("firstBloodKill")]
        public bool FirstBloodKill { get; set; } = false;

        [JsonProperty("firstInhibitorAssist")]
        public bool FirstInhibitorAssist { get; set; } = false;

        [JsonProperty("firstInhibitorKill")]
        public bool FirstInhibitorKill { get; set; } = false;

        [JsonProperty("firstTowerAssist")]
        public bool FirstTowerAssist { get; set; } = false;

        [JsonProperty("firstTowerKill")]
        public bool FirstTowerKill { get; set; } = false;

        [JsonProperty("gameEndedInEarlySurrender")]
        public bool GameEndedInEarlySurrender { get; set; } = false;

        [JsonProperty("gameEndedInSurrender")]
        public bool GameEndedInSurrender { get; set; } = false;

        [JsonProperty("goldEarned")]
        public ulong GoldEarned { get; set; } = 0;

        [JsonProperty("goldSpent")]
        public ulong GoldSpent { get; set; } = 0;

        [JsonProperty("inhibitorKills")]
        public ulong InhibitorKills { get; set; } = 0;

        [JsonProperty("item0")]
        public ulong Item0 { get; set; } = 0;

        [JsonProperty("item1")]
        public ulong Item1 { get; set; } = 0;

        [JsonProperty("item2")]
        public ulong Item2 { get; set; } = 0;

        [JsonProperty("item3")]
        public ulong Item3 { get; set; } = 0;

        [JsonProperty("item4")]
        public ulong Item4 { get; set; } = 0;

        [JsonProperty("item5")]
        public ulong Item5 { get; set; } = 0;

        [JsonProperty("item6")]
        public ulong Item6 { get; set; } = 0;

        [JsonProperty("killingSprees")]
        public ulong KillingSprees { get; set; } = 0;

        [JsonProperty("kills")]
        public ulong Kills { get; set; } = 0;

        [JsonProperty("largestCriticalStrike")]
        public ulong LargestCriticalStrike { get; set; } = 0;

        [JsonProperty("largestKillingSpree")]
        public ulong LargestKillingSpree { get; set; } = 0;

        [JsonProperty("largestMultiKill")]
        public ulong LargestMultiKill { get; set; } = 0;

        [JsonProperty("longestTimeSpentLiving")]
        public ulong LongestTimeSpentLiving { get; set; } = 0;

        [JsonProperty("magicDamageDealt")]
        public ulong MagicDamageDealt { get; set; } = 0;

        [JsonProperty("magicDamageDealtToChampions")]
        public ulong MagicDamageDealtToChampions { get; set; } = 0;

        [JsonProperty("magicalDamageTaken")]
        public ulong MagicalDamageTaken { get; set; } = 0;

        [JsonProperty("neutralMinionsKilled")]
        public ulong NeutralMinionsKilled { get; set; } = 0;

        [JsonProperty("neutralMinionsKilledEnemyJungle")]
        public ulong NeutralMinionsKilledEnemyJungle { get; set; } = 0;

        [JsonProperty("neutralMinionsKilledTeamJungle")]
        public ulong NeutralMinionsKilledTeamJungle { get; set; } = 0;

        [JsonProperty("objectivePlayerScore")]
        public ulong ObjectivePlayerScore { get; set; } = 0;

        [JsonProperty("participantId")]
        public ulong ParticipantId { get; set; } = 0;

        [JsonProperty("pentaKills")]
        public ulong PentaKills { get; set; } = 0;

        [JsonProperty("perk0")]
        public ulong Perk0 { get; set; } = 0;

        [JsonProperty("perk0Var1")]
        public ulong Perk0Var1 { get; set; } = 0;

        [JsonProperty("perk0Var2")]
        public ulong Perk0Var2 { get; set; } = 0;

        [JsonProperty("perk0Var3")]
        public ulong Perk0Var3 { get; set; } = 0;

        [JsonProperty("perk1")]
        public ulong Perk1 { get; set; } = 0;

        [JsonProperty("perk1Var1")]
        public ulong Perk1Var1 { get; set; } = 0;

        [JsonProperty("perk1Var2")]
        public ulong Perk1Var2 { get; set; } = 0;

        [JsonProperty("perk1Var3")]
        public ulong Perk1Var3 { get; set; } = 0;


        [JsonProperty("perk2")]
        public ulong Perk2 { get; set; } = 0;

        [JsonProperty("perk2Var1")]
        public ulong Perk2Var1 { get; set; } = 0;

        [JsonProperty("perk2Var2")]
        public ulong Perk2Var2 { get; set; } = 0;

        [JsonProperty("perk2Var3")]
        public ulong Perk2Var3 { get; set; } = 0;


        [JsonProperty("perk3")]
        public ulong Perk3 { get; set; } = 0;

        [JsonProperty("perk3Var1")]
        public ulong Perk3Var1 { get; set; } = 0;

        [JsonProperty("perk3Var2")]
        public ulong Perk3Var2 { get; set; } = 0;

        [JsonProperty("perk3Var3")]
        public ulong Perk3Var3 { get; set; } = 0;


        [JsonProperty("perk4")]
        public ulong Perk4 { get; set; } = 0;

        [JsonProperty("perk4Var1")]
        public ulong Perk4Var1 { get; set; } = 0;

        [JsonProperty("perk4Var2")]
        public ulong Perk4Var2 { get; set; } = 0;

        [JsonProperty("perk4Var3")]
        public ulong Perk4Var3 { get; set; } = 0;


        [JsonProperty("perk5")]
        public ulong Perk5 { get; set; } = 0;

        [JsonProperty("perk5Var1")]
        public ulong Perk5Var1 { get; set; } = 0;

        [JsonProperty("perk5Var2")]
        public ulong Perk5Var2 { get; set; } = 0;

        [JsonProperty("perk5Var3")]
        public ulong Perk5Var3 { get; set; } = 0;


        [JsonProperty("perkPrimaryStyle")]
        public ulong PerkPrimaryStyle { get; set; } = 0;

        [JsonProperty("perkSubStyle")]
        public ulong PerkSubStyle { get; set; } = 0;

        [JsonProperty("physicalDamageDealt")]
        public ulong PhysicalDamageDealt { get; set; } = 0;

        [JsonProperty("physicalDamageDealtToChampions")]
        public ulong PhysicalDamageDealtToChampions { get; set; } = 0;

        [JsonProperty("physicalDamageTaken")]
        public ulong PhysicalDamageTaken { get; set; } = 0;

        [JsonProperty("playerAugment1")]
        public ulong PlayerAugment1 { get; set; } = 0;

        [JsonProperty("playerAugment2")]
        public ulong PlayerAugment2 { get; set; } = 0;

        [JsonProperty("playerAugment3")]
        public ulong PlayerAugment3 { get; set; } = 0;

        [JsonProperty("playerAugment4")]
        public ulong PlayerAugment4 { get; set; } = 0;

        [JsonProperty("playerAugment5")]
        public ulong PlayerAugment5 { get; set; } = 0;

        [JsonProperty("playerAugment6")]
        public ulong PlayerAugment6 { get; set; } = 0;


        [JsonProperty("playerScore0")]
        public ulong PlayerScore0 { get; set; } = 0;

        [JsonProperty("playerScore1")]
        public ulong PlayerScore1 { get; set; } = 0;

        [JsonProperty("playerScore2")]
        public ulong PlayerScore2 { get; set; } = 0;

        [JsonProperty("playerScore3")]
        public ulong PlayerScore3 { get; set; } = 0;

        [JsonProperty("playerScore4")]
        public ulong PlayerScore4 { get; set; } = 0;

        [JsonProperty("playerScore5")]
        public ulong PlayerScore5 { get; set; } = 0;

        [JsonProperty("playerScore6")]
        public ulong PlayerScore6 { get; set; } = 0;

        [JsonProperty("playerScore7")]
        public ulong PlayerScore7 { get; set; } = 0;

        [JsonProperty("playerScore8")]
        public ulong PlayerScore8 { get; set; } = 0;

        [JsonProperty("playerScore9")]
        public ulong PlayerScore9 { get; set; } = 0;

        [JsonProperty("playerSubteamId")]
        public ulong PlayerSubteamId { get; set; } = 0;

        [JsonProperty("quadraKills")]
        public ulong QuadraKills { get; set; } = 0;

        [JsonProperty("sightWardsBoughtInGame")]
        public ulong SightWardsBoughtInGame { get; set; } = 0;

        [JsonProperty("subteamPlacement")]
        public ulong SubteamPlacement { get; set; } = 0;

        [JsonProperty("teamEarlySurrendered")]
        public bool teamEarlySurrendered { get; set; } = false;

        [JsonProperty("timeCCingOthers")]
        public ulong TimeCCingOthers { get; set; } = 0;

        [JsonProperty("totalDamageDealt")]
        public ulong TotalDamageDealt { get; set; } = 0;

        [JsonProperty("totalDamageDealtToChampions")]
        public ulong TotalDamageDealtToChampions { get; set; } = 0;

        [JsonProperty("totalDamageTaken")]
        public ulong TotalDamageTaken { get; set; } = 0;

        [JsonProperty("totalHeal")]
        public ulong TotalHeal { get; set; } = 0;

        [JsonProperty("totalMinionsKilled")]
        public ulong TotalMinionsKilled { get; set; } = 0;

        [JsonProperty("totalPlayerScore")]
        public ulong TotalPlayerScore { get; set; } = 0;

        [JsonProperty("totalScoreRank")]
        public ulong TotalScoreRank { get; set; } = 0;

        [JsonProperty("totalTimeCrowdControlDealt")]
        public ulong TotalTimeCrowdControlDealt { get; set; } = 0;

        [JsonProperty("totalUnitsHealed")]
        public ulong TotalUnitsHealed { get; set; } = 0;

        [JsonProperty("tripleKills")]
        public ulong TripleKills { get; set; } = 0;

        [JsonProperty("trueDamageDealt")]
        public ulong TrueDamageDealt { get; set; } = 0;

        [JsonProperty("trueDamageDealtToChampions")]
        public ulong TrueDamageDealtToChampions { get; set; } = 0;

        [JsonProperty("trueDamageTaken")]
        public ulong TrueDamageTaken { get; set; } = 0;

        [JsonProperty("turretKills")]
        public ulong TurretKills { get; set; } = 0;

        [JsonProperty("unrealKills")]
        public ulong UnrealKills { get; set; } = 0;

        [JsonProperty("visionScore")]
        public ulong VisionScore { get; set; } = 0;

        [JsonProperty("visionWardsBoughtInGame")]
        public ulong VisionWardsBoughtInGame { get; set; } = 0;

        [JsonProperty("wardsKilled")]
        public ulong WardsKilled { get; set; } = 0;

        [JsonProperty("wardsPlaced")]
        public ulong WardsPlaced { get; set; } = 0;

        [JsonProperty("win")]
        public bool Win { get; set; } = false;

    }


    public class ParticipantTimeline
    {
        //creepsPerMinDeltas {}
        //csDiffPerMinDeltas {}
        //damageTakenDiffPerMinDeltas {}
        //damageTakenPerMinDeltas {}
        //goldPerMinDeltas {}
        //xpDiffPerMinDeltas {}
        //xpPerMinDeltas {}
        [JsonProperty("lane")]
        public string Lane { get; set; } = "";

        [JsonProperty("role")]
        public string Role { get; set; } = "";

        [JsonProperty("participantId")]
        public ulong ParticipantId { get; set; } = 0;

    }

    public class Participant
    {
        [JsonProperty("championId")]
        public ulong ChampionId { get; set; } = 0;

        [JsonProperty("highestAchievedSeasonTier")]
        public string HighestAchievedSeasonTier { get; set; } = "";

        [JsonProperty("participantId")]
        public ulong ParticipantId { get; set; } = 0;

        [JsonProperty("spell1Id")]
        public ulong Spell1Id { get; set; } = 0;

        [JsonProperty("spell2Id")]
        public ulong Spell2Id { get; set; } = 0;

        [JsonProperty("stats")]
        public ParticipantStats ParticipantStats { get; set; }

        [JsonProperty("teamId")]
        public ulong TeamId { get; set; } = 0;

        [JsonProperty("timeline")]
        public ParticipantTimeline ParticipantTimeline { get; set; }

    }

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

    public class Ban
    {
        [JsonProperty("championId")]
        public int ChampionId { get; set; } = 0;

        [JsonProperty("pickTurn")]
        public int PickTurn { get; set; } = 0;

    }

    public class Team
    {
        [JsonProperty("bans")]
        public List<Ban> Bans { get; set; }

        [JsonProperty("baronKills")]
        public ulong BaronKills { get; set; } = 0;

        [JsonProperty("dominionVictoryScore")]
        public ulong DominionVictoryScore { get; set; } = 0;

        [JsonProperty("dragonKills")]
        public ulong DragonKills { get; set; } = 0;

        [JsonProperty("firstBaron")]
        public bool FirstBaron { get; set; } = false;

        [JsonProperty("firstBlood")]
        public bool FirstBlood { get; set; } = false;

        [JsonProperty("firstDargon")]
        public bool FirstDargon { get; set; } = false;

        [JsonProperty("firstInhibitor")]
        public bool FirstInhibitor { get; set; } = false;

        [JsonProperty("firstTower")]
        public bool FirstTower { get; set; } = false;

        [JsonProperty("hordeKills")]
        public ulong HordeKills { get; set; } = 0;

        [JsonProperty("inhibitorKills")]
        public ulong InhibitorKills { get; set; } = 0;

        [JsonProperty("riftHeraldKills")]
        public ulong RiftHeraldKills { get; set; } = 0;

        [JsonProperty("teamId")]
        public ulong TeamId { get; set; } = 0;

        [JsonProperty("towerKills")]
        public ulong TowerKills { get; set; } = 0;

        [JsonProperty("vilemawKills")]
        public ulong VilemawKills { get; set; } = 0;

        [JsonProperty("win")]
        public string Win { get; set; } = "";
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
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }


        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }
    }

    public class GamesObjects
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
        public List<GameObject> GameObjectList { get; set; }
    }

    public class GameRecord
    {
        [JsonProperty("accountId")]
        public ulong AccountId { get; set; } = 0;

        [JsonProperty("platformId")]
        public string PlatformId { get; set; } = "";

        [JsonProperty("games")]
        public GamesObjects GamesObjects { get; set; }


        public GameRecord()
        {

        }

        public GameRecord(string jsonString)
        {
            JsonConvert.PopulateObject(jsonString, this);
        }
    }
}
