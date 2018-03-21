
using Livet;

using Reactive.Bindings;

using SandBeige.OneMoreFreelifeOnlineTool.Models;

namespace SandBeige.OneMoreFreelifeOnlineTool.ViewModels {
	class MainWindowViewModel : ViewModel {

		private DoungeonModel _doungeon;

		private ReactiveCommand _resetCommand;
		/// <summary>
		/// アイテムリストリセットコマンド
		/// </summary>
		public ReactiveCommand ResetCommand {
			get {
				return this._resetCommand ?? (this._resetCommand = new ReactiveCommand());
			}
		}

		private ReactiveCommand _startCommand;
		/// <summary>
		/// 開始コマンド
		/// </summary>
		public ReactiveCommand StartCommand {
			get {
				return this._startCommand ?? (this._startCommand = new ReactiveCommand());
			}
		}

		private ReactiveCommand _stopCommand;
		/// <summary>
		/// 終了コマンド
		/// </summary>
		public ReactiveCommand StopCommand {
			get {
				return this._stopCommand ?? (this._stopCommand = new ReactiveCommand());
			}
		}

		/// <summary>
		/// アイテムリスト
		/// </summary>
		public ReadOnlyReactiveCollection<Item> AcquiredItems {
			get;
		}

		public MainWindowViewModel() {
			this._doungeon = new DoungeonModel();

			this.AcquiredItems = this._doungeon.AcquiredItems.ToReadOnlyReactiveCollection();


			this.ResetCommand.Subscribe(this._doungeon.Reset);
			this.StartCommand.Subscribe(this._doungeon.Start);
			this.StopCommand.Subscribe(this._doungeon.Stop);
		}
	}
}
