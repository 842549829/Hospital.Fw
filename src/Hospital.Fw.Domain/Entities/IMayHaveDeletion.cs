namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有删除人标识
/// </summary>
public interface IMayHaveDeletion : IMayHaveDeletionId, IMayHaveDeletionTime, IMayHaveDeletionName;