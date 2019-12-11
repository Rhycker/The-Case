using UnityEngine;

public class StartScreen : GameScreenSingleton<StartScreen> {

	public override GameScreenState Initialize() {
		Instance = this;
		return base.Initialize();
	}

	public new void Activate() {
		base.Activate();
	}

	public void Button_GoToNextScreen() {
		ControllerScreen.Instance.Activate();
	}
	
}