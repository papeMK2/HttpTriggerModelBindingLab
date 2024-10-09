using HttpTriggerModelBindingLab.Domains.Interfaces;
using HttpTriggerModelBindingLab.Models;
using System.Text.Json;

namespace HttpTriggerModelBindingLab.Domains;

public class ItemDeserializer : IDeserializer
{
    public IBindModel? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Item>(json);
    }
}