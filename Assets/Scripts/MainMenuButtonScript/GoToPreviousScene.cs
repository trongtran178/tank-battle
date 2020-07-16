using System.IO;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{
    public class GoToPreviousScene : MonoBehaviour
    {
        public Button yourButton;
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(GoToPreviousSceneMethod);
        }

        void GoToPreviousSceneMethod()
        {
            if (File.Exists(Application.persistentDataPath + "/player.fun"))
            {
                Globals.IsNewGame = false;
                // GET PREVIOUS USER DATA
                PlayerData playerData = SaveSystem.SaveSystem.LoadPlayer();
                SceneManager.LoadScene(playerData.CurrentLevel);
            }
            else
            {
                // DO NOTHING
            }
        }
    }
}