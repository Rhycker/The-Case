public interface IInput {

	float Horizontal();
	float Vertical();
	bool InventoryButtonDown();
	bool BackButtonDown();
	bool InteractButtonDown();
	bool PreviousChoiceButtonDown();
	bool NextChoiceButtonDown();

}