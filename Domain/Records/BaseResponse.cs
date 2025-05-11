
using Flunt.Notifications;

namespace Domain.Records;
public class BaseResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<Notification>? Notifications { get; set; }

    public BaseResponse(int statusCode, string message, T? data = default, List<Notification>? notifications = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Notifications = notifications;
    }
}
