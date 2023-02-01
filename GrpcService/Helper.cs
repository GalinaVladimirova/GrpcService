using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrpcService
{
    public static class Helper
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public class JsonObject
        {
            public string Type { get; set; }
        }

        private static string GetType(string item)
        {
            var jsonObj = JsonSerializer.Deserialize<JsonObject>(item, _jsonSerializerOptions);
            return jsonObj.Type;
        }

        public static Dictionary<string, List<string>> GroupItems(List<string> items)
        {
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var item in items)
            {
                var trimmed = item.Trim();
                var itemType = GetType(trimmed);
                if (!dictionary.ContainsKey(itemType))
                {
                    dictionary.Add(itemType, new List<string>() { trimmed });
                    continue;
                }

                dictionary[itemType].Add(trimmed);
            }
            return dictionary;
        }
    }
}
