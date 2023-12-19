namespace Shared.Models.Responses;

public enum ServiceResultStatus
{
    SUCCESS,
    FAILED,
    NOT_FOUND,
    ALREADY_EXISTS,
    CREATED,
    UPDATED,
    DELETED
}
public class ServiceResult
{
    public ServiceResultStatus Status { get; set; }
    public object Result { get; set; }
}
