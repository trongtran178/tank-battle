using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{ 
	public class GoToPreviousScene: MonoBehaviour
	{
		public Button yourButton;
		void Start()
		{
			Button btn = yourButton.GetComponent<Button>();
			btn.onClick.AddListener(GoToPreviousSceneMethod);
		}

		void GoToPreviousSceneMethod()
		{
			Globals.IsNewGame = false;
			SceneManager.LoadScene("Main");
		}
	}
}