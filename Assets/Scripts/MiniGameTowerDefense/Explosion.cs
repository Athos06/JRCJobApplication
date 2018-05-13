using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    /// <summary>
    /// Explosion created when a ship is destroyed
    /// </summary>
    public class Explosion : MonoBehaviour
    {
                
        /// <summary>
        /// Audio source for the explosion sound
        /// </summary>
        AudioSource audioSource;
        /// <summary>
        /// the audio clip of the explosion sound
        /// </summary>
        public AudioClip audioClip;
        /// <summary>
        /// The particle system to create
        /// </summary>
        public GameObject particles;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
        {
            if (audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("audio source in " + gameObject + " is missing");
            }

            if (particles != null)
            {
                GameObject particle =  (GameObject)Instantiate(particles, transform.position, particles.transform.rotation);
                Destroy(particle, 2f);
            }
            else
            {
                Debug.LogError("the particles effect in  " + gameObject + " is missing");
            }

        }
    }
}