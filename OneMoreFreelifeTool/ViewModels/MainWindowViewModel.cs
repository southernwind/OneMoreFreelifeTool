using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Codeplex.Data;

using Fiddler;

using Livet;
using Livet.Commands;
using SandBeige.OneMoreFreelifeOnlineTool.Models;

namespace SandBeige.OneMoreFreelifeOnlineTool.ViewModels {
	class MainWindowViewModel : ViewModel {

		public DispatcherCollection<Item> AcquiredItems {
			get;
		}

		public MainWindowViewModel() {
			this.AcquiredItems = new DispatcherCollection<Item>(DispatcherHelper.UIDispatcher);

			FiddlerApplication.AfterSessionComplete += new SessionStateHandler((session) => {
				if (Regex.IsMatch(session.host, @".+\.omf-game\.jp")) {
					var contentType = session.ResponseHeaders.FirstOrDefault(x => x.Name == "Content-Type")?.Value;
					if (contentType == "application/json" && session.url.Contains("battleresult")) {
						foreach (var item in GetItems(session.GetResponseBodyAsString())) {
							var existingItem = this.AcquiredItems.FirstOrDefault(x => x.Name == item.Name && x.ItemImageUrl == item.Img);
							if (existingItem == null) {
								var newItem = new Item(item.Name, item.Img);
								newItem.Count += item.Num;
								this.AcquiredItems.Add(newItem);
								continue;
							}
							existingItem.Count += item.Num;
						}
					}
				}
			});
		}

		private ICommand _resetCommand;
		/// <summary>
		/// リセットコマンド
		/// </summary>
		public ICommand ResetCommand {
			get {
				return this._resetCommand ?? (this._resetCommand = new ViewModelCommand(()=>
				{
					this.AcquiredItems.Clear();
				}));
			}
		}

		/// <summary>
		/// 探索結果のjsonを(アイテム名、アイテム画像URL、個数)にする
		/// </summary>
		/// <param name="jsonString">json形式のテキスト</param>
		/// <returns></returns>
		private IEnumerable<(string Name, string Img, int Num)> GetItems(string jsonString) {
			var json = DynamicJson.Parse(jsonString);
			for (var waveIndex = 0; waveIndex < 3; waveIndex++) {
				if (!json.IsDefined("treasure_data_list") || !json.treasure_data_list.IsDefined(waveIndex)) {
					continue;
				}
				var wave = json.treasure_data_list[waveIndex];
				for (var itemIndex = 1; itemIndex <= 6; itemIndex++) {
					if (!wave.IsDefined($"img_{itemIndex}") ||
						!wave.IsDefined($"name_{itemIndex}") ||
						!wave.IsDefined($"num_{itemIndex}")) {
						continue;
					}
					var img = wave[$"img_{itemIndex}"];
					var name = wave[$"name_{itemIndex}"];
					var num = wave[$"num_{itemIndex}"];
					if (num is string numString) {
						yield return (name, img, int.Parse(numString));
					}
					if (num is double numDouble) {
						yield return (name, img, (int)numDouble);
					}
				}
			}
		}

	}
}
