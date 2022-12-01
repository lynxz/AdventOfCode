namespace Tools
{
    public abstract class DayBase
    {
        private const string DATA_FILE = "data";
        private readonly HttpClient _httpClient;
        private readonly string _year;

        protected DayBase(string day)
        {
            var cookie = $"session={File.ReadAllText("../cookie")}";
            _httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com") };
            _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);

            _year = File.ReadAllText("../year");
            Day = day;
        }

        protected string Day { get; }

        public abstract string FirstStar();

        public abstract string SecondStar();

        protected string GetRawData() =>
            GetAllInput().GetAwaiter().GetResult();

        protected string[] GetRowData() =>
            GetData(s => s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));

        protected int[] GetIntRowData() =>
            GetRowData().Select(r => int.Parse(r)).ToArray();

        protected T GetData<T>(Func<string, T> processor) =>
            processor(GetAllInput().GetAwaiter().GetResult());

        private async Task<string> GetAllInput()
        {
            if (File.Exists(DATA_FILE))
                return await File.ReadAllTextAsync(DATA_FILE);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_year}/day/{Day}/input");

            using var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                await File.WriteAllTextAsync(DATA_FILE, data);
                return data;
            }
            else
            {
                throw new Exception($"Could not fetch data Status Code {response.StatusCode}, {response.ReasonPhrase}");
            }
        }

        public void OutputFirstStar() =>
            System.Console.WriteLine(FirstStar());

        public void OutputSecondStar() =>
            System.Console.WriteLine(SecondStar());

        public void PostFirstStar() =>
            SendAnswer(FirstStar(), 1).GetAwaiter().GetResult();

        public void PostSecondStar() =>
            SendAnswer(SecondStar(), 2).GetAwaiter().GetResult();

        private async Task SendAnswer(string answer, int level)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_year}/day/{Day}/answer");
            request.Content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string> ("level", level.ToString()),
                new KeyValuePair<string, string>("answer", answer)
            });

            using (var response = await _httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("That's the right answer!"))
                    {
                        System.Console.WriteLine("Success");
                    }
                    else
                    {
                        var start = content.IndexOf("<main>") + 6;
                        var stop = content.IndexOf("</main>");
                        System.Console.WriteLine(content.Substring(start, stop - start));
                    }
                }
                else
                {
                    throw new Exception($"Could not post answer Status Code {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
        }

    }
}

