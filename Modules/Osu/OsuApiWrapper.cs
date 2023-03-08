using System.Text;
using Newtonsoft.Json;
using Discord.Interactions;
using System.Net.Http.Headers;

namespace AstolfoBot.Modules.Osu.Api
{
    public class OsuApiWrapper
    {
        private const string BaseOsuUrl = "https://osu.ppy.sh/api/v2/";
        private const string BaseLazerUrl = "https://lazer.ppy.sh/api/v2/";
        private readonly HttpClient Client;
        private ClientCredentials clientCredentials = new();
        public OsuApiWrapper()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("User-Agent", "AstolfoBot");
        }

        public async Task<Structures.User> GetUserAsync(string id, Structures.GameMode? Mode = null)
        {
            var user = await GetAsync<Structures.User>($"users/{id}{(Mode != null ? $"/{Mode}" : "")}");
            return user;
        }
        public async Task<Structures.Score[]> GetUserScoresRecentAsync(string id, Structures.GameMode? Mode = null, int? limit = 10, int? offset = 0, bool? includeFails = true)
        {
            limit ??= 10;
            offset ??= 0;
            Dictionary<string, string> parameters = new()
            {
                { "include_fails", includeFails == true ? "1" : "0" }
            };
            if (Mode != null)
                parameters.Add("mode", Mode.ToString()!);
            parameters.Add("limit", limit.ToString()!);
            parameters.Add("offset", offset.ToString()!);
            string url = $"users/{id}/scores/recent?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            var scores = await GetAsync<Structures.Score[]>(url);
            return scores ?? Array.Empty<Structures.Score>();
        }
        public async Task<Structures.Score[]> GetUserScoresBestAsync(string id, Structures.GameMode? Mode = null, int? limit = 10, int? offset = 0, bool? includeFails = true)
        {
            limit ??= 10;
            offset ??= 0;
            Dictionary<string, string> parameters = new()
            {
                { "include_fails", includeFails == true ? "1" : "0" }
            };
            if (Mode != null)
                parameters.Add("mode", Mode.ToString()!);
            parameters.Add("limit", limit.ToString()!);
            parameters.Add("offset", offset.ToString()!);
            string url = $"users/{id}/scores/best?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            var scores = await GetAsync<Structures.Score[]>(url);
            return scores ?? Array.Empty<Structures.Score>();
        }
        public async Task<Structures.Score[]> GetUserScoresFirstsAsync(string id, Structures.GameMode? Mode = null, int? limit = 10, int? offset = 0, bool? includeFails = true)
        {
            limit ??= 10;
            offset ??= 0;
            Dictionary<string, string> parameters = new()
            {
                { "include_fails", includeFails == true ? "1" : "0" }
            };
            if (Mode != null)
                parameters.Add("mode", Mode.ToString()!);
            parameters.Add("limit", limit.ToString()!);
            parameters.Add("offset", offset.ToString()!);
            string url = $"users/{id}/firsts?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            var scores = await GetAsync<Structures.Score[]>(url);
            return scores ?? Array.Empty<Structures.Score>();
        }
        public async Task<Structures.Beatmapset> GetBeatmapsetAsync(ulong id)
        {
            var beatmapset = await GetAsync<Structures.Beatmapset>($"beatmapsets/{id}");
            return beatmapset;
        }
        public async Task<Structures.Beatmapsets> SearchBeatmapsetsAsync(string query)
        {
            var sets = await GetAsync<Structures.Beatmapsets>($"beatmapsets/search?q={query}");
            return sets;
        }
        public async Task<Structures.Beatmap> GetBeatmapAsync(ulong id)
        {
            var beatmap = await GetAsync<Structures.Beatmap>($"beatmaps/{id}");
            return beatmap;
        }
        public async Task<Structures.Scores> GetBeatmapScoresAsync(ulong id, Structures.GameMode? Mode = null)
        {
            var scores = await GetAsync<Structures.Scores>($"beatmaps/{id}/scores{(Mode != null ? $"?mode={Mode}" : "")}");
            return scores;
        }
        public async Task<Structures.Scores> GetBeatmapUserScoresAsync(ulong mapid, ulong userid)
        {
            var scores = await GetAsync<Structures.Scores>($"beatmaps/{mapid}/scores/users/{userid}/all");
            return scores;
        }
        //TODO set private
        public async Task<T?> GetAsync<T>(string path, bool isLazer = false)
        {
            return JsonConvert.DeserializeObject<T>(await GetAsync(path, isLazer));
        }
        public async Task<string> GetAsync(string path, bool isLazer = false)
        {
            var url = (isLazer ? BaseLazerUrl : BaseOsuUrl) + path;
            if (clientCredentials.created.AddSeconds(clientCredentials.expires_in) < DateTime.Now)
                await RefreshClientCredentials();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"{clientCredentials.token_type} {clientCredentials.access_token}");
            // request.Headers.Authorization = new AuthenticationHeaderValue(clientCredentials.token_type, clientCredentials.access_token);
            // var response = await Client.GetAsync();
            var response = await Client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        private async Task RefreshClientCredentials()
        {
            string[] token = File.ReadAllLines("osutoken.txt");
            var headers = new Dictionary<string, string>
                {
                    { "client_id", token[0]},
                    { "client_secret", token[1]},
                    { "grant_type", "client_credentials"},
                    { "scope", "public "}
                };
            var json = JsonConvert.SerializeObject(headers);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("https://osu.ppy.sh/oauth/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            clientCredentials = JsonConvert.DeserializeObject<ClientCredentials>(responseContent);
        }
        public struct ClientCredentials
        {
            public ClientCredentials(
                string token_type,
                int expires_in,
                string access_token
            )
            {
                this.token_type = token_type;
                this.expires_in = expires_in;
                this.access_token = access_token;
                this.created = DateTime.Now;
            }
            public string token_type = "";
            public int expires_in = 0;
            public string access_token = "";
            public DateTime created = DateTime.Now;
        }
    }
}