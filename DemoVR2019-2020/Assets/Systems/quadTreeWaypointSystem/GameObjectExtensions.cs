using UnityEngine;

namespace SoleilGS.Breath
{
	public static class GameObjectExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject instance) where T : Component
		{
			var targetComponent = instance.GetComponent<T>();
			if (!targetComponent)
			{
				targetComponent = instance.AddComponent<T>();
			}

			return targetComponent;
		}

		public static T GetOrAddComponentInChildren<T>(this GameObject instance) where T : Component
		{
			var targetComponent = instance.GetComponentInChildren<T>();
			if (!targetComponent)
			{
				targetComponent = instance.AddComponent<T>();
			}

			return targetComponent;
		}
	}
}
