using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Progression : MonoBehaviour {

	private static Progression instance;
	public static Progression Instance {
		get {
			if(instance == null) {
				instance = new GameObject("Progression").AddComponent<Progression>();
			}
			return instance;
		}
	}

	public delegate void EventDelegate(GameEvent gameEvent);
	public event EventDelegate OnEventCompleted;

	private List<GameEvent> openEvents;
	private List<GameEvent> closedEvents;

	public void CompleteEvent(GameEvent gameEvent) {
		openEvents.Remove(gameEvent);
		closedEvents.Add(gameEvent);
		OnEventCompleted?.Invoke(gameEvent);
	}

	public bool EventIsCompleted(GameEvent gameEvent) {
		return closedEvents.Contains(gameEvent);
	}

	private void Awake() {
		openEvents = Resources.LoadAll<GameEvent>("GameEvents").ToList();
	}

}