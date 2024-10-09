using HttpTriggerModelBindingLab.Domains.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HttpTriggerModelBindingLab.Models;
public class Item : IBindModel
{
    [MaxLength(10)]
    public required string Name { get; set; }
    public Detail? Detail { get; set; }
}

public class Detail
{
    public required string Code { get; set; }
    [Required]
    public int Price { get; set; }

}
