namespace Hospital.Fw.Domain.Entities
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public interface IHasEnabled
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }
    }
}
