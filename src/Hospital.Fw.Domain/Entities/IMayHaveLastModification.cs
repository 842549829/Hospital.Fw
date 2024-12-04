namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可能拥有最后修改人
/// </summary>
public interface IMayHaveLastModification: IMayHaveLastModificationId, IMayHaveLastModificationName, IMayHaveLastModificationTime;