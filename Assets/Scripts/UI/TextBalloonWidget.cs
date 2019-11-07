using UnityEngine;
using UnityEngine.UI;

public class TextBalloonWidget : MonoBehaviour {
	
	[SerializeField] private Text balloonText;

	private int characterLimit;
	private Vector3 balloonTextScale;

	private void Awake() {
		characterLimit = balloonText.text.Length;
		balloonTextScale = balloonText.transform.localScale;
	}

	public bool ShowText(Vector3 position, string text, bool flipBalloonX) {
		if(text.Length > characterLimit) { return false; }

		transform.position = position;
		balloonText.text = text;

		int flipX = flipBalloonX ? -1 : 1;
		transform.localScale = new Vector3(flipX, 1f, 1f);
		balloonText.transform.localScale = new Vector3(flipX, 1f, 1f);

		return true;
	}

}