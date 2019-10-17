using UnityEngine;

[RequireComponent(typeof(VIDE_Assign))]
public class DoorBlockingCharacter : MonoBehaviour {

	public bool BlockDoor { get { return dialogue.overrideStartNode == -1 || dialogue.overrideStartNode == doorBlockOverrideStartNode; } }

	[SerializeField] private int doorBlockOverrideStartNode;

	private VIDE_Assign dialogue;

	private void Awake() {
		dialogue = GetComponent<VIDE_Assign>();
	}

	public void ShowBlockDoorDialogue() {
		dialogue.overrideStartNode = doorBlockOverrideStartNode;
		DialogueUI.Instance.StartDialogue(dialogue);
	}

}