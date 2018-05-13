using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class SuperSpecialAttack : MonoBehaviour
    {

        /// <summary>
        /// The position where the UFO will go to stay
        /// </summary>
        public Transform targetPosition;
                
        [SerializeField]
        private float m_Speed;

        public void Activate()
        {
            gameObject.SetActive(true);
            StartCoroutine(moveIntoPosition());
            //superSecretButton.SetActive(false);
        }

        // in the end we do all this in the unity editor with events in buttons
        //public void UnlockSecretWeapon()
        //{
        //    superSecretButton.SetActive(true);
        //}

        /// <summary>
        /// We move the ufo into position
        /// </summary>
        /// <returns></returns>
        private IEnumerator moveIntoPosition()
        {
            bool reachDestination = false;
            while (!reachDestination)
            {
                float distanceThisFrame = m_Speed * Time.deltaTime;
                transform.LookAt(targetPosition);

                Vector3 direction = targetPosition.position - transform.position;
                transform.Translate(direction.normalized * distanceThisFrame, Space.World);

                if (direction.magnitude <= distanceThisFrame) { reachDestination = true; }

                yield return new WaitForEndOfFrame();
            }

            yield break;
        }
    }
}