using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DialogueChoiceWidget : MonoBehaviour {
	
	[SerializeField] private Color highlightColour;
	[SerializeField] private Color standardColour;

	private TMP_Text text;

	public delegate void ReturnCommentText(string commentText);

	public void Initialize(string commentText, Dictionary<string, object> extraVars) {
		text = GetComponent<TMP_Text>();
		UpdateHighlight(false);
		
		text.text = commentText;
	}

	public void UpdateHighlight(bool showHighlight) {
		text.color = showHighlight ? highlightColour : standardColour;
	}

}