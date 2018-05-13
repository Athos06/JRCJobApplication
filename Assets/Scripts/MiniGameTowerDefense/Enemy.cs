using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace JobApplicationGame
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Enemy starting HP
        /// </summary>
        [SerializeField]
        private int startingHP;
        /// <summary>
        /// Enemy current HP
        /// </summary>
        private int hp;
        /// <summary>
        /// Damage that the enemy deals when it reaches the player base
        /// </summary>
        [SerializeField]
        private int damage;
        /// <summary>
        /// The index of the wave where the enemy was created (used to control how many enemies are left in the specific wave
        /// </summary>
        private int waveIndex;


        /// <summary>
        /// If multiple proyectiles hit the enemy in the same frame it could be destroyed multiple times in that frame, therefore send the event of OnRemovedEnemy multiple times. This flag avoids that.
        /// </summary>
        private bool destroyed = false;

        /// <summary>
        /// The Player base position where enemies are headed to
        /// </summary>
        private Transform goalPoint;
        /// <summary>
        /// The AI character controller of this enemy
        /// </summary>
        private AICharacterControl characterControl;

        /// <summary>
        /// Object in charge to create the explosion particles effect and sounds
        /// </summary>
        public GameObject explosion;

        private void Awake()
        {
            characterControl = GetComponent<AICharacterControl>();
        }
        // Use this for initialization
        void Start()
        {
            hp = startingHP;
        }

        /// <summary>
        /// We initialize the enemy with the start position, and the goal position to the player base
        /// </summary>
        /// <param name="start">the start position where the enemy is created</param>
        /// <param name="finish">The position of the player base</param>
        /// <param name="index">The index of the wave in which it was created</param>
        public void Initiate(Transform start, Transform finish, int index)
        {
            goalPoint = finish;
            waveIndex = index;
            gameObject.SetActive(false);
            transform.position = start.position;
            gameObject.SetActive(true);
            //we set the target position where we want our enemy to go (the player base) and the navagent will take care of the pathfinding on the navmesh to reach there
            characterControl.SetTarget(finish);
        }

        /// <summary>
        /// When the enemy hit the trigger collider of the player base we call this function to broadcast the event OnHitBase
        /// </summary>
        public void HitBase()
        {
            //we send the event OnHitBase to indicate the player base has being hit and we pass the damage the enemy does as a parameter
            Messenger.Broadcast<int>("OnHitBase", damage);
            RemoveEnemy();
        }

        /// <summary>
        /// when the enemy get hit it takes damage calling this function
        /// </summary>
        /// <param name="damage">The amount of damage the enemy took</param>
        public void GetDamaged(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                RemoveEnemy();
            }
        }


        /// <summary>
        /// We destroy the enemy in this function and we also send the event to notify it
        /// </summary>
        public void RemoveEnemy()
        {
            //if multiple proyectiles hit the enemy in the same frame it could be destroyed multiple (until destroy(gameobject) is called at the end of the frame) times in that frame, therefore send the event of OnRemovedEnemy multiple times. This flag avoids that.
            if (destroyed) return;

            destroyed = true;

            //we create the explosion particle effect
            GameObject expl = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
            Destroy(expl, 2);
            //we send waveIndex as a parameter so we can know to what wave this enemy was part of
            Messenger.Broadcast<int>("OnRemovedEnemy", waveIndex);
            Destroy(gameObject);
        }
    }
}