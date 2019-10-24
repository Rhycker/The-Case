using UnityEngine;

public class InventoryPanel : MonoBehaviour {

	[SerializeField] private ItemWidget itemWidgetTemplate;
	[SerializeField] private ItemInteractionPopup interactionPopup;

	private ItemWidget selectedItemWidget;

	public void AddItem(Item item) {
		ItemWidget newItemWidget = Instantiate(itemWidgetTemplate, itemWidgetTemplate.transform.parent);
		newItemWidget.Initialize(this, item);
		newItemWidget.gameObject.SetActive(true);
		interactionPopup.Deactivate();
	}

	public void InteractItemWidget(ItemWidget itemWidget) {
		if (selectedItemWidget == itemWidget) { return; }
		selectedItemWidget = itemWidget;
		interactionPopup.Activate(itemWidget);
	}

	private void Awake() {
		itemWidgetTemplate.gameObject.SetActive(false);
	}

	private void Update() {
		if (Input.GetKeyUp(KeyCode.Space)) {
			Item[] allItems = Resources.LoadAll<Item>("Items");
			Item item = allItems[Random.Range(0, allItems.Length)];
			Debug.Log("Add random item: " + item.name);
			AddItem(item);
		}
	}
}