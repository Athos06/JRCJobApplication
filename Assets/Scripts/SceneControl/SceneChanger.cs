using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    
    /// <summary>
    /// A little helper class just to attach to a game object in the scene so we can access the SceneController from within the unityevents system in the editoer
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadScene(string sceneName)
        {
            SceneController.Instance.FadeAndLoadScene(sceneName);
        }
    }
}