namespace Util.Domains.Repositories {
    /// <summary>
    /// 查询条件
    /// </summary>
    public interface ICriteria {
        /// <summary>
        /// 获取谓词
        /// </summary>
        string GetPredicate();
        /// <summary>
        /// 获取值列表
        /// </summary>
        object[] GetValues();
    }
}
