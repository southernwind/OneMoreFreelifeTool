using Livet;

using Reactive.Bindings;

using SandBeige.OneMoreFreelifeOnlineTool.Models;

namespace SandBeige.OneMoreFreelifeOnlineTool.ViewModels {
	class CharactersWindowViewModel : ViewModel {
		private DoungeonModel _doungeon;

		public ReadOnlyReactiveCollection<Character> Characters {
			get;
		}

		public CharactersWindowViewModel(DoungeonModel doungeon) {
			this._doungeon = doungeon;

			this.Characters = this._doungeon.Characters.ToReadOnlyReactiveCollection();
		}
	}
}
