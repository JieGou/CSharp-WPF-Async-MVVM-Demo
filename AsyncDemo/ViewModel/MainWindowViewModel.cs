﻿using AsyncDemo.Data;
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
    class MainWindowViewModel : ViewModelBase
    {
        AsyncDemoHelper mAsyncDemoHelper;
        private RelayCommand mStartDemoCommand;
        private RelayCommand mClearListCommand;
        IDialogService mProgressDialogService;

        /// <summary>
        /// Sets and gets the listBox property
        /// </summary>
        public ObservableCollection<string> ListBox { get; set; }


        public MainWindowViewModel(IDialogService dialogService)
        {
            base.ViewTitle = Resources.MainWindowViewModel_Title;

            // Passing an IDialog service allows us to mock up this service
            // and unit test this viewmodel with the mock.
            mProgressDialogService = dialogService;

            ListBox = new ObservableCollection<string>();
            mAsyncDemoHelper = new AsyncDemoHelper();
            mAsyncDemoHelper.WorkPerformed += new EventHandler<WorkPerformedEventArgs>(WorkPerformed);
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
            Thread t = new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    mProgressDialogService.ShowDialog();
                });
            });
            t.Start();

            ListBox.Add("Starting demo!");

            string result = await mAsyncDemoHelper.DoStuffAsync();
            ListBox.Add(result);

            Application.Current.Dispatcher.Invoke(() =>
            {
                mProgressDialogService.CloseDialog();
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
        private void WorkPerformed(object sender, WorkPerformedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBox.Add(args.Data);
            });
        }
    }
}