using HttpTriggerModelBindingLab.Domains.Interfaces;

namespace HttpTriggerModelBindingLab.Models;
public class Book : IBindModel
{
    public required string Author {  get; set; }
}
