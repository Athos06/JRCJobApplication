using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    /// <summary>
    /// Class for Player base
    /// </summary>
    public class PlayerBase : MonoBehaviour
    {
        /// <summary>
        /// Hitpoints that the base has at start
        /// </summary>
        [SerializeField]
        private int startHP;
        /// <summary>
        /// current HP
        /// </summary>
        [SerializeField]
        private int HP;

        /// <summary>
        /// Access to the class in charge of the take damage flash effect
        /// </summary>
        public DamageFlash damageFlash;

        /// <summary>
        /// particle effect to create when the base has less than half HP
        /// </summary>
        public ParticleSystem baseDamagedFX;
        /// <summary>
        /// particle effect to show when the base has been destroyed
        /// </summary>
        public ParticleSystem baseDestroyedFX;

        /// <summary>
        /// 
        /// </summary>
        public Transform particlesSpawnPosition;

        // Use this for initialization
        void Start()
        {
            Messenger.AddListener<int>("OnHitBase", OnHitBase);
            Messenger.AddListener("OnStartWave", OnStartWave);

            damageFlash = GetComponent<DamageFlash>();

            HP = startHP;
            
            // At first we dont want the particle effects to play            
            baseDamagedFX.Stop();
            baseDestroyedFX.Stop();
        }
        
        /// <summary>
        /// Resets the base HP, and stops the particle effects
        /// </summary>
        private void ResetBase()
        {
            baseDamagedFX.Stop();
            baseDestroyedFX.Stop();
            HP = startHP;
        } 

        /// <summary>
        /// Called when the base takes damage
        /// </summary>
        /// <param name="damage">Amount of damage the base took</param>
        public void OnHitBase(int damage)
        {
            damageFlash.Flash(4);
            HP -= damage;
            //if the base has less than half HP left we show the smoke effect
            if (HP <= startHP/2)
            {
                baseDamagedFX.Play();
            }
            // if the base is destroyed we also show the fire
            if (HP <= 0)
            {
                baseDestroyedFX.Play();
                //we broadcast that the base has been destroyed and therefore the wave lost
                Messenger.Broadcast("OnLostWave");
            }
         }

        /// <summary>
        /// When a new wave starts the base is reset
        /// </summary>
        public void OnStartWave()
        {
            ResetBase();
        }

        public void OnTriggerEnter(Collider other)
        {
            //if an enemy enters the base collider the base will take damage
            if (other.gameObject.tag == "Enemy")
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.HitBase();
            }
        }

    }
}