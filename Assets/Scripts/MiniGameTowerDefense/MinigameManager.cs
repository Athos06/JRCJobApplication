using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    /// <summary>
    /// Manager for the tower defense minigame
    /// </summary>
    public class MinigameManager : MonoBehaviour
    {

        /// <summary>
        /// Access to the wave controller
        /// </summary>
        public WaveController waveController;
        /// <summary>
        /// Access to the wave controller
        /// </summary>
        public Currency currency;
        
        /// <summary>
        /// Interactable component for the scene, that controls the flow of the dialog in the minigame
        /// </summary>
        public Interactable dialog;

        private static MinigameManager _instance = null;

        public static MinigameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (MinigameManager)FindObjectOfType(typeof(MinigameManager));
                return _instance;
            }
        }

        // Use this for initialization
        void Start()
        {
            // When we start the mini game we also start the dialog
            dialog.Interact();
        }


        public void AddCurrency(int amount)
        {
            currency.UpdateCredits(amount);
        }


        public void ActivateCurrencyGain()
        {
            currency.StartTick();
        }

        public void DeactivateCurrencyGain()
        {
            currency.StopTick();
        }
        public void StartGame()
        {
            waveController.StartWaves();
            currency.StartTick();
        }
              
    }
}