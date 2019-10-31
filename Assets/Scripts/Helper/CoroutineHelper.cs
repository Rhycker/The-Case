using System.Collections;
using UnityEngine;
using System;

public class CoroutineHelper : MonoBehaviour {

	private static CoroutineHelper _instance;
	private static CoroutineHelper instance {
		get {
			if(_instance == null) {
				_instance = new GameObject("CoroutineHelper").AddComponent<CoroutineHelper>();
			}
			return _instance;
		}
	}

	public delegate bool StopWaitingCondition ();

	public static void WaitOneFrame(Action action){
		instance.StartCoroutine (IE_WaitOneFrame (action));		
	}

	private static IEnumerator IE_WaitOneFrame(Action action){
		yield return null;
		if (action != null) {
			action ();
		}
	}

	public static void WaitForSeconds(float delay, Action action){
		instance.StartCoroutine(IE_WaitForSeconds(delay, action));
	}

	private static IEnumerator IE_WaitForSeconds(float delay, Action action){
		yield return new WaitForSeconds (delay);
		if (action != null) {
			action ();
		}
	}

	public static void WaitUntil(Action action, StopWaitingCondition stopWaitCondition){
		instance.StartCoroutine(IE_WaitUntil(action, stopWaitCondition));
	}

	private static IEnumerator IE_WaitUntil(Action action, StopWaitingCondition stopWaitCondition){
		yield return new WaitUntil (() => stopWaitCondition ());
		if (action != null) {
			action ();
		}
	}

	public static void StartCoroutineAtInstance(IEnumerator enumerator) {
		instance.StartCoroutine(enumerator);
	}

}