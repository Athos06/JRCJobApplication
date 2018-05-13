using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JobApplicationGame
{
    /// <summary>
    /// Cllas for the UI when we click on a built tower
    /// </summary>
    public class BuildingPlatformUI : MonoBehaviour
    {
        /// <summary>
        /// The building platform we are going to select
        /// </summary>
        private BuildingPlatform m_Target;

        /// <summary>
        /// Selling price text on the UI
        /// </summary>
        public Text sellPrice;
        /// <summary>
        /// Tower name on the UI
        /// </summary>
        public Text towerName;

        /// <summary>
        /// the UI gameobject
        /// </summary>
        public GameObject UI;

        /// <summary>
        /// We select a specific BuildingPlatform as the target
        /// </summary>
        /// <param name="target">The buildingPlatform we are going to select</param>
        public void SetTarget(BuildingPlatform target)
        {
            m_Target = target;

            //We use the building position as the position for the UI
            transform.position = target.GetBuildPosition();

            towerName.text = target.GetTower().name;
            sellPrice.text = target.GetTower().SellPrice + "U";
            UI.SetActive(true);
        }

        /// <summary>
        /// We hide the UI
        /// </summary>
        public void Hide()
        {
            UI.SetActive(false);
        }

        /// <summary>
        /// Callback for when the player press on the sell button.
        /// </summary>
        public void OnSellButton()
        {
            BuildManager.Instance.SellTower(m_Target);
            BuildManager.Instance.DeselectBuildingPlatform();
            
        }

        /// <summary>
        /// Callback for when the player press on the sell button.
        /// </summary>
        public void OnExitButton()
        {
            Hide();
        }
    }
}