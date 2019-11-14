using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasUI : MonoBehaviour {

	private Canvas canvas;
	
	public void ShowCanvas(bool show) {
		if (canvas == null) {
			canvas = GetComponent<Canvas>();
		}
		canvas.enabled = show;
	}

	protected virtual void Awake() {
		ShowCanvas(true);
    }
	
}