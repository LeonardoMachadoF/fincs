namespace FinCs.Communication.Responses;

public class ResponseErrorJson(string errorMessage)
{
    public string[] Errors { get; set; } = errorMessage.Split("|");
}