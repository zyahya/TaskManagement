using Mapster;

using TaskManagement.Core.Contracts.Request;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TaskItem, CreateTaskItemRequest>();
    }
}
