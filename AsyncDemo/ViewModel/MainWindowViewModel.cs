using AsyncDemo.Data;
using AsyncDemo.EvtArgs;
using AsyncDemo.Properties;
using AsyncDemo.Services;
using AsyncDemo.Veiw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AsyncDemo.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 异步Demo类
        /// </summary>
        private AsyncDemoHelper mAsyncDemoHelper;

        /// <summary>
        /// 开始命令
        /// </summary>
        private RelayCommand mStartDemoCommand;

        /// <summary>
        /// 清除命令
        /// </summary>
        private RelayCommand mClearListCommand;

        /// <summary>
        /// 进度对话框
        /// </summary>
        private IDialogService mProgressDialogService;

        /// <summary>
        /// 开始按钮 文本
        /// </summary>
        /// <remarks>
        /// 从资源来获取
        /// </remarks>
        public string StartButtonText { get; } = Resources.MainWindowViewModel_StartButtonText;

        /// <summary>
        /// 清空按钮 文本
        /// </summary>
        /// <remarks>
        /// 从资源来获取
        /// </remarks>
        public string ClearButtonText { get; } = Resources.MainWindowViewModel_ClearButtonText;

        public string ResultTextBlockText { get; } = Resources.MainWindowViewModel_TextBlockText;

        /// <summary>
        /// Sets and gets the listBox property
        /// </summary>
        public ObservableCollection<string> ListBox { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dialogService"></param>
        public MainWindowViewModel(IDialogService dialogService)
        {
            //标题 通过资源来赋值
            base.ViewTitle = Resources.MainWindowViewModel_Title;

            // Passing an IDialog service allows us to mock up this service
            // and unit test this viewmodel with the mock.
            mProgressDialogService = dialogService;

            ListBox = new ObservableCollection<string>();
            mAsyncDemoHelper = new AsyncDemoHelper();
            mAsyncDemoHelper.WorkPerformed += new EventHandler<WorkPerformedEventArgs>(OnMessageReceived);
        }

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// start the demo.
        /// </summary>
        public ICommand StartDemoCommand
        {
            get
            {
                if (mStartDemoCommand == null)
                {
                    mStartDemoCommand = new RelayCommand(param => OnStartButtonClick());
                }

                return mStartDemoCommand;
            }
        }

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to clear the list box.
        /// </summary>
        public ICommand ClearListCommand
        {
            get
            {
                if (mClearListCommand == null)
                {
                    mClearListCommand = new RelayCommand(param => OnClearListClick());
                }

                return mClearListCommand;
            }
        }

        /// <summary>
        /// Fired when a command is received from the start button
        /// </summary>
        private async void OnStartButtonClick()
        {
            ShowProgressDialog();
            ListBox.Add("Starting demo!");

            string result = await mAsyncDemoHelper.DoStuffAsync();
            ListBox.Add(result);

            Application.Current.Dispatcher.Invoke(() =>
            {
                mProgressDialogService.CloseDialog();
            });
        }

        /// <summary>
        /// Async method to run the progress dialog on a background thread while the UI
        /// Stays responsive
        /// </summary>
        private async void ShowProgressDialog()
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    mProgressDialogService.ShowDialog();
                });
            });
        }

        /// <summary>
        /// Clear the list box in the main window
        /// </summary>
        private void OnClearListClick()
        {
            ListBox.Clear();
        }

        /// <summary>
        /// Callback fired each time some work is performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMessageReceived(object sender, WorkPerformedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBox.Add(args.Data);
            });
        }
    }
}