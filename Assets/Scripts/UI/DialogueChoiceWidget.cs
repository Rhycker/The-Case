using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DialogueChoiceWidget : MonoBehaviour {

	public string DialogueText { get; private set; }

	[SerializeField] private Color highlightColour;
	[SerializeField] private Color standardColour;

	private TMP_Text text;

	public delegate void ReturnCommentText(string commentText);

	public void Initialize(string commentText, Dictionary<string, object> extraVars) {
		gameObject.SetActive(true);
		text = GetComponent<TMP_Text>();
		UpdateHightlight(false);
		
		text.text = commentText;
		DialogueText = GetDialogueText(commentText, extraVars, (newCommentText) => {
			text.text = newCommentText;
		});
	}

	public void UpdateHightlight(bool showHighlight) {
		text.color = showHighlight ? highlightColour : standardColour;
	}
	
	public static string GetDialogueText(string commentText, Dictionary<string, object> extraVars, ReturnCommentText updateCommentText = null) {
		if (commentText.StartsWith("[VAR_TEXT")) {
			string replacementTextKey = commentText.Split("]"[0])[0] + "]";
			string newCommentText = commentText.Replace(replacementTextKey, "");
			updateCommentText?.Invoke(newCommentText);
			if (extraVars.ContainsKey(replacementTextKey)) {
				return extraVars[replacementTextKey].ToString();
			}
			else {
				Debug.LogWarning("Extra var with key: " + replacementTextKey + " is not available");
			}
		}

		return commentText;
	}

}