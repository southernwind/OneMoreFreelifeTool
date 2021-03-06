﻿using System;
using System.IO;
using System.Windows;

using Fiddler;

using Livet;

using SandBeige.OneMoreFreelifeOnlineTool.ViewModels;
using SandBeige.OneMoreFreelifeOnlineTool.Views;

namespace SandBeige.OneMoreFreelifeOnlineTool {
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application {
		protected override void OnStartup(StartupEventArgs e) {
			DispatcherHelper.UIDispatcher = this.Dispatcher;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			this.MainWindow = new MainWindow() {
				DataContext = new MainWindowViewModel()
			};
			this.MainWindow.ShowDialog();
		}

		//集約エラーハンドラ
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {

			URLMonInterop.ResetProxyInProcessToDefault();
			FiddlerApplication.Shutdown();

			if (e.ExceptionObject is Exception ex) {
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				File.AppendAllText("log.txt", $"{ex.Message}{ex.StackTrace}");
			} else {
				Console.WriteLine(e.ToString());
			}

			//TODO:ロギング処理など
			MessageBox.Show(
				$"不明なエラーが発生しました。アプリケーションを終了します。{e.ToString()}",
				"エラー",
				MessageBoxButton.OK,
				MessageBoxImage.Error);

			Environment.Exit(1);
		}
	}
}
