using UnityEngine;

namespace JobApplicationGame
{
    /// <summary>
    /// The platforms where the player can build towers
    /// </summary>
    public class BuildingPlatform : MonoBehaviour
    {



        /// <summary>
        /// Offset to build the tower on the platform so its on the surface of it.
        /// </summary>
        public Vector3 positionOffset;
        /// <summary>
        /// Tower that is built in this platform.
        /// </summary>
        private Tower m_Tower;

        /// <summary>
        /// Color to which the platform will change when the mouse is on it to indicate we can build.
        /// </summary>
        public Color hoverColorGood;
        /// <summary>
        /// Color to which the platform will change when the mouse is on it to indicate we can't build.
        /// </summary>
        public Color hoverColorBad;
        /// <summary>
        /// Renderer of the platform.
        /// </summary>
        private Renderer m_Renderer;
        /// <summary>
        /// The initial color of the material, to go back to it when the mouse is not hovering it.
        /// </summary>
        private Color m_StartColor;

        // Use this for initialization
        void Start()
        {
            m_Renderer = GetComponent<Renderer>();
            m_StartColor = m_Renderer.material.color;
        }

        
        /// <summary>
        /// Get the position where to build on this platform.
        /// </summary>
        /// <returns>The Vector3 position where to build</returns>
        public Vector3 GetBuildPosition()
        {
            return transform.position + positionOffset;
        }

        /// <summary>
        /// Get the tower that is build on this platform.
        /// </summary>
        /// <returns>The tower built on the platform. If there is no tower build it returns null</returns>
        public Tower GetTower()
        {
            return m_Tower;
        }

        /// <summary>
        /// Sells the tower on this platform.
        /// </summary>
        public void SellTower()
        {
            if (m_Tower == null) { Debug.LogError("the tower should never be null now that you are selling"); }
            Destroy(m_Tower.gameObject);
            m_Tower = null;
        }

        void OnMouseDown()
        {
            // If there is a tower on the platform first and we click on it we select that platform (to show the UI for the tower for instance)
            if (m_Tower != null)
            {
                BuildManager.Instance.SelectPlatform(this);
                return;
            }
            // If we cannot build in the platform we dont do anything
            if (!BuildManager.Instance.CanBuild) { return; }

            // If the platform is empty and we can build on it we build the tower
            m_Tower = BuildManager.Instance.BuildTowerOn(this);
            m_Renderer.material.color = m_StartColor;

        }

        void OnMouseOver()
        {
            //If there is already a tower we dont want to do anything on hover, the player cannot build so there is no reason for showing building visual clues
            if (m_Tower) { return; }

            if (!BuildManager.Instance.CanBuild) { return; }

            //If we can afford to build the tower we show green color
            if (BuildManager.Instance.CanAfford())
            {
                m_Renderer.material.color = hoverColorGood;
            }
            // Otherwise we show the red color
            else
            {
                m_Renderer.material.color = hoverColorBad;
            }

        }
        //when we are not hovering we go back to the original color
        void OnMouseExit()
        {
            m_Renderer.material.color = m_StartColor;

        }
    }
}