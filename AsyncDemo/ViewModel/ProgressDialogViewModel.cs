using AsyncDemo.Properties;

namespace AsyncDemo.ViewModel
{
    /// <summary>
    /// 进度条的的ViewModel
    /// </summary>
    internal class ProgressDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgressDialogViewModel()
        {
            //进度条标题 从资源中获取
            base.ViewTitle = Resources.ProgressDialogViewModel_Title;
        }
    }
}