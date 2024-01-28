namespace TrashTalk
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using System;

    public partial class PlayerData
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }


    public partial class PlayerDataForCustom
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public User User { get; set; }
    }

    public partial class LeaderboardUsers
    {
        public List<User> data;
    }

    public partial class LeaderboardUsers
    {
        public static LeaderboardUsers FromJson(string json) => JsonConvert.DeserializeObject<LeaderboardUsers>(json, TrashTalk.Converter.Settings);
    }

    public partial class GlobalUsers
    {
        public GlobalUsersData data;
    }

    public partial class GlobalUsers
    {
        public static GlobalUsers FromJson(string json) => JsonConvert.DeserializeObject<GlobalUsers>(json, TrashTalk.Converter.Settings);
    }

    public partial class GlobalUsersData
    {
        public List<User> data;
    }

    public partial class GameResponse
    {
        public GameResponseData data;
    }

    public partial class GameResponse
    {
        public static GameResponse FromJson(string json) => JsonConvert.DeserializeObject<GameResponse>(json, TrashTalk.Converter.Settings);
    }

    [Serializable]
    public partial class GameResponseData
    {
        [JsonProperty("GameID")]
        public string GameID;

    //    [JsonProperty("UserIDs")]
    //    public string UserIDs;

    //    [JsonProperty("CoinsToPlay")]
    //    public int FullName;
    }

    [Serializable]
    public partial class User
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("UserID")]
        public string UserId;

        [JsonProperty("FullName")]
        public string FullName;

        [JsonProperty("Email")]
        public string Email;

        [JsonProperty("Password")]
        public string Password;

        [JsonProperty("Image")]
        public string Image;

        [JsonProperty("ImagePath")]
        public string ImagePath;

        [JsonProperty("AuthProvider")]
        public string AuthProvider;

        [JsonProperty("Coins")]
        public long Coins;

        [JsonProperty("AccessToken")]
        public string AccessToken;

        [JsonProperty("WinCount")]
        public int winCount;
    }

    public partial class PlayerData
    {
        public static PlayerData FromJson(string json) => JsonConvert.DeserializeObject<PlayerData>(json, TrashTalk.Converter.Settings);
    }


    public partial class DeSerialize
    {
        public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, TrashTalk.Converter.Settings);
    }


    public static class Serialize
    {
        public static string ToJson<T>(this T self) => JsonConvert.SerializeObject(self, TrashTalk.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
