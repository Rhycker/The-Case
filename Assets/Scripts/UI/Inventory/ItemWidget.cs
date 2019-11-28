using UnityEngine;
using UnityEngine.UI;

public class ItemWidget : MonoBehaviour {

	public Item Item { get { return UniqueItem.Item; } }
	public UniqueWidgetItem UniqueItem { get; private set; }

	[SerializeField] private Image iconImage;
	[SerializeField] private GameObject selectionContainer;
	[SerializeField] private GameObject warningContainer;
	[SerializeField] private float showWarningDuration;

	private float remainingShowWarningDuration;

	public void Initialize() {
		iconImage.sprite = null;
		iconImage.enabled = false;
	}

	public void ShowSelection(bool show) {
		selectionContainer.SetActive(show);
	}

	public void ShowWarning() {
		warningContainer.SetActive(true);
		remainingShowWarningDuration = showWarningDuration;
	}

	public void BindItem(Item item) {
		UniqueItem = new UniqueWidgetItem(item);
		iconImage.sprite = item.Icon;
		iconImage.enabled = true;
	}

	public void Clear() {
		UniqueItem = null;
		iconImage.sprite = null;
		iconImage.enabled = false;
	}

	private void Awake() {
		ShowSelection(false);
		warningContainer.SetActive(false);
	}

	private void Update() {
		if(remainingShowWarningDuration <= 0f) { return; }
		remainingShowWarningDuration -= Time.deltaTime;
		if(remainingShowWarningDuration <= 0f) {
			warningContainer.SetActive(false);
		}
	}

}