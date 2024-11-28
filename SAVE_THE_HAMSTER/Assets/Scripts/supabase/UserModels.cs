using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace com.example.Models
{
    [Table("users")]
    public class UserProfile : BaseModel
    {
        [PrimaryKey("user_id", false)]
        [Column("user_id")]
        public string user_id { get; set; }

        [Column("nickname")]
        public string nickname { get; set; }

        [Column("almonds")]
        public int almonds { get; set; }

        [Column("stage1_clear")]
        public bool stage1_clear { get; set; }

        [Column("stage2_clear")]
        public bool stage2_clear { get; set; }

        [Column("stage3_clear")]
        public bool stage3_clear { get; set; }
    }

    [Table("store")]
    public class UserStore : BaseModel
    {
        [PrimaryKey("user_id", false)]
        [Column("user_id")]
        public string user_id { get; set; }

        [Column("hamster_skin_1")]
        public bool hamster_skin_1 { get; set; }

        [Column("hamster_skin_2")]
        public bool hamster_skin_2 { get; set; }

        [Column("cannon_skin_1")]
        public bool cannon_skin_1 { get; set; }

        [Column("cannon_skin_2")]
        public bool cannon_skin_2 { get; set; }

        [Column("ball_tail_effect_1")]
        public bool ball_tail_effect_1 { get; set; }

        [Column("ball_tail_effect_2")]
        public bool ball_tail_effect_2 { get; set; }
    }

    [Table("stage_status")]
    public class UserStageStatus : BaseModel
    {
        [Column("user_id")]
        public string user_id { get; set; }

        [Column("stage_id")]
        public int stage_id { get; set; }

        [Column("almond_status")]
        public bool[] almond_status { get; set; }
    }

    [Table("stage_records")]
    public class UserStageRecords : BaseModel
    {
        [Column("user_id")]
        public string user_id { get; set; }

        [Column("nickname")]
        public string nickname { get; set; }

        [Column("stage_id")]
        public int stage_id { get; set; }

        [Column("num_hits")]
        public int num_hits { get; set; }

        [Column("clear_time")]
        public float clear_time { get; set; }

        [Column("last_attempt")]
        public string last_attempt { get; set; }
    }
}
