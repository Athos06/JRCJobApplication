using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class BuildManager : MonoBehaviour
    {

        private static BuildManager _instance = null;

        public static BuildManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (BuildManager)FindObjectOfType(typeof(BuildManager));
                return _instance;
            }
        }

                /// <summary>
        /// Cursor when you are not in build mode
        /// </summary>
        [SerializeField] Texture2D normalCursor;
        /// <summary>
        /// Cursor when you are building
        /// </summary>
        [SerializeField] Texture2D buildCursor;
        /// <summary>
        /// Hotspot for the cursor icon.
        /// </summary>
        [SerializeField] Vector2 cursorNormalHotspot = new Vector2(0, 0);
        [SerializeField] Vector2 cursorBuildHotspot = new Vector2(0, 0);

        /// <summary>
        /// Tower selected to build
        /// </summary>
        private Tower m_TowerToBuild;
        /// <summary>
        /// Selected building platform
        /// </summary>
        private BuildingPlatform m_SelectedPlatform;

        /// <summary>
        /// Audio source to play the sounds
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// sound clip to use when building towers
        /// </summary>
        public AudioClip buildSound;
        /// <summary>
        /// sound clip to use when selling towers
        /// </summary>
        public AudioClip sellSound;

        /// <summary>
        /// Access to the building platform UI (the UI when we click on some tower on a building platform)
        /// </summary>
        public BuildingPlatformUI buildingPlatformUI;

        private void Update()
        {
            //if we press esc we leave building mode
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) ){
                StopBuilding();
            }
        }

        public bool CanBuild
        {
            get { return m_TowerToBuild != null; }
        }

        public Tower GetTowerToBuild()
        {
            return m_TowerToBuild;
        }

        /// <summary>
        /// Particles effect to play when building the tower.
        /// </summary>
        public ParticleSystem BuildEffect;
        /// <summary>
        /// Particles effect to play when selling the tower.
        /// </summary>
        public ParticleSystem SellEffect;

        private void Start()
        {
            Cursor.SetCursor(normalCursor, cursorNormalHotspot, CursorMode.Auto);
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// We select the tower we want to build
        /// </summary>
        /// <param name="tower">Tower to build</param>
        public void SetTowerToBuild(Tower tower)
        {
            m_TowerToBuild = tower;
            // We change the cursor to the build mode one
            Cursor.SetCursor(buildCursor, cursorBuildHotspot, CursorMode.Auto);
        }

        /// <summary>
        /// Build a tower on the selected platform
        /// </summary>
        /// <param name="platform">Bulding platform where we want to build the tower</param>
        /// <returns></returns>
        public Tower BuildTowerOn(BuildingPlatform platform)
        {
            // If we can affor to build it we do so othwerwise we do nothing
            if (CanAfford())
            {
                MinigameManager.Instance.currency.UpdateCredits(-m_TowerToBuild.Cost);
                Tower towerToBuild = Instantiate<Tower>(m_TowerToBuild, platform.GetBuildPosition(), platform.transform.rotation);
                ParticleSystem particles = Instantiate<ParticleSystem>(BuildEffect, platform.GetBuildPosition(), BuildEffect.transform.rotation);
                Destroy(particles.gameObject, 5f);
                
                //Play the audio sound
                audioSource.clip = buildSound;
                audioSource.Play();
                
                StopBuilding();
                return towerToBuild;
            }

            return null;
        }

        /// <summary>
        /// Sells the tower on this platform.
        /// </summary>
        public void SellTower(BuildingPlatform platform)
        {
            //we add the credits that we get from the sale and we destroy the tower
            MinigameManager.Instance.AddCurrency(platform.GetTower().SellPrice);
            ParticleSystem particles = Instantiate<ParticleSystem>(SellEffect, platform.GetBuildPosition(), SellEffect.transform.rotation);
            Destroy(particles, 1.5f);

            //Play the audio sound
            audioSource.clip = sellSound;
            audioSource.Play();

            platform.SellTower();

        }

        /// <summary>
        /// Called for selecting a specific building platform and showing the UI
        /// </summary>
        /// <param name="buildingPlatform">The building platform we are trying to select</param>
        public void SelectPlatform(BuildingPlatform buildingPlatform)
        {
            //if we click on the same platform that was already selected we deselect it
            if (buildingPlatform == m_SelectedPlatform)
            {
                DeselectBuildingPlatform();
                return;
            }

            m_SelectedPlatform = buildingPlatform;
            buildingPlatformUI.SetTarget(m_SelectedPlatform);
        }

        /// <summary>
        /// We deselect the building platform selected so we hide the UI
        /// </summary>
        public void DeselectBuildingPlatform()
        {
            m_SelectedPlatform = null;
            buildingPlatformUI.Hide();
        }

        /// <summary>
        /// We check if we can afford the cost of the tower we want to build.
        /// </summary>
        /// <returns></returns>
        public bool CanAfford()
        {
            return (MinigameManager.Instance.currency.Credits >= m_TowerToBuild.Cost) ? true : false;
        }

        /// <summary>
        /// Called to leave buidling mode.
        /// </summary>
        public void StopBuilding()
        {
            m_TowerToBuild = null;
            Cursor.SetCursor(normalCursor, cursorNormalHotspot, CursorMode.Auto);
        }

    }
}