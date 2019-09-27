using System.Collections.Generic;
using UnityEngine;

public class ActionTester : MonoBehaviour {

	private VIDE_Assign vide;

	private void Awake() {
		vide = GetComponent<VIDE_Assign>();
	}

	public void SayHello() {
		Debug.Log("Hello");
	}
	
}