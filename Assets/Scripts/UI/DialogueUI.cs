using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using TMPro;
using UnityEngine.UI;
using System;

// Example: VIDEUIManager1.cs
[DefaultExecutionOrder(-1)]
public class DialogueUI : MonoBehaviour {

	private const string SAVE_GAME_NAME = "Game";

	public static DialogueUI Instance { get; private set; }

	public bool IsActive { get; private set; }

	[SerializeField] private GameObject dialogueContainer;
	[Space]
	[SerializeField] private GameObject playerContainer;
	[SerializeField] private DialogueChoiceWidget playerChoiceWidgetTemplate;
	[SerializeField] private Image playerImage;
	[SerializeField] private TMP_Text playerLabelText;
	[Space]
	[SerializeField] private GameObject npcContainer;
	[SerializeField] private Image npcImage;
	[SerializeField] private TMP_Text npcText;
	[SerializeField] private TMP_Text npcLabelText;

	private VIDE_Assign currentDialogue;
	private List<DialogueChoiceWidget> currentChoiceWidgets;

	private Transform playerTransform;
	private string playerDisplayName;

	private Action onDialogueFinished;

	public void SetPlayerDetails(Transform playerTransform, string displayName) {
		this.playerTransform = playerTransform;
		playerDisplayName = displayName;
	}

	public void StartDialogue(VIDE_Assign dialogue, Action onDialogueFinishedCallback = null) {
		if (VD.isActive) { return; }

		playerLabelText.text = "";
		npcText.text = "";
		npcLabelText.text = "";

		currentDialogue = dialogue;
		onDialogueFinished = onDialogueFinishedCallback;
		IsActive = true;
		VD.BeginDialogue(dialogue);

		dialogueContainer.SetActive(true);
	}

	public void StopDialogue(VIDE_Assign dialogue) {
		if (!VD.isActive) { return; }
		if(currentDialogue != dialogue) { return; }
		StopDialogue();
	}

	private void Awake() {
		Instance = this;

		//VD.LoadDialogues();
#if !UNITY_EDITOR
		VD.LoadState(SAVE_GAME_NAME, true);
#endif

		currentChoiceWidgets = new List<DialogueChoiceWidget>();
		dialogueContainer.SetActive(false);
		playerChoiceWidgetTemplate.gameObject.SetActive(false);
	}

	private void Update() {
		if (!VD.isActive) { return; }
		VD.NodeData nodeData = VD.nodeData;
		if (nodeData.pausedAction) { return; }

		bool playerChoiceIsShowing = nodeData.isPlayer && nodeData.comments.Length > 1;
		if (playerChoiceIsShowing) {
			UpdateCommentChoice(nodeData);
		}

		if (GameInput.Instance.Service.InteractButtonDown()) {
			VD.Next();
		}
	}

	// Make sure that IsActive is set after the normal update loop, so that we wont have any other interaction in this frame
	private void LateUpdate() {
		if(IsActive && !dialogueContainer.activeInHierarchy) {
			IsActive = false;
		}
	}

	private void UpdateCommentChoice(VD.NodeData nodeData) {
		if (GameInput.Instance.Service.PreviousChoiceButtonDown()) {
			if (nodeData.commentIndex == 0) {
				nodeData.commentIndex = currentChoiceWidgets.Count - 1;
			}
			else {
				nodeData.commentIndex--;
			}
			UpdateChoiceVisuals(nodeData.commentIndex);
		}
		else if (GameInput.Instance.Service.NextChoiceButtonDown()) {
			if (nodeData.commentIndex == currentChoiceWidgets.Count - 1) {
				nodeData.commentIndex = 0;
			}
			else {
				nodeData.commentIndex++;
			}
			UpdateChoiceVisuals(nodeData.commentIndex);
		}
	}

	private void UpdateChoiceVisuals(int commentIndex) {
		for (int i = 0; i < currentChoiceWidgets.Count; i++) {
			if (i == commentIndex) {
				currentChoiceWidgets[i].UpdateHighlight(true);
			}
			else {
				currentChoiceWidgets[i].UpdateHighlight(false);
			}
		}
	}

	private void StopDialogue() {
		TextBalloonUI.Instance.DeactivateUI();
		onDialogueFinished?.Invoke();
		dialogueContainer.SetActive(false);
		VD.EndDialogue();
		VD.SaveState(SAVE_GAME_NAME, true);
	}

	private void OnDialogueEnd(VD.NodeData nodeData) {
		StopDialogue();
	}

	private void OnNodeChange(VD.NodeData nodeData) {
		// Reset some variables and previous player choices
		foreach (DialogueChoiceWidget choice in currentChoiceWidgets) {
			Destroy(choice.gameObject);
		}
		currentChoiceWidgets.Clear();
		playerContainer.SetActive(false);
		npcContainer.SetActive(false);

		// Set the UI according to the new node data
		if (nodeData.isPlayer) {
			bool useCustomNodeSprite = nodeData.sprite != null;
			playerImage.sprite = useCustomNodeSprite ? nodeData.sprite : VD.assigned.defaultPlayerSprite;

			for (int i = 0; i < nodeData.comments.Length; i++) {
				DialogueChoiceWidget newChoiceWidget = Instantiate(playerChoiceWidgetTemplate, playerChoiceWidgetTemplate.transform.position, Quaternion.identity, playerChoiceWidgetTemplate.transform.parent);
				newChoiceWidget.Initialize(nodeData.comments[i], nodeData.extraVars);
				currentChoiceWidgets.Add(newChoiceWidget);
			}
			if (nodeData.comments.Length > 1) {
				UpdateChoiceVisuals(nodeData.commentIndex);
			}
			else {
				TextBalloonUI.Instance.ShowText(playerTransform.position, nodeData.comments[nodeData.commentIndex]);
				//playerText.text = nodeData.comments[nodeData.commentIndex];
			}

			bool useCustomNodeLabel = !string.IsNullOrEmpty(nodeData.tag);
			playerLabelText.text = useCustomNodeLabel ? nodeData.tag : playerDisplayName;

			playerContainer.SetActive(true);
		}
		else {
			bool useCustomNodeSprite = nodeData.sprite != null;
			npcImage.sprite = useCustomNodeSprite ? nodeData.sprite : VD.assigned.defaultNPCSprite;

			TextBalloonUI.Instance.ShowText(currentDialogue.transform.position, nodeData.comments[nodeData.commentIndex], true);
			//npcText.text = nodeData.comments[nodeData.commentIndex];

			bool useCustomNodeLabel = !string.IsNullOrEmpty(nodeData.tag);
			npcLabelText.text = useCustomNodeLabel ? nodeData.tag : VD.assigned.alias;

			npcContainer.SetActive(true);
		}
	}

	private void OnEnable() {
		VD.OnNodeChange += OnNodeChange;
		VD.OnEnd += OnDialogueEnd;
	}

	private void OnDisable() {
		VD.OnNodeChange -= OnNodeChange;
		VD.OnEnd -= OnDialogueEnd;
		onDialogueFinished = null;
		IsActive = false;
		dialogueContainer.SetActive(false);
		VD.EndDialogue();
	}

#if UNITY_EDITOR
	public void Editor_SetUIActive(bool active) {
		if(dialogueContainer == null) { return; }
		dialogueContainer.SetActive(active);
	}
#endif

}