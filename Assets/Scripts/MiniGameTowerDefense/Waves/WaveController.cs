using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JobApplicationGame
{
    /// <summary>
    /// It control all the logic relate to the managment of waves, start/stop waves, spawn the enemies in the wave, etc..
    /// </summary>
    public class WaveController : MonoBehaviour
    {
        /// <summary>
        /// All The waves that this level has
        /// </summary>
        [SerializeField]
        private LevelWaves levelWaves;

        /// <summary>
        /// the wave that is currently active
        /// </summary>
        private Wave currentWave;
        /// <summary>
        /// the index of the current wave in the wave list in levelWaves
        /// </summary>
        private int waveIndex;
        /// <summary>
        /// number of enemies left to spawn in the current wave. Used to check if we finished spawning all the enemies for this wave and therefore we should stop spawning
        /// </summary>
        private int enemiesLeftToSpawn;

        /// <summary>
        /// A list with the enemies to spawn in the current wave
        /// </summary>
        List<EnemiesInWave> enemiesListInWave = new List<EnemiesInWave>();

        /// <summary>
        /// The start position for the enemies, enemy base spawn point
        /// </summary>
        [SerializeField]
        private Transform enemySpawnPoint;
        /// <summary>
        /// The goal position for the enemies, player base position
        /// </summary>
        [SerializeField]
        private Transform goalPosition;

        /// <summary>
        /// Flag to indicate if we should stop enemy spawning or not
        /// </summary>
        private bool stop = false;
        /// <summary>
        /// Flag to indicate if we lost the current wave
        /// </summary>
        private bool lostWave = false;

        /// <summary>
        /// The UI text to show the name of the wave
        /// </summary>
        public Text waveNameText;

        /// <summary>
        /// Audio source for the sounds
        /// </summary>
        public AudioSource audioSource;
        /// <summary>
        /// sound to play when we win the wave
        /// </summary>
        public AudioClip winWaveClip;
        /// <summary>
        /// sound to play when we lost the wave
        /// </summary>
        public AudioClip loseWaveClip;

        // Use this for initialization
        void Start()
        {
            // We register to the OnRemovedEnemy event to get notification when a enemy is removed
            Messenger.AddListener<int>("OnRemovedEnemy", OnRemovedEnemy);
            Messenger.AddListener("OnLostWave", OnLostWave);

            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (levelWaves.waves.Count == 0)
            {
                Debug.LogError("There are no waves in this level, please set them up");
            }

            if (levelWaves.waves.Count > 0)
            {
                // We get the first wave of the level
                waveIndex = 0;
                currentWave = levelWaves.waves[waveIndex];
            }

            CalculateEnemiesInWave();

        }

        /// <summary>
        /// In this function we calculate how many enemies are in the current wave and initialize different variable about that.
        /// </summary>
        private void CalculateEnemiesInWave()
        {
            lostWave = false;

            enemiesListInWave.Clear();

            foreach ( var enem in currentWave.enemies) {
                enemiesListInWave.Add(enem);
            }

            var waveTotalEnemies = 0;
            // We go through the array of different enemies in this wave to add them together and get the total number of enemies.
            for (int i = 0; i < enemiesListInWave.Count; i++)
            {
                waveTotalEnemies += enemiesListInWave[i].numberOfEnemies;
                enemiesListInWave[i].EnemiesLeft = enemiesListInWave[i].numberOfEnemies;
            }
            enemiesLeftToSpawn = waveTotalEnemies;
            // We also initialize here how many enemies there left to destroy in the wave, before the wave starts.
            levelWaves.waves[waveIndex].EnemiesLeftToDestroy = waveTotalEnemies;

            GetWaveName();

        }

        /// <summary>
        /// We update the name of the wave to show
        /// </summary>
        private void GetWaveName()
        {
            waveNameText.text = currentWave.name;
        }

        /// <summary>
        /// Entry point to We start the waves.
        /// </summary>
        public void StartWaves()
        {
            //we start the current wave with the specific time delay to start that the wave could have
            StartCoroutine(StartWave(currentWave.StartDelay));
        }


        public void StopSpawning()
        {
            stop = true;
        }

        public void ContinueSpawning()
        {
            stop = false;
            SpawnNextWave();
        }

        /// <summary>
        /// Coroutine to start the current wave after the start delay
        /// </summary>
        /// <param name="delayTime">time delay to start the wave</param>
        /// <returns></returns>
        private IEnumerator StartWave(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SpawningWave();

        }

        /// <summary>
        /// It's in charge of intantiating the enemies of the wave.
        /// </summary>
        /// <param name="enemyType">The enemy to spawn</param>
        /// <param name="waitTime"> How long does it wave between the spawn of the current enemy and previous one</param>
        /// <returns></returns>
        IEnumerator spawnEnemy(Enemy enemyType, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            enemiesLeftToSpawn--;

            Enemy enemy = Instantiate<Enemy>(enemyType);
            enemy.Initiate(enemySpawnPoint, goalPosition, waveIndex);

            SpawningWave();

        }

        /// <summary>
        /// In charge of calling to spawn the different enemies of the wave
        /// </summary>
        private void SpawningWave()
        {
            if (enemiesLeftToSpawn > 0)
            {
                bool getValidEnemy = false;
                while (!getValidEnemy)
                {
                    
                    // We try to get one random enemy from the different kinds of enemies that the wave has
                    int randomEnemyIndex = UnityEngine.Random.Range(0, enemiesListInWave.Count);
                    //If there enemies of that kind left to spawn we do so
                    if (enemiesListInWave[randomEnemyIndex].EnemiesLeft > 0)
                    {
                        enemiesListInWave[randomEnemyIndex].EnemiesLeft--;

                        StartCoroutine(spawnEnemy(enemiesListInWave
                        [randomEnemyIndex].enemyType, currentWave.SpawnsDelay));
                        IsWaveSpawningEnd();
                        //we got a valid enemy to spawn
                        getValidEnemy = true;
                    }
                    // Otherwise we remove that kind of enemies from the list of enemies available and then we will try to get another one
                    else
                    {
                        enemiesListInWave.RemoveAt(randomEnemyIndex);
                    }
                }
            }
            //We check if the wave finished spawning enemies after spawning the previous one
            IsWaveSpawningEnd();
        }

        /// <summary>
        /// returns if the wave finished spawning enemies, and if so we finish the spawning of the wave.
        /// </summary>
        /// <returns></returns>
        private bool IsWaveSpawningEnd()
        {
            //if no enemies left to spawn then we end the spawning of this wave
            if (enemiesLeftToSpawn <= 0)
            {
                endSpawningWave();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stops the spawning of enemies in the current wave and calls to start the next one if there is such.
        /// </summary>
        private void endSpawningWave()
        {
            // We stop spawning enemies also from the next Wave in case the StopAfterWaveSpawning flag is ticked. With this we can stop between waves to do something in between and have more control 
            if (levelWaves.waves[waveIndex].StopAfterWaveSpawning)
            {
                StopSpawning();
                return;
            }
            //If we dont want to stop the spawning of the next wave we start it
            SpawnNextWave();
        }

        /// <summary>
        /// Called to start the next wave.
        /// </summary>
        private void SpawnNextWave()
        {
            // If we wanted to stop the spawning we dont start next wave yet
            if (stop) return;

            //if we lost the current wave we will have to retry the same one 
            if (lostWave)
            {
                currentWave = levelWaves.waves[waveIndex];
                lostWave = false;
            }
            else
            {
                //we check if we already finished all the waves
                if (AllWavesEnded()) { return; }

                //if we didnt we update the index to the next wave
                waveIndex++;

                currentWave = levelWaves.waves[waveIndex];
            }

            Messenger.Broadcast("OnStartWave");
            CalculateEnemiesInWave();
            //we start the next wave
            StartCoroutine(StartWave(currentWave.StartDelay));
        }

        /// <summary>
        /// Check if all waves from the level ended
        /// </summary>
        /// <returns></returns>
        private bool AllWavesEnded()
        {
            if (waveIndex >= levelWaves.waves.Count - 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when all the enemies in the current wave have been destroyed
        /// </summary>
        /// <param name="waveIndex"></param>
        private void AllEnemiesInWaveDestroyed(int waveIndex)
        {
            //we trigger the events (in case we had any) that we set for when all the enemies in the wave were destroyed
            //if we beat the wave we call the event for the victory, if lost we call the lose events
            if (lostWave) {
                audioSource.clip = loseWaveClip;
                audioSource.Play();
                levelWaves.waves[waveIndex].waveCallbackLose.Invoke();
            }
            else {
                audioSource.clip = winWaveClip;
                audioSource.Play();
                levelWaves.waves[waveIndex].waveCallbackWin.Invoke();
            }
        }

        /// <summary>
        /// we subscribe to the event that triggers when a enemy is destroyed waveIndex is the index of the wave the enemy was created in, to count if all the enemies in that specific wave were killed and therefore end the wave
        /// </summary>
        /// <param name="waveIndex">The index of the wave to which the enemy that was removed belongs</param>
        public void OnRemovedEnemy(int waveIndex)
        {
            levelWaves.waves[waveIndex].EnemiesLeftToDestroy--;
            if (levelWaves.waves[waveIndex].EnemiesLeftToDestroy <= 0)
            {
                //if all the enemies in the wave has been destroyed we call the proper function
                AllEnemiesInWaveDestroyed(waveIndex);

                //if all the enemies in the last wave have been destroyed we have cleared the level
                if ((waveIndex == this.waveIndex) && AllWavesEnded())
                {
                   //We dont use it here, but if for some reason we want to do something after all the waves have been clean we would notify of that happening
                    //Messenger.Broadcast("OnLevelClear");
                }
            }
        }

        /// <summary>
        /// Called when the player base has been destroyed and therefore we lost the wave
        /// </summary>
        public void OnLostWave()
        {
            lostWave = true;
        }

    }
}