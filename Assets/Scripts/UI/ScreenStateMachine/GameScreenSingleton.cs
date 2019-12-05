using UnityEngine;

public abstract class GameScreenSingleton<T> : GameScreen where T : GameScreen {

	protected static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				Debug.LogWarning("GameScreenSingleton of type '" + typeof(T).ToString() + "' is not set...");
			}
			return instance;
		}
		protected set {
			instance = value;
		}
	}

}