using UnityEngine;
using System.Collections;

namespace JobApplicationGame
{

    /// <summary>
    /// Base class representing a specific menu page
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MenuState : MonoBehaviour
    {

        /// <summary>
        /// The system relies on an animator since with the open and close animations the menu will be shown or hidden.
        /// </summary>
        public Animator animator;
        /// <summary>
        /// The canvas group of this menu page to hid or show.
        /// </summary>
        public CanvasGroup canvasGroup;


        /// <summary>
        /// We activate or deactivate the menu.
        /// </summary>
        public bool IsActive
        {
            get { return animator.GetBool("IsOpen"); }
            set { animator.SetBool("IsOpen", value); }
        }

        public void Awake()
        {
            if (animator == null) { animator = GetComponent<Animator>(); }
            if (canvasGroup == null) { canvasGroup = GetComponent<CanvasGroup>(); }
        }

        /// <summary>
        /// We open the menu.
        /// </summary>
        virtual public void Open()
        {
            //activating the menu
            canvasGroup.blocksRaycasts = true;
            IsActive = true;
        }

        /// <summary>
        /// We close the menu
        /// </summary>
        public void Close()
        {
            //deactivating the menu
            canvasGroup.blocksRaycasts = false;
        
            IsActive = false;

        }
    }

}