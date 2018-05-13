using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{

    /// <summary>
    /// A little class just in charge of detecting if the player gets inside the range collider and therefore is in range to interact.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class InteractionRange : MonoBehaviour
    {
        /// <summary>
        /// The trigger collider that represents the interaction range
        /// </summary>
        public Collider triggerCollider;
        private bool onRange = false;
        public bool OnRange
        {
            get { return onRange; }
        }

        private void Start()
        {
            if (!triggerCollider) { triggerCollider = GetComponent<Collider>(); }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                onRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                onRange = false;
            }
        }
    }
}