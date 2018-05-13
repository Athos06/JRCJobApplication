using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JobApplicationGame
{
    public class MainMenu : MenuState
    {
        override public void Open()
        {
            base.Open();
        }

        public void OnPlayButton()
        {
            MenuManager.Instance.PopAll();
            SceneController.Instance.FadeAndLoadScene("Lobby");
        }
        
        public void OnOptionsButton()
        {
            MenuManager.Instance.PushMenu("OptionsMenu");
        }
        public void OnExitButton()
        {
            Application.Quit();
        }
    }
}