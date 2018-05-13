using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JobApplicationGame
{
    /// <summary>
    /// Class for the side shop to buy towers
    /// </summary>
    public class Shop : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the ballista tower
        /// </summary>
        public Tower tower1;
        /// <summary>
        /// Prefab for the cannon tower
        /// </summary>
        public Tower tower2;
        
        /// <summary>
        /// Called when we click on the ballista tower button
        /// </summary>
        public void PurchaseTower()
        {
            BuildManager.Instance.SetTowerToBuild(tower1);
        }

        /// <summary>
        /// Called when we click on the ballista tower button
        /// </summary>
        public void PurchaseTower2()
        {
            BuildManager.Instance.SetTowerToBuild(tower2);
        }

        //public void UnlockTower1()
        //{
        //    if (Tower1ButtonPrefab == null)
        //    {
        //        Debug.LogError("The prefab of the tower 1 button its not initialized");
        //        return;
        //    }

        //    Button UnlockedTowerButton = Instantiate<Button>(Tower1ButtonPrefab, UIShopParent);
        //    UnlockedTowerButton.onClick.AddListener(delegate { PurchaseTower(); });

        //}
        
    }
}