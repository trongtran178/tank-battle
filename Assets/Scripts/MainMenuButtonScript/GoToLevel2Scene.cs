using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{
    public class GoToLevel2Scene: MonoBehaviour
	{
		public Button button;
		void Start()
		{
			Button btn = button.GetComponent<Button>();
			btn.onClick.AddListener(GoToLevel2);
		}

		void GoToLevel2()
		{
			Globals.IsNewGame = true;
			SceneManager.LoadScene("Main");
		}
	}
}
