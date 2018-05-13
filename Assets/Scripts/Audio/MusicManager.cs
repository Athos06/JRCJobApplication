using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace JobApplicationGame
{
    /// <summary>
    /// Class in charge on chaning the music
    /// </summary>
    public class MusicManager : MonoBehaviour
    {

        /// <summary>
        /// Speed to fade in/out music changing scenes
        /// </summary>
        public float fadeSpeed = 1.0f;

        /// <summary>
        /// little hack to show something like a dictionary on the editor.
        /// </summary>
        [System.Serializable]
        public struct LevelMusic
        {
            public string sceneName;
            public AudioClip audioClip;
        }

        /// <summary>
        /// Lazy singleton, change for a proper singleton.
        /// </summary>
        private static MusicManager _instance = null;
        public static MusicManager Instance
        {
            get
            {
                if (_instance == null) { _instance = (MusicManager)FindObjectOfType(typeof(MusicManager)); }

                return _instance;
            }
        }

        /// <summary>
        /// The audio source for the music.
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// the music asociated with each scene name.
        /// </summary>
        public LevelMusic[] levelMusicDictionary;


        private float maxVolume;

        private void Start()
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            maxVolume = audioSource.volume;
        }

        /// <summary>
        /// We get the audio clip we want to play for the specified scene.
        /// </summary>
        /// <param name="sceneName">Name of the scene we gont to get the audio clip for.</param>
        /// <returns></returns>
        private AudioClip GetMusicForScene(string sceneName)
        {
            for (int i = 0; i < levelMusicDictionary.Length; i++)
            {
                if (levelMusicDictionary[i].sceneName == sceneName)
                {
                    return levelMusicDictionary[i].audioClip;
                }
            }

            return null;
        }


        /// <summary>
        /// Called to change the music to the one asociated to the scene name passed.
        /// </summary>
        /// <param name="sceneName">Name of the scene we want to get the music from</param>
        public void SetSceneMusic(string sceneName)
        {
            audioSource.clip = GetMusicForScene(sceneName);
            if (audioSource.clip == null) { Debug.LogError("that audioclip for the scene doesnt exist"); }
        }

        private bool fadeIn = false;
        private bool fadeOut = false;

        public void FadeAndChangeMusic(string sceneName)
        {
            StartCoroutine(FadeAndSwitchMusic(GetMusicForScene(sceneName), fadeSpeed));
        }

        private IEnumerator FadeAndSwitchMusic(AudioClip clip, float speed)
        {

            if (audioSource.clip == null)
            {
                audioSource.clip = clip;
                if (audioSource.clip == null) { Debug.LogError("that audioclip for the scene doesnt exist"); }
                yield return StartCoroutine(FadeIn(speed));
                StartMusic();
            }
            else
            {
                yield return StartCoroutine(FadeOut(speed));
                audioSource.clip = clip;
                if (audioSource.clip == null) { Debug.LogError("that audioclip for the scene doesnt exist"); }
                yield return StartCoroutine(FadeIn(speed));
                StartMusic();
            }
        }

        public void FadeOutMusic()
        {
            if (audioSource.clip == null)
            {
                return;
            }

            StartCoroutine(FadeOut(fadeSpeed));
        }

        private IEnumerator FadeIn(float speed)
        {
            fadeIn = true;
            fadeOut = false;

            audioSource.volume = 0;
            float volume = audioSource.volume;

            while(audioSource.volume < maxVolume && fadeIn)
            {
                volume += speed * Time.deltaTime;
                audioSource.volume = volume;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FadeOut(float speed)
        {
            fadeIn = false;
            fadeOut = true;

            float volume = audioSource.volume;

            while (audioSource.volume > 0 && fadeOut)
            {
                volume -= speed * Time.deltaTime;
                audioSource.volume = volume;
                yield return new WaitForEndOfFrame();
            }
        }
        /// <summary>
        /// Starts the music.
        /// </summary>
        public void StartMusic()
        {
            audioSource.Play();
        }

        /// <summary>
        /// Stops the music.
        /// </summary>
        public void StopMusic()
        {
            audioSource.Stop();
        }

        /// <summary>
        /// Pauses the music.
        /// </summary>
        public void PauseMusic()
        {
            audioSource.Pause();
        }
    }
}