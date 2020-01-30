using UnityEngine;

	public class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = SingletonHolder.Instance.gameObject.GetOrAddComponentInChildren<T>();
				}

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (!_instance)
			{
				_instance = this as T;
				return;
			}

			if (_instance != this)
			{
				Destroy(this);
			}
		}

		protected virtual void OnDestroy()
		{
			if (_instance && (_instance == this))
			{
				_instance = null;
			}
		}
	}

