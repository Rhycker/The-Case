using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryPanel : MonoBehaviour {

	public bool IsActive { get { return gameObject.activeInHierarchy; } }
	public int MaxItemCount { get; private set; }

	[SerializeField] private ItemInteractionPopup interactionPopup;
	[SerializeField] private float minSelectionSwitchTime;
	
	private int itemCount { get { return widgetsWithItem.Count; } }
	private List<ItemWidget> itemWidgets;
	private List<ItemWidget> widgetsWithItem;
	private ItemWidget selectedItemWidget { get { return widgetsWithItem[0]; } }
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

	public void AddItemWidget(Item item) {
		int nextWidgetIndex = itemCount;
		if(nextWidgetIndex > 1) {
			if(itemCount % 2 == 0) {
				for(int i = MaxItemCount - 1; i > 1; i--) {
					if(itemWidgets[i].Item == null) {
						nextWidgetIndex = i;
						break;
					}
				}
			}
			else {
				for (int i = 2; i < MaxItemCount; i++) {
					if (itemWidgets[i].Item == null) {
						nextWidgetIndex = i;
						break;
					}
				}
			}
		}

		ItemWidget nextItemWidget = itemWidgets[nextWidgetIndex];
		nextItemWidget.BindItem(item);
		widgetsWithItem.Insert(0, nextItemWidget);
		
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
			OrganizeItemWidgets(itemWidget);
			interactionPopup.Activate(selectedItemWidget);
		}
		else {
			isCombining = false;
			Item itemA = selectedItemWidget.Item;
			Item itemB = itemWidget.Item;
			Item combinedItem = itemA.Combine(itemB);
			if(combinedItem != null) {
				selectedItemWidget.Clear();
				itemWidget.Clear();
				widgetsWithItem.Remove(itemWidget);
				widgetsWithItem.Remove(selectedItemWidget);
				Inventory.Instance.CombineItems(itemA, itemB, combinedItem);
			}
		}
	}

	public void Button_InteractItemWidget(ItemWidget itemWidget) {
		RemoveItem(itemWidget);
		//InteractItemWidget(itemWidget);
	}
	private void RemoveItem(ItemWidget itemWidget) {
		itemWidget.Clear();
		widgetsWithItem.Remove(itemWidget);
		OrganizeItemWidgets();
	}

	private void Awake() {
		itemWidgets = GetComponentsInChildren<ItemWidget>().ToList();
		MaxItemCount = itemWidgets.Count;
		widgetsWithItem = new List<ItemWidget>();
		foreach (ItemWidget itemWidget in itemWidgets) {
			itemWidget.Initialize();
		}
		interactionPopup.Initialize(this);
		if (IsActive) {
			Toggle();
		}
	}
	
	private void Update() {
		if (GameInput.Instance.Service.InteractButtonDown()) {
			InteractItemWidget(selectedItemWidget);
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
				SelectItemWidget(1);
			}
		}
		else {
			if (!selectLeftDown) {
				selectRightDown = false;
				selectLeftDown = true;
				SelectItemWidget(-1);
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
			targetIndex = itemCount - indexShift;
		}

		ItemWidget selectionTargetWidget = widgetsWithItem.GetAtIndex(targetIndex, true);
		Debug.Log("Select item widget: " + targetIndex + ", for item shift: " + indexShift);
		OrganizeItemWidgets(selectionTargetWidget);
	}

	private void OrganizeItemWidgets(ItemWidget firstWidget = null) {
		Debug.Log("Organize widgets: " + firstWidget, firstWidget);
		if(firstWidget != null) {
			List<ItemWidget> sortedWidgetsWithItem = new List<ItemWidget>();
			int firstWidgetIndex = widgetsWithItem.IndexOf(firstWidget);

			for(int i = firstWidgetIndex; i < itemCount; i++) {
				sortedWidgetsWithItem.Add(widgetsWithItem[i]);
			}
			sortedWidgetsWithItem.Add(widgetsWithItem[0]);
			for (int i = 1; i < firstWidgetIndex; i++) {
				sortedWidgetsWithItem.Add(widgetsWithItem[i]);
			}

			widgetsWithItem = sortedWidgetsWithItem;
		}

		List<Item> sortedItems = new List<Item>();
		foreach(ItemWidget itemWidget in widgetsWithItem) {
			sortedItems.Add(itemWidget.Item);
			itemWidget.Clear();
		}
		widgetsWithItem.Clear();
		
		foreach(Item item in sortedItems) {
			AddItemWidget(item);
		}
	}

}