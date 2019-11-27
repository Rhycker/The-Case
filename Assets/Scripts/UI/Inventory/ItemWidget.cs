using UnityEngine;
using UnityEngine.UI;

public class ItemWidget : MonoBehaviour {

	public Item Item { get; private set; }

	[SerializeField] private Image iconImage;
	[SerializeField] private GameObject selectionContainer;
	[SerializeField] private GameObject warningContainer;

	public void Initialize() {
		iconImage.sprite = null;
		iconImage.enabled = false;
	}

	public void ShowSelection(bool show) {
		selectionContainer.SetActive(show);
	}

	public void ShowWarning(bool show) {
		warningContainer.SetActive(show);
	}

	public void BindItem(Item item) {
		Item = item;
		iconImage.sprite = item.Icon;
		iconImage.enabled = true;
	}

	public void Clear() {
		Item = null;
		iconImage.sprite = null;
		iconImage.enabled = false;
	}

	private void Awake() {
		ShowSelection(false);
		ShowWarning(false);
	}

}