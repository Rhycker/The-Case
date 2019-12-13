using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScreen : GameScreenSingleton<StoryScreen> {

	[SerializeField] private Transform storyboard;
	[SerializeField] private int gameSceneIndex;

	private GameObject[] pages;
	private int pageIndex;

	public override GameScreenState Initialize() {
		Instance = this;
		return base.Initialize();
	}

	public new void Activate() {
		base.Activate();
		pages = new GameObject[storyboard.childCount];
		for (int i = 0; i < storyboard.childCount; i++) {
			pages[i] = storyboard.GetChild(i).gameObject;
			pages[i].SetActive(false);
		}
		pages[0].SetActive(true);
	}

	public void Button_Continue() {
		pages[pageIndex].SetActive(false);
		pageIndex++;

		if (pageIndex >= pages.Length) {
			SceneManager.LoadScene(gameSceneIndex);
		}
		else {
			pages[pageIndex].SetActive(true);
		}
	}
	
}