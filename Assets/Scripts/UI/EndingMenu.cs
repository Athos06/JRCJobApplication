using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class EndingMenu : MonoBehaviour
    {

        public void OnBackMainMenu()
        {
            SceneController.Instance.FadeAndLoadScene("MainMenu");
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }
    }
}
