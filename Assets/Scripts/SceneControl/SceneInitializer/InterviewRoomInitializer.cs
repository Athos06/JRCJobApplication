using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame { 
    public class InterviewRoomInitializer : MonoBehaviour {

	    // Use this for initialization
	    void Start () {
            Messenger.AddListener("OnStartSceneFadeout", OnStartSceneFadeout);
        }
	
	    // Update is called once per frame
	    void Update () {
		
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
