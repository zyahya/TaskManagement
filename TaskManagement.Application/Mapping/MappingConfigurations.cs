using Mapster;

using TaskManagement.Application.Contracts.TaskItem;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TaskItem, CreateTaskItemRequest>();
    }
}
