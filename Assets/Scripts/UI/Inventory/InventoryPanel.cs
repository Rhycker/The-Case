using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryPanel : MonoBehaviour {

	public bool IsActive { get { return gameObject.activeInHierarchy; } }
	public int MaxItemCount { get; private set; }

	[SerializeField] private ItemInteractionPopup interactionPopup;
	[SerializeField] private float minSelectionSwitchTime;

	private int itemCount { get { return sortedItems.Count; } }
	private List<ItemWidget> itemWidgets;
	public List<UniqueWidgetItem> sortedItems;//QQ
	private ItemWidget topItemWidget { get { return itemWidgets[0]; } }
	private bool isCombining;

	private bool selectLeftDown;
	private bool selectRightDown;
	private float selectionTime;

	public void Toggle() {
		if (IsActive) {
			gameObject.SetActive(false);
			isCombining = false;
			interactionPopup.Deactivate();
		}
		else {
			gameObject.SetActive(true);
		}
	}

	public void AddItemWidget(Item item, bool isNewItem = true) {
		if (isNewItem) {
			UniqueWidgetItem uniqueItem = new UniqueWidgetItem(item);
			sortedItems.Insert(0, uniqueItem);
		}
		UpdateItemWidgets();
	}

	public void StartCombining() {
		isCombining = true;
		interactionPopup.Deactivate();
	}

	public void InteractItemWidget(ItemWidget itemWidget) {
		if (itemWidget.Item == null) { return; }
		if (topItemWidget == itemWidget) {
			if (interactionPopup.gameObject.activeInHierarchy) {
				interactionPopup.Deactivate();
			}
			else {
				interactionPopup.Activate(itemWidget);
			}
			return;
		}

		if (!isCombining) {
			isCombining = true;
			OrganizeItemWidgets(itemWidget.UniqueItem);
			interactionPopup.Activate(topItemWidget);
		}
		else {
			isCombining = false;
			Item itemA = topItemWidget.Item;
			Item itemB = itemWidget.Item;
			Item combinedItem = itemA.Combine(itemB);
			if(combinedItem != null) {
				sortedItems.Remove(topItemWidget.UniqueItem);
				sortedItems.Remove(itemWidget.UniqueItem);
				Inventory.Instance.CombineItems(itemA, itemB, combinedItem);
			}
			else {
				itemWidget.ShowWarning();
			}
		}
	}

	public void Button_InteractItemWidget(ItemWidget itemWidget) {
		InteractItemWidget(itemWidget);
	}

	private void RemoveItem(ItemWidget itemWidget) {
		sortedItems.Remove(itemWidget.UniqueItem);
		UpdateItemWidgets();
	}

	private void Awake() {
		itemWidgets = GetComponentsInChildren<ItemWidget>().ToList();
		foreach (ItemWidget itemWidget in itemWidgets) {
			itemWidget.Initialize();
		}
		MaxItemCount = itemWidgets.Count;
		sortedItems = new List<UniqueWidgetItem>();

		interactionPopup.Initialize(this);
		interactionPopup.transform.SetAsLastSibling();

		if (IsActive) {
			Toggle();
		}
	}
	
	private void Update() {
		if (itemCount == 0) { return; }
		if (GameInput.Instance.Service.InteractButtonDown()) {
			InteractItemWidget(topItemWidget);
		}

		float horizontalInput = GameInput.Instance.Service.Horizontal();
		if (horizontalInput < -0.3f) {
			UpdateSelection(false);
		}
		else if(horizontalInput > 0.3f) {
			UpdateSelection(true);
		}
		else if(horizontalInput == 0f) {
			selectLeftDown = false;
			selectRightDown = false;
		}
	}

	private void UpdateSelection(bool isRight) {
		if (isRight) {
			if (!selectRightDown) {
				selectLeftDown = false;
				selectRightDown = true;
				SelectItemWidget(-1);
			}
		}
		else {
			if (!selectLeftDown) {
				selectRightDown = false;
				selectLeftDown = true;
				SelectItemWidget(1);
			}
		}

		if((Time.time - selectionTime) > minSelectionSwitchTime) {
			int indexShift = isRight ? 1 : -1;
			SelectItemWidget(indexShift);
		}
	}

	private void SelectItemWidget(int indexShift) {
		if (itemCount <= 1) { return; }

		selectionTime = Time.time;
		int targetIndex = indexShift;
		if (indexShift < 0) {
			targetIndex = itemCount + indexShift;
		}

		UniqueWidgetItem targetItem = sortedItems.GetAtIndex(targetIndex, true);
		OrganizeItemWidgets(targetItem);
	}
	
	private void OrganizeItemWidgets(UniqueWidgetItem firstWidgetItem) {
		if (itemCount < 2) { return; }
		int sortedItemTargetIndex = sortedItems.IndexOf(firstWidgetItem);

		// Sort widgets so firstWidget is actually the first widget of the list
		List<UniqueWidgetItem> newSortedItems = new List<UniqueWidgetItem>();
		for(int i = sortedItemTargetIndex; i < sortedItems.Count; i++) {
			newSortedItems.Add(sortedItems[i]);
		}
		for(int i = 0; i < sortedItemTargetIndex; i++) {
			newSortedItems.Add(sortedItems[i]);
		}
		sortedItems = newSortedItems;
		
		// Add the items the new order
		UpdateItemWidgets();
	}

	private void UpdateItemWidgets() {
		foreach (ItemWidget itemWidget in itemWidgets) {
			itemWidget.Clear();
		}

		if (itemCount >= 1) {
			itemWidgets[0].BindItem(sortedItems[0]);
			topItemWidget.ShowSelection(true);
		}
		else {
			topItemWidget.ShowSelection(false);
		}
		if (itemCount >= 2) {
			itemWidgets[1].BindItem(sortedItems[1]);
		}
		if (itemCount >= 3) {
			int lastItemIndex = itemCount - 1;
			itemWidgets[2].BindItem(sortedItems[lastItemIndex]);
		}
	}

}