namespace HttpTriggerModelBindingLab.Domains.Interfaces;
public interface IDeserializer
{
    IBindModel? Deserialize(string json);
}
