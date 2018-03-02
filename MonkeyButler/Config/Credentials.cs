using Newtonsoft.Json;
using System.IO;

namespace MonkeyButler.Config
{
    public class Credentials
    {
        private const string DefaultFileLocation = "Config\\Credentials.json";

        public long ClientId { get; set; }
        public long OwnerId { get; set; }
        public string Token { get; set; }

        public Credentials(string fileLocation = DefaultFileLocation)
        {
            using (var file = File.OpenText(fileLocation))
            {
                var serializer = new JsonSerializer();
                serializer.Populate(file, this);
            }
        }
    }
}
