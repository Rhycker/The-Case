public class ControllerScreen : GameScreenSingleton<ControllerScreen> {

	public override GameScreenState Initialize() {
		Instance = this;
		return base.Initialize();
	}

	public new void Activate() {
		base.Activate();
	}
	
	public void Button_Continue() {

	}

}