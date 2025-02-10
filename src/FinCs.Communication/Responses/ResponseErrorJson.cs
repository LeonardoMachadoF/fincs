namespace FinCs.Communication.Responses;

public class ResponseErrorJson
{
    public ResponseErrorJson(string message)
    {
        Errors = [message];
    }

    public ResponseErrorJson(List<string> errorMessages)
    {
        Errors = errorMessages;
    }

    public List<string> Errors { get; set; }
}