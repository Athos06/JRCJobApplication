using UnityEngine;
using System.Collections;

namespace JobApplicationGame
{
    public class PauseMenu : MenuState
    {
        override public void Open()
        {
            base.Open();
        }

        public void OnResumeButton()
        {
            MenuManager.Instance.PopMenu();
            GameController.Instance.UnpauseGame();

        }

        public void OnOptionsButton()
        {
            MenuManager.Instance.PushMenu("OptionsMenu");
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }

    }
}