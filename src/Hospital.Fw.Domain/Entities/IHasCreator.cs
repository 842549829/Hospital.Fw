namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
public interface IHasCreator : IHasCreatorTime, IMayHaveCreatorName, IMayHaveCreatorId, IHasEnabled;