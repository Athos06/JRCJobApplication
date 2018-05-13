using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JobApplicationGame
{    
    /// <summary>
    /// Base class for the tower in the tower defense mini game.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class Tower : MonoBehaviour
    {

        /// <summary>
        /// The turn speed of the tower.
        /// </summary>
        [Header("tower properties")]
        [SerializeField]
        private float m_turnSpeed;
        /// <summary>
        /// The current target the tower has.
        /// </summary>
        [SerializeField]
        private Transform m_target;
        /// <summary>
        /// Number of proyectiles the tower shoots per second.
        /// </summary>
        [SerializeField]
        private float m_fireRate;
        /// <summary>
        /// Damage the tower deals.
        /// </summary>
        [SerializeField]
        private int m_damage;
        /// <summary>
        /// How many credits does it cost to build.
        /// </summary>
        [SerializeField]
        private int cost;
        public int Cost
        {
            get { return cost; }
        }
        /// <summary>
        /// How many credits do we get back selling the tower.
        /// </summary>
        [SerializeField]
        private int m_SellPrice;
        public int SellPrice
        {
            get { return m_SellPrice; }
        }

        public string name;

        /// <summary>
        /// Trigger collider that represents the range of the tower.
        /// </summary>
        private Collider m_rangeCollider;

        /// <summary>
        /// Audio source for the shooting sound
        /// </summary>
        protected AudioSource audioSource;
        /// <summary>
        /// audio clip for the shooting sound
        /// </summary>
        public AudioClip audioClip;

        /// <summary>
        /// List with all the possible targets of the tower in that frame.
        /// </summary>
        private List<Transform> m_targetsList = new List<Transform>();

        /// <summary>
        /// Flag to check if the tower is shooting.
        /// </summary>
        private bool isShooting = false;

        /// <summary>
        /// The part of the tower that rotates
        /// </summary>
        [Header("tower init")]
        public Transform headToRotate;
        /// <summary>
        /// The position from where to proyectiles are created
        /// </summary>
        public Transform firePoint;

        /// <summary>
        /// The proyectilePrefab that we will instantiate at firing
        /// </summary>
        public Proyectile proyectilePrefab;

        /// <summary>
        /// The initial rotation when we create the tower so we can go back to it
        /// </summary>
        private Quaternion startRotation;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            startRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            GetTarget();
            if (m_target) { StartCoroutine(Shoot()); }
        }

        /// <summary>
        /// Called to shoot
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Shoot()
        {
            if (isShooting) { yield break; }

            isShooting = true;
           
            Proyectile proyectile = Instantiate<Proyectile>(proyectilePrefab, firePoint.position, firePoint.rotation);
            if (proyectile != null)
            {
                proyectile.Init(m_target, m_damage);
            }

            //plays the shooting sound
            if (audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }

            yield return new WaitForSeconds(1 / m_fireRate);

            isShooting = false;
        }

        /// <summary>
        /// We acquire the target for the tower every frame here
        /// </summary>
        protected void GetTarget()
        {
            m_target = null;

            if (m_targetsList.Count > 0)
            {
                //we get the enemy that got first into range
                m_target = m_targetsList[0];

                if (m_target)
                {
                    LockOnTarget();
                }

                //if target is null it means the target has been destroyed and didnt leave the collider, therefore we will be stuck always locked on the null target.
                //we have to remove the destroyed target and acquire a new target again
                else
                {
                    m_targetsList.RemoveAt(0);
                    GetTarget();
                    return;
                }
            }
            else
            {
                // If we have no targets we reset the start rotation
                ResetRotation();
            }
        }

        /// <summary>
        /// Function to reset the rotation of the tower to the initial position
        /// </summary>
        protected void ResetRotation()
        {
            Vector3 rotation = Quaternion.Lerp(headToRotate.rotation, startRotation, Time.deltaTime * m_turnSpeed).eulerAngles;
            headToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }


        /// <summary>
        /// Called to lock on the current target
        /// </summary>
        protected void LockOnTarget()
        {
            if (m_target == null)
            {
                return;
            }
            // we lock on the target to always face to it
            Vector3 direction = m_target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(headToRotate.rotation, lookRotation, Time.deltaTime * m_turnSpeed).eulerAngles;
            headToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        public void OnTriggerEnter(Collider other)
        {
            //if an enemy gets inside the trigger range we add it to the list of targets
            if (other.tag == "Enemy")
            {
                m_targetsList.Add(other.gameObject.transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            //when enemies leave the collider we remove them from the list of possible targets
            if (other.tag == "Enemy")
            {
                m_targetsList.Remove(other.gameObject.transform);
            }
        }
    }
}