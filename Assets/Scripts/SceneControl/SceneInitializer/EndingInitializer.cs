using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class EndingInitializer : SceneInitializer
    {

        private void Awake()
        {
            InitScene();
        }

        private void Start()
        {
            Messenger.AddListener("OnStartSceneFadeout", OnStartSceneFadeout);
        }
        protected override void InitScene()
        {
            base.InitScene();

            MusicManager.Instance.FadeAndChangeMusic("Ending");
            MenuManager.Instance.PopAll();
        
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