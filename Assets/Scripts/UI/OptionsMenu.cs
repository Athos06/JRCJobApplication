using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JobApplicationGame
{
    public class OptionsMenu : MenuState
    {

        public Slider masterSlider;

        public Slider sfxSlider;

        public Slider musicSlider;

        override public void Open()
        {
            //first we get the default values for the volumes
            float masterVolume, sfxVolume, musicVolume;
            AudioManager.Instance.GetVolumes(out masterVolume, out sfxVolume, out musicVolume);

            if (masterSlider != null)
            {
                masterSlider.value = masterVolume;
            }
            if (sfxSlider != null)
            {
                sfxSlider.value = sfxVolume;
            }
            if (musicSlider != null)
            {
                musicSlider.value = musicVolume;
            }

            base.Open();
        }

        /// <summary>
        /// Retrieve values from sliders
        /// </summary>
        void GetSliderVolumes(out float masterVolume, out float sfxVolume, out float musicVolume)
        {
            masterVolume = masterSlider != null ? masterSlider.value : 1;
            sfxVolume = sfxSlider != null ? sfxSlider.value : 1;
            musicVolume = musicSlider != null ? musicSlider.value : 1;

            //we save the new values
            AudioManager.Instance.SetVolumes(masterVolume, sfxVolume, musicVolume, true);
        }

        /// <summary>
        /// Event fired when sliders change
        /// </summary>
        public void UpdateVolumes()
        {
            float masterVolume, sfxVolume, musicVolume;
            GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);

        }


        public void OnBackButton()
        {
            MenuManager.Instance.PopMenu();
        }


    }
}