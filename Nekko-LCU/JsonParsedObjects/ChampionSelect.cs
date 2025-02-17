using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko_LCU
{
    public class TeamMemberInfo
    {
        [JsonProperty("assignedPosition")]
        public string AssignedPosition { get; set; } = "";

        [JsonProperty("cellId")]
        public int CellId { get; set; } = 0;

        [JsonProperty("championId")]
        public int ChampionId { get; set; } = 0;

        [JsonProperty("championPickIntent")]
        public int ChampionPickIntent { get; set; } = 0;

        [JsonProperty("puuid")]
        public string Puuid { get; set; } = "";
        [JsonProperty("summonerId")]
        public ulong summonerId { get; set; } = 0;
    }


    public class ChampionSelect
    {
        [JsonProperty("myTeam")]
        public List<TeamMemberInfo> teamMemberList { get; set; }


        public ChampionSelect() { }

        public ChampionSelect(string jsonString)
        {
            JsonConvert.PopulateObject(jsonString, this);
        }


    }
}
