using UnityEngine;

public class TextBalloonUI : MonoBehaviour {

	public static TextBalloonUI Instance { get; private set; }

	[SerializeField] private float characterOffset;

	private TextBalloonWidget[] textBalloonWidgets;

	private void Awake() {
		Instance = this;
		textBalloonWidgets = GetComponentsInChildren<TextBalloonWidget>(true);
		DeactivateUI();
	}

	public void ShowText(Vector3 characterPosition, string text, bool flipBalloonX = false) {
		Vector3 position = characterPosition + Vector3.up * characterOffset;
		bool success = false;
		foreach(TextBalloonWidget textBalloonWidget in textBalloonWidgets) {
			textBalloonWidget.gameObject.SetActive(false);
			if (success) { continue; }

			success = textBalloonWidget.ShowText(position, text, flipBalloonX);
			textBalloonWidget.gameObject.SetActive(success);
		}

		if (!success) {
			Debug.LogWarning("Could not show text: '" + text + "' of length: " + text.Length);
		}
	}

	public void DeactivateUI() {
		foreach (TextBalloonWidget widget in textBalloonWidgets) {
			widget.gameObject.SetActive(false);
		}
	}

}