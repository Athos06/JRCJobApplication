using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JobApplicationGame
{
    public class InterviewMiniGame : SceneInitializer
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
            MusicManager.Instance.FadeAndChangeMusic("InterviewMiniGame");
        }

        /// <summary>
        /// We change the music to phase2 of the minigame (this function shouldnt be in the scene intializer)
        /// </summary>
        public void ChangeMusicPhase2()
        {
            MusicManager.Instance.FadeAndChangeMusic("InterviewMiniGamePhase2");
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