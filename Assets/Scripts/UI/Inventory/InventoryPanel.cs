using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {
    [SerializeField] private AudioClip scrollSound;

    private enum ScrollState {
		Right = -1,
		None,
		Left = 1
	}

	public bool IsActive { get { return gameObject.activeInHierarchy; } }
	public int MaxItemCount { get; private set; }

	[SerializeField] private ItemInteractionPopup interactionPopup;
	[SerializeField] private GameObject selectionIndicatorContainer;
	[SerializeField] private GameObject warningIndicatorContainer;
	[SerializeField] private GameObject combineIndicatorContainer;
	[SerializeField] private float minSelectionSwitchTime;
	[SerializeField] private float warningShowDuration;

	private int itemCount { get { return sortedItems.Count; } }
	private List<ItemWidget> itemWidgets;
	private List<UniqueWidgetItem> sortedItems;
	private ItemWidget topItemWidget { get { return itemWidgets[0]; } }

	private UniqueWidgetItem selectedCombineItem;
	private bool isCombining { get { return selectedCombineItem != null; } }
	
	private ScrollState startScrollState;
	private ScrollState currentScrollState;
	private float selectionTime;

	private bool isShowingWarning;

	public void Toggle() {
		if (IsActive) {
			gameObject.SetActive(false);
			UpdateInteractionCombineState();
			interactionPopup.Deactivate();
			warningIndicatorContainer.SetActive(false);
			isShowingWarning = false;
		}
		else {
			gameObject.SetActive(true);
			startScrollState = GetCurrentScrollState();
		}
	}

	public void AddItemWidget(Item item) {
		UniqueWidgetItem uniqueItem = new UniqueWidgetItem(item);
		sortedItems.Insert(0, uniqueItem);
		UpdateItemWidgets();
		UpdateInteractionCombineState();
	}

	public void ShowWarning(bool show) {
		if(show && isShowingWarning) { return; }

		warningIndicatorContainer.SetActive(show);
		if (show) {
			CoroutineHelper.WaitForSeconds(warningShowDuration, () => {
				warningIndicatorContainer.SetActive(false);
				isShowingWarning = false;
			});
		}

		isShowingWarning = show;
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

		ScrollState newScrollState = GetCurrentScrollState();
		if(startScrollState != ScrollState.None && startScrollState == newScrollState) {
			return;
		}
		else {
			startScrollState = ScrollState.None;
		}

		if(newScrollState == ScrollState.None) {
			currentScrollState = ScrollState.None;
			return;
		}

		if(newScrollState != currentScrollState) {
			SelectItemWidget((int)newScrollState);
			currentScrollState = newScrollState;
			return;
		}

		if ((Time.time - selectionTime) > minSelectionSwitchTime) {
			SelectItemWidget((int)newScrollState);
		}
	}

	private void UpdateInteractionCombineState(UniqueWidgetItem item = null) {
		selectedCombineItem = item;
		selectionIndicatorContainer.SetActive(!isCombining && itemCount > 0);
		combineIndicatorContainer.SetActive(isCombining);
	}

	private void SelectItemWidget(int indexShift) {
        //Raf scroll sound 
        SoundManager.Instance.PlaySound(scrollSound);
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

	private ScrollState GetCurrentScrollState() {
		float horizontal = GameInput.Instance.Service.InventoryHorizontal();
		if (horizontal < -0.3f) {
            return ScrollState.Right;
		}
		else if (horizontal > 0.3f) {
            return ScrollState.Left;
		}
		return ScrollState.None;

	}
}