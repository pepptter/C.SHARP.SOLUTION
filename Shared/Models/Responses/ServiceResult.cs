using Shared.Enums;
using Shared.Interfaces;

namespace Shared.Models.Responses;
public class ServiceResult : IServiceResult
{
    public ServiceResultStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
