using System.IO;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.MainMenuButtonScript
{
    public class QuitGame : MonoBehaviour
    {
        public Button yourButton;
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(QuitGameScript);
        }

        void QuitGameScript()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}