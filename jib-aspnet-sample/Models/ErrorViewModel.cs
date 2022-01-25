namespace jib_example.Models;

///<summary>Sadly there was a problem</summary>
public class ErrorViewModel
{
    ///<summary>here's how you can refer to it</summary>
    public string? RequestId { get; set; }

    ///<summary>but sometimes it's not available</summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
