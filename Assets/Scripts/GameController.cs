using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{   
    /// <summary>
    /// In charge of controlling the state of the game.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        //Lazy singleton
        private static GameController _instance;
        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (GameController)FindObjectOfType(typeof(GameController));
                return _instance;
            }
        }


        /// <summary>
        /// an image to fade as background when the game is paused.
        /// </summary>
        public CanvasGroup PauseFade;

        private void Awake()
        {
            IsFirstTimePlayed();
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
        /// Pauses the game.
        /// </summary>
        public void PauseGame()
        {
            PauseFade.gameObject.SetActive(true);
            Time.timeScale = 0.00001f;
            PauseFade.alpha = 1;

            Messenger.Broadcast("OnPauseGame");
        }

        /// <summary>
        /// Unpauses the game.
        /// </summary>
        public void UnpauseGame()
        {
            PauseFade.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            PauseFade.alpha = 0;

            Messenger.Broadcast("OnUnpauseGame");
        }


        /// <summary>
        /// Check if its the first time ever we opened the game, and if it is we create the playerprefs values.
        /// </summary>
        void IsFirstTimePlayed()
        {
            if (!PlayerPrefs.HasKey("FirstTimePlayed"))
            {
                PlayerPrefs.SetInt(Saves.FirstTimePlayed, 0);
                PlayerPrefs.SetFloat(Saves.MasterVolume, 0.7f);
                PlayerPrefs.SetFloat(Saves.SFXVolume, 0.7f);
                PlayerPrefs.SetFloat(Saves.MusicVolume, 0.7f);
            }
        }
    }
}