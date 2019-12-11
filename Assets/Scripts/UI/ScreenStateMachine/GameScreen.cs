using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class GameScreen : MonoBehaviour {

	// Class that is only accessed by the ScreenStateMachine
	public class GameScreenState {
		private GameScreen instance;
		public GameScreenState(GameScreen instance) {
			this.instance = instance;
		}

		public bool IsShowing { get { return instance.IsShowing; } }

		public void Show() {
			instance.Show();
		}

		public void Hide() {
			instance.Hide();
		}

		public bool IsBlockingInteraction(Vector2 screenPosition) {
			if (!instance.isInteractable) { return false; }
			return instance.IsInRectTransform(screenPosition);
		}
	}

	public bool IsShowing { get; private set; }

	[SerializeField] private bool isInteractable;
	[SerializeField] private Button activeStartButton;

	private RectTransform rectTransform;

	private GameScreenState stateInstance;
	private ScreenStateMachine screenStateMachine;

	public void Deactivate() {
		screenStateMachine.DeactivateScreen(stateInstance);
	}

	public virtual GameScreenState Initialize() {
		rectTransform = GetComponent<RectTransform>();

		screenStateMachine = transform.GetComponentInParent<ScreenStateMachine>();
		gameObject.SetActive(false);

		stateInstance = new GameScreenState(this);
		return stateInstance;
	}

	protected void Activate() {
		screenStateMachine.ActivateScreen(stateInstance);
	}

	protected void Show() {
		gameObject.SetActive(true);
		IsShowing = true;
		activeStartButton.Select();
	}

	protected virtual void Hide() {
		gameObject.SetActive(false);
		IsShowing = false;
	}

	private bool IsInRectTransform(Vector2 screenPosition) {
		Vector2 localPosition = rectTransform.InverseTransformPoint(screenPosition);
		return rectTransform.rect.Contains(localPosition);
	}

}