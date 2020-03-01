namespace AsyncDemo.Services
{
    /// <summary>
    /// 窗口对话框的接口
    /// </summary>
    internal interface IDialogService
    {
        /// <summary>
        /// 关闭对话框
        /// </summary>
        void CloseDialog();

        /// <summary>
        /// 显示对话框
        /// </summary>
        void ShowDialog();
    }
}