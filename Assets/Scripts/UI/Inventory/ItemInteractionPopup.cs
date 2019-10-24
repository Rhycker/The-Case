using UnityEngine;

public class ItemInteractionPopup : MonoBehaviour {

	[SerializeField] private GameObject useButton;
	[SerializeField] private GameObject wearButton;

	private RectTransform rectTransform;
	private Vector2 localPosition;

	private Item item;

	public void Activate(ItemWidget itemWidget) {
		item = itemWidget.Item;

		transform.SetParent(itemWidget.transform);
		rectTransform.anchoredPosition = localPosition;
		transform.SetParent(itemWidget.transform.parent);
		transform.SetAsLastSibling();

		gameObject.SetActive(true);
		bool isWearableItem = item.ItemUse == Item.ItemUseType.Wear;
		useButton.SetActive(!isWearableItem);
		wearButton.SetActive(isWearableItem);
	}

	public void Deactivate() {
		gameObject.SetActive(false);
	}

	public void Button_Use() {
		Debug.Log("Use " + item.name);
	}

	public void Button_Wear() {
		Debug.Log("Wear " + item.name);
	}

	public void Button_Combine() {
		Debug.Log("Combine " + item.name);
	}

	public void Button_Examine() {
		Debug.Log("Examine: " + item.ExaminationText);
	}

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		localPosition = rectTransform.anchoredPosition;
		gameObject.SetActive(false);
	}

}