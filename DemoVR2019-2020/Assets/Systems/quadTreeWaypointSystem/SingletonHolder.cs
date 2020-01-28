using UnityEngine;


	public sealed class SingletonHolder : MonoBehaviour
	{

		private static SingletonHolder _instance;

		public static SingletonHolder Instance
		{
			get
			{
				if (!_instance)
				{
					var holder = new GameObject();
					DontDestroyOnLoad(holder);
					holder.name = typeof(SingletonHolder).Name;
					_instance = holder.AddComponent<SingletonHolder>();
				}

				return _instance;
			}
		}

		private void Awake()
		{
			if (!_instance)
			{
				_instance = this;
				return;
			}

			if (_instance != this)
			{
				Destroy(this);
			}
		}

		private void OnDestroy()
		{
			if (_instance && (_instance == this))
			{
				_instance = null;
			}
		}
	}

