using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Livet;

namespace SandBeige.OneMoreFreelifeOnlineTool.ViewModels {
	class MainWindowViewModel:ViewModel {

		public MainWindowViewModel() {
			FiddlerApplication.AfterSessionComplete += new SessionStateHandler((session) => {
				Console.WriteLine(session.host);
			});
		}
	}
}
