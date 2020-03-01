using AsyncDemo.Veiw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncDemo.Services
{
    /// <summary>
    /// 进度条对话框
    /// </summary>
    internal class ProgressDialogService : IDialogService
    {
        /// <summary>
        /// 进度条
        /// </summary>
        private Window progressDialog;

        /// <summary>
        /// 进度条的父窗口
        /// </summary>
        private Window owner;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner"></param>
        public ProgressDialogService(Window owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// 关闭进度条
        /// </summary>
        public void CloseDialog()
        {
            if (progressDialog != null)
            {
                progressDialog.Close();
            }
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        public void ShowDialog()
        {
            progressDialog = new ProgressDialog();

            if (owner != null)
            {
                progressDialog.Owner = owner;
            }

            progressDialog.ShowDialog();
        }
    }
}