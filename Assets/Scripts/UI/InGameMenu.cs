using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class InGameMenu : MenuState
    {

        public override void Open()
        {
            base.Open();
        }

        public void OnPauseMenu()
        {
            GameController.Instance.PauseGame();
            MenuManager.Instance.PushMenu("PauseMenu");
        }
    }
}