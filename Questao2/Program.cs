using Newtonsoft.Json;
using System.Text.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }


    private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";


    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        using var http = new HttpClient();

        var homeTask = SomarGolsAsync(http, team, year, timeEhMandante: true);
        var awayTask = SomarGolsAsync(http, team, year, timeEhMandante: false);
        var results = await Task.WhenAll(homeTask, awayTask);
        return results[0] + results[1];
    }


    private static async Task<int> SomarGolsAsync(HttpClient http, string team, int year, bool timeEhMandante)
    {
        string param = timeEhMandante ? "team1" : "team2";
        string campoGols = timeEhMandante ? "team1goals" : "team2goals";

        int page = 1;
        int somaGols = 0;
        int totalPages;

        do
        {
            var url = $"{BaseUrl}?year={year}&{param}={Uri.EscapeDataString(team)}&page={page}";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            totalPages = root.GetProperty("total_pages").GetInt32();

            foreach (var partida in root.GetProperty("data").EnumerateArray())
                somaGols += int.Parse(partida.GetProperty(campoGols).GetString()!);

        } while (++page <= totalPages);

        return somaGols;
    }
}

