using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JobApplicationGame
{
    /// <summary>
    /// Class in charge of controlling the credits of the player
    /// </summary>
    public class Currency : MonoBehaviour
    {
        /// <summary>
        /// Credits the player has
        /// </summary>
        [SerializeField]
        private int m_Credits;
        public int Credits
        {
            get { return m_Credits; }

        }
        /// <summary>
        /// Credits that the player starts with
        /// </summary>
        [SerializeField]
        private int m_StartCredits;
        /// <summary>
        /// Credits the player gets with every tick (flow of credits per time)
        /// </summary>
        [SerializeField]
        private int m_CreditsPerTick;

        /// <summary>
        /// If the tick (increase of credits in time) are active or not
        /// </summary>
        private bool m_Tick = false;
        /// <summary>
        /// Time in which the tick happens
        /// </summary>
        [SerializeField]
        private float m_TimeForTick = 1.0f;
        
        /// <summary>
        /// Access to the credits counter text in the UI (to update it)
        /// </summary>
        public Text CreditsCounterText;


        // Use this for initialization
        void Start()
        {
            m_Credits = m_StartCredits;
            UpdateCounter();

        }

        /// <summary>
        /// called adds/substract credits
        /// </summary>
        /// <param name="credits"></param>
        public void UpdateCredits(int credits)
        {
            m_Credits += credits;
            //after modifying the credits counter we have to update the UI counter
            UpdateCounter();
        }

        /// <summary>
        /// Start the credits tick
        /// </summary>
        public void StartTick()
        {
            m_Tick = true;
            StartCoroutine(Tick());
        }

        /// <summary>
        /// Stops the ticks from happening.
        /// </summary>
        public void StopTick()
        {
            m_Tick = false;
        }

        /// <summary>
        /// Coroutine that does the tick, giving credits to the player.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Tick()
        {
            if (!m_Tick) { yield break; }

            while (m_Tick)
            {
                UpdateCredits(m_CreditsPerTick);
                yield return new WaitForSeconds(m_TimeForTick);
            }

            yield break;
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        private void UpdateCounter()
        {
            CreditsCounterText.text = m_Credits.ToString();

        }

    }
}