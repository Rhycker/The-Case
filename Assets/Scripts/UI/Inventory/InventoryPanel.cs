using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {

	public bool IsActive { get { return gameObject.activeInHierarchy; } }
	public int MaxItemCount { get; private set; }

	[SerializeField] private ItemInteractionPopup interactionPopup;
	[SerializeField] private GameObject selectionIndicatorContainer;
	[SerializeField] private GameObject combineIndicatorContainer;
	[SerializeField] private float minSelectionSwitchTime;

	private int itemCount { get { return sortedItems.Count; } }
	private List<ItemWidget> itemWidgets;
	private List<UniqueWidgetItem> sortedItems;
	private ItemWidget topItemWidget { get { return itemWidgets[0]; } }

	private UniqueWidgetItem selectedCombineItem;
	private bool isCombining { get { return selectedCombineItem != null; } }

	private bool selectLeftDown;
	private bool selectRightDown;
	private float selectionTime;

	public void Toggle() {
		if (IsActive) {
			gameObject.SetActive(false);
			UpdateInteractionCombineState();
			interactionPopup.Deactivate();
		}
		else {
			gameObject.SetActive(true);
		}
	}

	public void AddItemWidget(Item item) {
		UniqueWidgetItem uniqueItem = new UniqueWidgetItem(item);
		sortedItems.Insert(0, uniqueItem);
		UpdateItemWidgets();
		UpdateInteractionCombineState();
	}

	public void StartCombining() {
		interactionPopup.Deactivate();
		UpdateInteractionCombineState(topItemWidget.UniqueItem);
	}

	public void InteractItemWidget(ItemWidget itemWidget) {
		if (itemWidget.Item == null) { return; }
		if (selectedCombineItem == itemWidget.UniqueItem) {
			UpdateInteractionCombineState(null);

			if (interactionPopup.gameObject.activeInHierarchy) {
				interactionPopup.Deactivate();
			}
			else {
				interactionPopup.Activate(itemWidget);
			}
			return;
		}

		if (!isCombining) {
			OrganizeItemWidgets(itemWidget.UniqueItem);
			interactionPopup.Activate(topItemWidget);
		}
		else {
			Item itemA = selectedCombineItem.Item;
			Item itemB = itemWidget.Item;
			Item combinedItem = itemA.Combine(itemB);
			if(combinedItem != null) {
				Inventory.Instance.RemoveItem(itemA);
				Inventory.Instance.RemoveItem(itemB);
				Inventory.Instance.AddItem(combinedItem);
			}

			UpdateInteractionCombineState();
			if(combinedItem == null) {
				itemWidget.ShowWarning();
			}
		}
	}

	public void Button_InteractItemWidget(ItemWidget itemWidget) {
		InteractItemWidget(itemWidget);
	}

	public void RemoveItemWidget(Item item) {
		int widgetItemIndex = 0;
		for(int i = 0; i < sortedItems.Count; i ++) {
			if(sortedItems[i].Item == item) {
				widgetItemIndex = i;
				break;
			}
		}
		sortedItems.RemoveAt(widgetItemIndex);
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

		selectionIndicatorContainer.SetActive(false);
		combineIndicatorContainer.SetActive(false);

		if (IsActive) {
			Toggle();
		}
	}
	
	private void Update() {
		if (itemCount == 0) { return; }
		if (GameInput.Instance.Service.InteractButtonDown()) {
			InteractItemWidget(topItemWidget);
		}

		float horizontalInput = GameInput.Instance.Service.InventoryHorizontal();
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

	private void UpdateInteractionCombineState(UniqueWidgetItem item = null) {
		selectedCombineItem = item;
		selectionIndicatorContainer.SetActive(!isCombining && itemCount > 0);
		combineIndicatorContainer.SetActive(isCombining);
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
		
		UpdateItemWidgets();
	}

	private void UpdateItemWidgets() {
		foreach (ItemWidget itemWidget in itemWidgets) {
			itemWidget.Clear();
		}

		if (itemCount >= 1) {
			itemWidgets[0].BindItem(sortedItems[0]);
		}
		else {
			selectionIndicatorContainer.SetActive(false);
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