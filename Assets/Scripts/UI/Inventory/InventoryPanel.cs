using UnityEngine;

public class InventoryPanel : MonoBehaviour {

	public static InventoryPanel Instance;

	[SerializeField] private ItemWidget itemWidgetTemplate;
	[SerializeField] private ItemInteractionPopup interactionPopup;

	private ItemWidget selectedItemWidget;
	private bool isCombining;

	public void Activate(bool active) {
		gameObject.SetActive(active);
		if (!active) {
			selectedItemWidget = null;
			isCombining = false;
			interactionPopup.Deactivate();
		}
	}

	public ItemWidget AddItemWidget(Item item) {
		ItemWidget newItemWidget = Instantiate(itemWidgetTemplate, itemWidgetTemplate.transform.parent);
		newItemWidget.Initialize(this, item);
		newItemWidget.gameObject.SetActive(true);
		interactionPopup.transform.SetAsLastSibling();
		return newItemWidget;
	}

	public void StartCombining() {
		isCombining = true;
		interactionPopup.Deactivate();
	}

	public void InteractItemWidget(ItemWidget itemWidget) {
		if (selectedItemWidget == itemWidget) { return; }
		if (!isCombining) {
			selectedItemWidget = itemWidget;
			interactionPopup.Activate(itemWidget);
		}
		else {
			isCombining = false;
			Item combinedItem = selectedItemWidget.Item.Combine(itemWidget.Item);
			if(combinedItem != null) {
				Debug.Log("Combining success!");
				Destroy(selectedItemWidget);
				Destroy(itemWidget);
				selectedItemWidget = AddItemWidget(combinedItem);
				
				interactionPopup.Activate(selectedItemWidget);
			}
			else {
				Debug.Log("Can't combine " + selectedItemWidget.Item.name + " with " + itemWidget.Item);
			}
		}
	}

	public void Button_Close() {
		Activate(false);
	}

	private void Awake() {
		itemWidgetTemplate.gameObject.SetActive(false);
		Instance = this;
		Activate(false);
	}

	private void Update() {
		if (Input.GetKeyUp(KeyCode.Space)) {
			Item[] allItems = Resources.LoadAll<Item>("Items");
			Item item = allItems[Random.Range(0, allItems.Length)];
			Debug.Log("Add random item: " + item.name);
			AddItemWidget(item);
		}
	}
}