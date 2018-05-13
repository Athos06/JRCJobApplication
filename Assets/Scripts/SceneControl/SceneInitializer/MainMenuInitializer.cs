using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class MainMenuInitializer : SceneInitializer
    {

        private void Awake()
        {
            InitScene();
        }
        // Use this for initialization
        void Start()
        {
            Messenger.AddListener("OnStartSceneFadeout", OnStartSceneFadeout);
        }

        protected override void InitScene()
        {
            MenuManager.Instance.PopAll();
            MenuManager.Instance.PushMenu("MainMenu");
            base.InitScene();
            MusicManager.Instance.FadeAndChangeMusic("MainMenu");
        }

        /// <summary>
        /// Called when the scene starts to fade out
        /// </summary>
        public void OnStartSceneFadeout()
        {
            MusicManager.Instance.FadeOutMusic();
        }
    }
}