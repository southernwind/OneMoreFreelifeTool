using Livet;

namespace SandBeige.OneMoreFreelifeOnlineTool.Models {
	class Item : NotificationObject{
		public Item(string name, string itemImageUrl) {
			this.Name = name;
			this.ItemImageUrl = itemImageUrl;
			this.Count = 0;
		}

		public string Name {
			get;
		}
		public string ItemImageUrl {
			get;
		}
		private int _count;
		public int Count {
			get {
				return this._count;
			}
			set {
				if (this._count == value) {
					return;
				}
				this._count = value;
				RaisePropertyChanged();
			}
		}
	}
}
