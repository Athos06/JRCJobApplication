using UnityEngine;
using System.Collections;
using UnityEngine.Audio;



namespace JobApplicationGame {
    /// <summary>
    /// Class to manage the audio volume
    /// </summary>
    public class AudioManager : MonoBehaviour {

        private static AudioManager _instance = null;                 
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (AudioManager)FindObjectOfType(typeof(AudioManager));
                return _instance;
            }
        }
    
        /// <summary>
        /// Reference to audio mixer for volume changing
        /// </summary>
        public AudioMixer gameMixer;

        /// <summary>
        /// Master volume parameter on the mixer
        /// </summary>
        public string masterVolumeParameter;

        /// <summary>
        /// SFX volume parameter on the mixer
        /// </summary>
        public string sfxVolumeParameter;

        /// <summary>
        /// Music volume parameter on the mixer
        /// </summary>
        public string musicVolumeParameter;


        void Start()
        {
            //we get the volume saved to start the game with the desired volume
            float master, sfx, music;
            GetVolumes(out master, out sfx, out music);
            SetVolumes(master, sfx, music, false);
        }
    
        /// <summary>
        /// We get the volume saved in playerprefs.
        /// </summary>
        /// <param name="master"></param>
        /// <param name="sfx"></param>
        /// <param name="music"></param>
        public void GetVolumes(out float master, out float sfx, out float music)
        {

            master = PlayerPrefs.GetFloat(Saves.MasterVolume);
            sfx = PlayerPrefs.GetFloat(Saves.SFXVolume);
            music = PlayerPrefs.GetFloat(Saves.MusicVolume);
        }

        /// <summary>
        /// Set and persist game volumes
        /// </summary>
        public void SetVolumes(float master, float sfx, float music, bool save)
        {
            //// Early out if no mixer set
            if (gameMixer == null)
            {
                return;
            }

            // Transform 0-1 into logarithmic -80-0
            if (masterVolumeParameter != null)
            {
                gameMixer.SetFloat(masterVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(master)));
            }
            if (sfxVolumeParameter != null)
            {
                gameMixer.SetFloat(sfxVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(sfx)));
            }
            if (musicVolumeParameter != null)
            {
                gameMixer.SetFloat(musicVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(music)));
            }

            // If indicated, we saved the volumes value to playerprefs
            if (save)
            {
                PlayerPrefs.SetFloat(Saves.MasterVolume, master);
                PlayerPrefs.SetFloat(Saves.SFXVolume, sfx);
                PlayerPrefs.SetFloat(Saves.MusicVolume, music);
            }
        }

        /// <summary>
        /// Transform volume from linear to logarithmic
        /// </summary>
        protected static float LogarithmicDbTransform(float volume)
        {
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            return volume - 80;
        }
    }
}







