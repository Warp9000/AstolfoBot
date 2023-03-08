namespace AstolfoBot.Modules.Osu.Api
{
    public static class OsuExtensions
    {
        public static string @string(this Structures.GameMode obj)
        {
            switch (obj)
            {
                case Structures.GameMode.osu:
                    return "osu!";
                case Structures.GameMode.taiko:
                    return "osu!taiko";
                case Structures.GameMode.fruits:
                    return "osu!catch";
                case Structures.GameMode.mania:
                    return "osu!mania";
                default:
                    return "Unknown";
            }
        }
        // public static string @string(this Structures.Sub.HitStatistics obj, Structures.GameMode gm)
        // {
        //     switch (gm)
        //     {
        //         case Structures.GameMode.mania:
        //             return $"{obj.CountKatu:N0}x MARV, {obj.Count300:N0}x PERF, {obj.CountGeki:N0}x GREAT, {obj.Count100:N0}x GOOD, {obj.Count50:N0}x OK, {obj.CountMiss:N0}x miss";
        //         default:
        //             return $"{obj.Count300:N0}x 300, {obj.Count100:N0}x 100, {obj.Count50:N0}x 50, {obj.CountMiss:N0}x miss";
        //     }
        // }
    }
}