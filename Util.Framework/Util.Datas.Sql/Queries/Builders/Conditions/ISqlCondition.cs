namespace Util.Datas.Sql.Queries.Builders.Conditions {
    /// <summary>
    /// Sql条件
    /// </summary>
    public interface ISqlCondition {
        /// <summary>
        /// 获取条件
        /// </summary>
        string GetCondition();
    }
}
