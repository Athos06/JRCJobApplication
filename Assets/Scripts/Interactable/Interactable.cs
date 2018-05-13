using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class Interactable : MonoBehaviour
    {
        public InteractionDialog interactionDialog;

        /// <summary>
        /// In charge to check if the player is inside the interaction range
        /// </summary>
        public InteractionRange interactionRange;

        /// <summary>
        /// return if the player is inside the range to interact with the object
        /// </summary>
        public bool OnRange
        {
            get { return interactionRange.OnRange; }
        }
        // Use this for initialization
        void Start()
        {
            if (!interactionDialog) { interactionDialog = GetComponent<InteractionDialog>(); }
            if (!interactionRange) { interactionRange = GetComponentInChildren<InteractionRange>(); }
        }

        /// <summary>
        /// We start the interaction
        /// </summary>
        public void Interact()
        {
            interactionDialog.StartInteraction();
        }
    }
}