namespace HttpTriggerModelBindingLab.Models;
public class ErrorResponse
{
    public ErrorResponse() 
    {
        this.Errors = new List<string>();
    }
    public List<string> Errors { get; set; }
}
