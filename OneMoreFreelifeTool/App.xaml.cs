using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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

			// 証明書インストール
			if (!CertMaker.rootCertExists()) {
				CertMaker.createRootCert();
				CertMaker.trustRootCert();
			}

			FiddlerApplication.Startup(0, true, true);

			this.MainWindow = new MainWindow() {
				DataContext = new MainWindowViewModel()
			};
			this.MainWindow.ShowDialog();
			FiddlerApplication.Shutdown();
		}

		//集約エラーハンドラ
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {

			FiddlerApplication.Shutdown();

			if (e.ExceptionObject is Exception ex) {
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
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
