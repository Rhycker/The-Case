using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using TMPro;
using UnityEngine.UI;

// Example: VIDEUIManager1.cs
[DefaultExecutionOrder(-1)]
public class DialogueUI : MonoBehaviour {

	private const string SAVE_GAME_NAME = "Game";

	public static DialogueUI Instance { get; private set; }

	[SerializeField] private GameObject dialogueContainer;
	[SerializeField] private Color textColorDefault;
	[SerializeField] private Color textColorHighlight;
	[Space]
	[SerializeField] private GameObject playerContainer;
	[SerializeField] private TMP_Text playerChoiceTemplate; 
	[SerializeField] private Image playerImage;
	[SerializeField] private TMP_Text playerLabelText;
	[Space]
	[SerializeField] private GameObject npcContainer;
	[SerializeField] private Image npcImage;
	[SerializeField] private TMP_Text npcText;
	[SerializeField] private TMP_Text npcLabelText;

	private List<TMP_Text> currentChoices;
	private string playerDisplayName;

	public void SetPlayerName(string displayName) {
		playerDisplayName = displayName;
	}

	public void Interact(VIDE_Assign dialogue) {
		if (VD.isActive) {
			VD.Next();
		}
		else {
			StartDialogue(dialogue);
		}
	}

	private void Awake() {
		Instance = this;

		//VD.LoadDialogues();
		VD.LoadState(SAVE_GAME_NAME, true);

		currentChoices = new List<TMP_Text>();
		dialogueContainer.SetActive(false);
		playerChoiceTemplate.gameObject.SetActive(false);
	}

	private void Update() {
		UpdateCommentChoice();
	}

	private void StartDialogue(VIDE_Assign dialogue) {
		playerLabelText.text = "";
		npcText.text = "";
		npcLabelText.text = "";

		VD.OnNodeChange += OnNodeChange;
		VD.OnEnd += OnDialogueEnd;
		VD.BeginDialogue(dialogue);

		dialogueContainer.SetActive(true);
	}

	private void UpdateCommentChoice() {
		if (!VD.isActive) { return; }
		VD.NodeData nodeData = VD.nodeData;
		if (!nodeData.isPlayer) { return; }
		if (nodeData.pausedAction) { return; }

		if (GameInput.Instance.Service.PreviousChoiceButtonDown()) {
			if (nodeData.commentIndex == 0) {
				nodeData.commentIndex = currentChoices.Count - 1;
			}
			else {
				nodeData.commentIndex--;
			}
			UpdateChoiceVisuals(nodeData.commentIndex);
		}
		else if (GameInput.Instance.Service.NextChoiceButtonDown()) {
			if (nodeData.commentIndex == currentChoices.Count - 1) {
				nodeData.commentIndex = 0;
			}
			else {
				nodeData.commentIndex++;
			}
			UpdateChoiceVisuals(nodeData.commentIndex);
		}
	}

	private void UpdateChoiceVisuals(int commentIndex) {
		for (int i = 0; i < currentChoices.Count; i++) {
			if (i == commentIndex) {
				currentChoices[i].color = textColorHighlight;
			}
			else {
				currentChoices[i].color = textColorDefault;
			}
		}
	}

	private void OnDialogueEnd(VD.NodeData nodeData) {
		VD.OnNodeChange -= OnNodeChange;
		VD.OnEnd -= OnDialogueEnd;
		dialogueContainer.SetActive(false);
		VD.EndDialogue();
		VD.SaveState(SAVE_GAME_NAME, true);
	}

	private void OnNodeChange(VD.NodeData nodeData) {
		// Reset some variables and previous player choices
		foreach(TMP_Text choice in currentChoices) {
			Destroy(choice.gameObject);
		}
		currentChoices.Clear();
		playerContainer.SetActive(false);
		npcContainer.SetActive(false);

		// Set the UI according to the new node data
		if (nodeData.isPlayer) {
			bool useCustomNodeSprite = nodeData.sprite != null;
			playerImage.sprite = useCustomNodeSprite ? nodeData.sprite : VD.assigned.defaultPlayerSprite;

			for (int i = 0; i < nodeData.comments.Length; i++) {
				TMP_Text newChoiceText = Instantiate(playerChoiceTemplate, playerChoiceTemplate.transform.position, Quaternion.identity, playerChoiceTemplate.transform.parent);
				newChoiceText.gameObject.SetActive(true);
				newChoiceText.text = nodeData.comments[i];
				currentChoices.Add(newChoiceText);
			}
			UpdateChoiceVisuals(nodeData.commentIndex);

			bool useCustomNodeLabel = !string.IsNullOrEmpty(nodeData.tag);
			playerLabelText.text = useCustomNodeLabel ? nodeData.tag : playerDisplayName;

			playerContainer.SetActive(true);
		}
		else {
			bool useCustomNodeSprite = nodeData.sprite != null;
			npcImage.sprite = useCustomNodeSprite ? nodeData.sprite : VD.assigned.defaultNPCSprite;

			npcText.text = nodeData.comments[nodeData.commentIndex];

			bool useCustomNodeLabel = !string.IsNullOrEmpty(nodeData.tag);
			npcLabelText.text = useCustomNodeLabel ? nodeData.tag : VD.assigned.alias;

			npcContainer.SetActive(true);
		}
	}

	private void OnDisable() {
		VD.OnNodeChange -= OnNodeChange;
		VD.OnEnd -= OnDialogueEnd;
		dialogueContainer.SetActive(false);
		VD.EndDialogue();
	}

}