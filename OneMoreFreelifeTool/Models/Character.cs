using Livet;

namespace SandBeige.OneMoreFreelifeOnlineTool.Models {
	class Character : NotificationObject {
		public Character(string name) {
			this.Name = name;
		}

		public string Name {
			get;
		}
		private string _imageUrl;
		public string ImageUrl {
			get {
				return this._imageUrl;
			}
			set {
				if (this._imageUrl == value) {
					return;
				}
				this._imageUrl = value;
				RaisePropertyChanged();
			}
		}

		private int _hp;
		public int Hp {
			get {
				return this._hp;
			}
			set {
				if (this._hp == value) {
					return;
				}
				this._hp = value;
				RaisePropertyChanged();
			}
		}

		private int _maxHp;
		public int MaxHp {
			get {
				return this._maxHp;
			}
			set {
				if (this._maxHp == value) {
					return;
				}
				this._maxHp = value;
				RaisePropertyChanged();
			}
		}


		private int _mp;
		public int Mp {
			get {
				return this._mp;
			}
			set {
				if (this._mp == value) {
					return;
				}
				this._mp = value;
				RaisePropertyChanged();
			}
		}


		private int _maxMp;
		public int MaxMp {
			get {
				return this._maxMp;
			}
			set {
				if (this._maxMp == value) {
					return;
				}
				this._maxMp = value;
				RaisePropertyChanged();
			}
		}
	}
}
