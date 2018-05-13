using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{

    /// <summary>
    /// A scene intilializer is a game object present on the scene and in charge of calling or creating specific behavior for a scene, usually at Awake() after the scene is loaded
    /// </summary>
    public class SceneInitializer : MonoBehaviour
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

        /// <summary>
        /// Called to initialize
        /// </summary>
        virtual protected void InitScene()
        {

        }
    }
}