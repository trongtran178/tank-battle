using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{
	public class GoToMultiplayerScene : MonoBehaviour
	{
		public Button button;
		void Start()
		{
			Button btn = button.GetComponent<Button>();
			btn.onClick.AddListener(GoToMultiplayerSceneScript);
		}

		void GoToMultiplayerSceneScript()
		{
			Globals.IsNewGame = true;
			SceneManager.LoadScene("Main2");
		}
	}
}