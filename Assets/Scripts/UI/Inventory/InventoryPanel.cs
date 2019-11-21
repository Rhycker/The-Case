using UnityEngine;

public class InventoryPanel : MonoBehaviour {

	public int MaxItemCount { get { return itemWidgets.Length; } }
	[SerializeField] private ItemInteractionPopup interactionPopup;

	private ItemWidget[] itemWidgets;
	private int itemCount;
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
			selectedItemWidget = itemWidgets[0];
			gameObject.SetActive(true);
		}
	}

	public void AddItemWidget(Item item) {
		ItemWidget nextItemWidget = itemWidgets[itemCount];
		nextItemWidget.BindItem(item);
		selectedItemWidget = itemWidgets[0];
		itemCount++;
		interactionPopup.transform.SetAsLastSibling();
	}

	public void StartCombining() {
		isCombining = true;
		interactionPopup.Deactivate();
	}

	public void InteractItemWidget(ItemWidget itemWidget) {
		if (itemWidget.Item == null) { return; }
		if (selectedItemWidget == itemWidget) {
			if (interactionPopup.gameObject.activeInHierarchy) {
				interactionPopup.Deactivate();
			}
			else {
				interactionPopup.Activate(itemWidget);
			}
			return;
		}

		if (!isCombining) {
			selectedItemWidget = itemWidget;
			interactionPopup.Activate(itemWidget);
		}
		else {
			isCombining = false;
			Item itemA = selectedItemWidget.Item;
			Item itemB = itemWidget.Item;
			Item combinedItem = itemA.Combine(itemB);
			if(combinedItem != null) {
				selectedItemWidget.Clear();
				itemWidget.Clear();
				Inventory.Instance.CombineItems(itemA, itemB, combinedItem);
			}
			else {
				selectedItemWidget = null;
			}
		}
	}

	public void Button_InteractItemWidget(ItemWidget itemWidget) {
		InteractItemWidget(itemWidget);
	}

	private void Awake() {
		itemWidgets = GetComponentsInChildren<ItemWidget>();
		foreach(ItemWidget itemWidget in itemWidgets) {
			itemWidget.Initialize();
		}
		interactionPopup.Initialize(this);
		if (gameObject.activeInHierarchy) {
			Toggle();
		}
	}
	
	private void Update() {
		if (GameInput.Instance.Service.InteractButtonDown()) {
			InteractItemWidget(selectedItemWidget);
		}
	}

}