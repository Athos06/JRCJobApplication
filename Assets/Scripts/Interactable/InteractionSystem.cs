using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    
    /// <summary>
    /// Class for the player interaction
    /// </summary>
    public class InteractionSystem : MonoBehaviour
    {

        public void StartInteraction(Interactable interactable)
        {
            interactable.Interact();
        }
    }
}
