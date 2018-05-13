using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JobApplicationGame
{
    /// <summary>
    /// Class that hold what kind of enemy, how many of them, and how many are left in the wave.
    /// </summary>
    [System.Serializable]
    public class EnemiesInWave
    {
        public string name;
        public Enemy enemyType;
        public int numberOfEnemies;

        private int enemiesLeft;
        public int EnemiesLeft
        {
            get { return enemiesLeft; }
            set { enemiesLeft = value; }
        }

    }
    /// <summary>
    /// Class that holds everything related to a Wave in the tower defense mini game.
    /// </summary>
    [System.Serializable]
    public class Wave
    {

        /// <summary>
        /// Name of the wave.
        /// </summary>
        public string name;

        /// <summary>
        /// List with the different kinds of enemies in the wave.
        /// </summary>
        public List<EnemiesInWave> enemies;

        /// <summary>
        /// The time to wait until the wave start (until enemies from the wave start spawning).
        /// </summary>
        [SerializeField]
        private float m_StartDelay;
        public float StartDelay
        {
            get { return m_StartDelay; }
        }
        /// <summary>
        /// Time between each enemy spawning in the wave
        /// </summary>
        [SerializeField]
        private float mSpawnsDelay;
        public float SpawnsDelay
        {
            get { return mSpawnsDelay; }
        }


        /// <summary>
        /// Should we stop the spawning of next waves after the current one?.
        /// </summary>
        [SerializeField]
        private bool m_stopAfterWaveSpawning = false;
        public bool StopAfterWaveSpawning
        {
            get
            {
                return m_stopAfterWaveSpawning;
            }

            set
            {
                m_stopAfterWaveSpawning = value;
            }
        }

        /// <summary>
        /// How many enemies left to destroy in the wave.
        /// </summary>
        private int m_enemiesLeftToDestroy;
        public int EnemiesLeftToDestroy
        {
            get { return m_enemiesLeftToDestroy; }
            set { m_enemiesLeftToDestroy = value; }
        }

        /// <summary>
        /// In case we want to call some callback when the wave finishes (and it was a victory).
        /// </summary>
        public UnityEvent waveCallbackWin;
        /// <summary>
        /// In case we want to call some callback when the wave finishes (and it was a lost).
        /// </summary>
        public UnityEvent waveCallbackLose;
    }

    [System.Serializable]
    public class LevelWaves
    {

        //the different waves in that level
        public List<Wave> waves = new List<Wave>();

    }
}