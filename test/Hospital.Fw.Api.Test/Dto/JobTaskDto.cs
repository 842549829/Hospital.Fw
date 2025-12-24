using Hospital.Fw.Application.Contract;

namespace Hospital.Fw.Api.Test.Dto;

public class JobTaskDto : EntityDto<string>
{
    public string JobName { get; set; } = null!;

    public string JobArgs { get; set; } = null!;
}


public class JobTaskCreateDto : EntityDto<string>
{
    public string JobName { get; set; } = null!;

    public string JobArgs { get; set; } = null!;
}

public class JobTaskUpdateDto : EntityDto<string>
{
    public string JobName { get; set; } = null!;

    public string JobArgs { get; set; } = null!;
}

public class JobTaskGetListDto : EntityDto<string>
{
    public string JobName { get; set; } = null!;

    public string JobArgs { get; set; } = null!;
}

public class JobTaskGetListInput : PageInput
{
    public string JobName { get; set; } = null!;

    public string JobArgs { get; set; } = null!;
}