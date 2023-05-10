using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Jazz.Covenant.Base
{
    public class TestBase
    {
        private readonly ITestOutputHelper _output;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public void PrintPayload<T>(T print)
        {
            var payload = JsonConvert.SerializeObject(print, Formatting.Indented);
            _output.WriteLine("--- PAYLOAD ----------------------------------------");
            _output.WriteLine(payload);
        }

        public void PrintResultJson<T>(T print)
        {
            var triggerJson = JsonConvert.SerializeObject(print, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            _output.WriteLine("--- RESULT ----------------------------------------");
            _output.WriteLine(triggerJson.Replace("\\", String.Empty).Replace("rn", String.Empty));
        }
    }
}