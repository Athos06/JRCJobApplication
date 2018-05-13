using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class Proyectile : MonoBehaviour
    {

        [SerializeField]
        private float m_speed;
        private int m_damage = 100;

        private Transform m_target;

        public GameObject explosionParticles;

        // Update is called once per frame
        void Update()
        {
            if (m_target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = m_target.position - transform.position;
            //this would be the distance that the proyectile would move in this frame
            float distanceThisFrame = m_speed * Time.deltaTime;

            //if the bullet will move past the target in this frame we hit the target
            if (direction.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            transform.LookAt(m_target);
        }

        private void HitTarget()
        {
            if (m_target == null) { return; }
            Enemy targetEnemy = m_target.GetComponent<Enemy>();
            if (targetEnemy == null) { return; }
            targetEnemy.GetDamaged(m_damage);
            GameObject explosion = (GameObject)Instantiate(explosionParticles, transform.position, transform.rotation);
            Destroy(explosion, 4f);
    
            Destroy(gameObject);
            
        }
        public void Init(Transform target, int damage)
        {
            m_target = target;
            m_damage = damage;
        }

    }
}