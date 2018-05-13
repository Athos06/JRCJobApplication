using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class InterviewRoomEndingInitializer : SceneInitializer
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
            MusicManager.Instance.FadeAndChangeMusic("InterviewRoomEnding");
        }


        public void OnStartSceneFadeout()
        {
            MusicManager.Instance.FadeOutMusic();
        }
    }
}