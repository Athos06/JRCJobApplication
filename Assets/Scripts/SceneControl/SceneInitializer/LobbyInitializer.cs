using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class LobbyInitializer : SceneInitializer
    {

        private void Awake()
        {
            InitScene();
        }
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void InitScene()
        {
            MenuManager.Instance.PopAll();
            MenuManager.Instance.PushMenu("InGameMenu");
            base.InitScene();
            MusicManager.Instance.FadeAndChangeMusic("Lobby");
        }

        public void OnStartSceneFadeout()
        {

        }
    }
}