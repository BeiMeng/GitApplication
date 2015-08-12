namespace Util.Webs.Controls.ValidateCodes {
    /// <summary>
    /// 验证码
    /// </summary>
    public interface IValidateCode {
        /// <summary>
        /// 宽
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        int FontSize { get; set; }
        /// <summary>
        /// 是否创建干扰线
        /// </summary>
        bool IsGenereateDisturbLine { get; set; }
        /// <summary>
        /// 生成验证码
        /// </summary>
        void Generate();
        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="input">用户填写的验证码</param>
        bool Check( string input );
    }
}
