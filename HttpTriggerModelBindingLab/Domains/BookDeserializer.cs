using HttpTriggerModelBindingLab.Domains.Interfaces;
using HttpTriggerModelBindingLab.Models;
using System.Text.Json;

namespace HttpTriggerModelBindingLab.Domains;
public class BookDeserializer : IDeserializer
{
    public IBindModel? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Book>(json);
    }
}
