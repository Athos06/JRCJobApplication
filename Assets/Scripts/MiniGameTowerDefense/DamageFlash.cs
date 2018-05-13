using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    /// <summary>
    /// Class that makes some game object blink or flash (when taking damage for instance)
    /// </summary>
    public class DamageFlash : MonoBehaviour
    {
        
        /// <summary>
        /// Color with which we want to flash
        /// </summary>
        public Color flashColor;

        /// <summary>
        /// Access to the renderer of the object
        /// </summary>
        private Renderer m_Renderer;
        /// <summary>
        /// Original color of the game object to go back to (its an array because the object can have multiple materials)
        /// </summary>
        private Color[] m_startColors;
        /// <summary>
        /// flag to control the flash
        /// </summary>
        private bool m_IsFlashing;


        // Use this for initialization
        void Start()
        {
            m_Renderer = GetComponent<Renderer>();
            m_startColors = new Color[m_Renderer.materials.Length];

            for(int i = 0; i < m_Renderer.materials.Length; i++)
            {
                m_startColors[i] = m_Renderer.materials[i].color;
            }
        }

        /// <summary>
        /// Starts flashing
        /// </summary>
        /// <param name="nTimes">Number of times we want the game object to blink</param>
        public void Flash(int nTimes)
        {
            if (m_IsFlashing) { return; }
            StartCoroutine(StartFlash(nTimes));
        }

        /// <summary>
        /// Coroutine that does the flashing
        /// </summary>
        /// <param name="nFlashes">Number of flashes</param>
        /// <returns></returns>
        private IEnumerator StartFlash(int nFlashes)
        {
            m_IsFlashing = true;

            for (int i = 1; i < nFlashes; i++)
            {
                for (int j = 0; j < m_Renderer.materials.Length; j++)
                {
                    m_Renderer.materials[j].color = flashColor;
                }
                // 0.1 is a hardcoded value for flashing every 0.1s
                yield return new WaitForSeconds(0.1f);
                for (int j = 0; j < m_Renderer.materials.Length; j++)
                {
                    m_Renderer.materials[j].color = m_startColors[j];
                }
                
                yield return new WaitForSeconds(0.1f);
            }

            m_IsFlashing = false;
        }
    }
}