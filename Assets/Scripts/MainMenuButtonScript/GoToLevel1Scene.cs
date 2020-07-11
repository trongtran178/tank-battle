using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{
	public class GoToLevel1Scene: MonoBehaviour
	{
		public Button button;
		void Start()
		{
			Button btn = button.GetComponent<Button>();
			btn.onClick.AddListener(GoToLevel1);
		}

		void GoToLevel1()
		{
			Globals.IsNewGame = true;
			SceneManager.LoadScene("Main");
		}
	}
}