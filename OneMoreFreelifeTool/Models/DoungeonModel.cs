using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

using Codeplex.Data;

using Fiddler;

using Livet;

namespace SandBeige.OneMoreFreelifeOnlineTool.Models {
	class DoungeonModel : NotificationObject {

		private ObservableCollection<Item> _acquiredItems;
		public ObservableCollection<Item> AcquiredItems {
			get {
				return this._acquiredItems ?? (this._acquiredItems = new ObservableCollection<Item>());
			}
		}

		public void Start() {

			// 証明書インストール
			if (!CertMaker.rootCertExists()) {
				CertMaker.createRootCert();
				CertMaker.trustRootCert();
			}

			// プロキシサーバー開始
			FiddlerApplication.Startup(24791, true, true);

			FiddlerApplication.AfterSessionComplete += new SessionStateHandler((session) => {
				if (Regex.IsMatch(session.host, @".+\.omf-game\.jp")) {
					var contentType = session.ResponseHeaders.FirstOrDefault(x => x.Name == "Content-Type")?.Value;
					if (contentType == "application/json") {
						var fileName = Regex.Replace(session.url, @".*/(.+?)\?.+", "$1");
						if (new[] { "battleresult", "battleraidbossresult" }.Contains(fileName)) {
							RegisterResult(session);
						}
					}
				}
			});
		}

		public void Reset() {
			this.AcquiredItems.Clear();
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

		private void RegisterResult(Session session) {
			foreach (var (Name, Img, Num) in GetItems(session.GetResponseBodyAsString())) {
				var existingItem = this.AcquiredItems.FirstOrDefault(x => x.Name == Name && x.ItemImageUrl == Img);
				if (existingItem == null) {
					var newItem = new Item(Name, Img);
					newItem.Count += Num;
					this.AcquiredItems.Add(newItem);
					continue;
				}
				existingItem.Count += Num;
			}
		}

		public void Stop() {
			// プロキシサーバー終了
			URLMonInterop.ResetProxyInProcessToDefault();
			FiddlerApplication.Shutdown();
		}
	}
}
