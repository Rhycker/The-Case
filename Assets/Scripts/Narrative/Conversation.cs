using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

// Example: VIDEUIManager1.cs
public class Conversation : MonoBehaviour {

	[SerializeField] private string saveGameName = "Game";

	private void Awake() {
		// VD.LoadDialogues(); //Load all dialogues to memory so that we dont spend time doing so later
		VD.LoadDialogues();

		//Loads the saved state of VIDE_Assigns and dialogues.
		VD.LoadState(saveGameName, true);
	}

	public void EndDialogue(VD.NodeData data) {
		//Saves VIDE stuff related to EVs and override start nodes
		VD.SaveState("VIDEDEMOScene1", true);
	}

	private void UpdateUI(VD.NodeData data) {

	}

	// NOT READY IMPLEMENTING
	private void OnDisable() {
		//If the script gets destroyed, let's make sure we force-end the dialogue to prevent errors
		//We do not save changes
		//VD.OnActionNode -= ActionHandler;
		VD.OnNodeChange -= UpdateUI;
		VD.OnEnd -= EndDialogue;
		//if (dialogueContainer != null)
			//dialogueContainer.SetActive(false);
		VD.EndDialogue();
	}

}