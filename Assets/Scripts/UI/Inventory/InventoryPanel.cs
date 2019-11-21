using UnityEngine;

public class InventoryPanel : MonoBehaviour {

	[SerializeField] private ItemWidget itemWidgetTemplate;
	[SerializeField] private ItemInteractionPopup interactionPopup;

	private ItemWidget selectedItemWidget;
	private bool isCombining;

	public void Toggle() {
		if (gameObject.activeInHierarchy) {
			gameObject.SetActive(false);
			isCombining = false;
			selectedItemWidget = null;
			interactionPopup.Deactivate();
		}
		else {
			gameObject.SetActive(true);
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
			bool combiningSucceeded = Inventory.Instance.CombineItems(selectedItemWidget.Item, itemWidget.Item);
			if(combiningSucceeded) {
				Destroy(selectedItemWidget.gameObject);
				Destroy(itemWidget.gameObject);				
			}
			else {
				selectedItemWidget = null;
			}
		}
	}

	private void Start() {
		itemWidgetTemplate.gameObject.SetActive(false);
		if (gameObject.activeInHierarchy) {
			Toggle();
		}
	}

}