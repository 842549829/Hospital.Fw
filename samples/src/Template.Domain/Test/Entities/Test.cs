using Hospital.Fw.Domain.Entities;
using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Template.Domain.Test.Entities;

[Tenant(SqlSugarCoreDbConst.Job)]
public class Test : AuditedEntity<string>
{
    [SugarColumn(Length = 64, IsNullable = true, ColumnDescription = "名称")]
    public string Name { get; set; } = default!;
}