public interface IInput {

	float Horizontal();
	float Vertical();
	float InventoryHorizontal();
	float InventoryVertical();
	bool InventoryButtonDown();
	bool BackButtonDown();
	bool InteractButtonDown();
	bool PreviousChoiceButtonDown();
	bool NextChoiceButtonDown();

}