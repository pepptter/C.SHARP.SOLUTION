using Shared.Enums;

namespace Shared.Interfaces;
/// <summary>
/// Models a service result response, which contains a result and a status.
/// </summary>
public interface IServiceResult
{
    object Result { get; set; }
    ServiceResultStatus Status { get; set; }
}