using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JobApplicationGame
{   
    /// <summary>
    /// Class that queries the scenary to allow player movement and interaction with the environment.
    /// </summary>
    public class CameraRayCaster : MonoBehaviour
    {

        /// <summary>
        /// Cursor icon for player movement.
        /// </summary>
        [SerializeField] Texture2D walkCursor;
        /// <summary>
        /// Cursor icon for player movement.
        /// </summary>
        [SerializeField] Texture2D interactCursor;
        /// <summary>
        /// Cursor hotspot in pixels from the top left of the walk cursor.
        /// </summary>
        [SerializeField] Vector2 walkCursorHotspot = new Vector2(0, 0);
        /// <summary>
        /// Cursor hotspot in pixels from the top left of the interact cursor.
        /// </summary>
        [SerializeField] Vector2 interactCursorHotspot = new Vector2(0, 0);
        /// <summary>
        /// Layer of object the player can walk on (8 is hardcoded number of the layer within unity that you created witha add layer in the editor).
        /// </summary>
        const int POTENTIALLY_WALKABLE_LAYER = 8;
        /// <summary>
        /// max depth for the raycast we use (hardcoded). 
        /// </summary>
        float maxRaycastDepth = 100f;

        /// <summary>
        /// Rect of the current size of the screen, to check if the cursor is in the screen.
        /// </summary>
        Rect currentScreenRect;


        /// <summary>
        /// Delegate to use when the cursor is on some object we can interact with.
        /// </summary>
        /// <param name="InteractbleObject">The interactable object we can interact with.</param>
        public delegate void OnMouseOverInteractable(Interactable InteractbleObject);
        /// <summary>
        /// Event that will be called when cursor is on some interactable object.
        /// </summary>
        public event OnMouseOverInteractable onMouseOverInteractable;

        // TODO --clean here
        ///// <summary>
        ///// Delegate to use when the cursor is on some object we can walk on.
        ///// </summary>
        ///// <param name="destination">Destination position </param>
        //public delegate void OnMouseOverWalkable(Vector3 destination);
        ///// <summary>
        ///// Event that will be called when cursor is on some walkable path.
        ///// </summary>
        //public event OnMouseOverWalkable onMouseOverWalkable;



        ///// <summary>
        ///// Delegate to use when the cursor is on some object we can walk on.
        ///// </summary>
        ///// <param name="destination">Destination position </param>
        public delegate void OnMouseOverTerrain(Vector3 destination);
        ///// <summary>
        ///// Event that will be called when cursor is on some walkable path.
        ///// </summary>
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        // some hack to solve a bug caused when i was trying to change the cursor every icon everyframe
        private enum CursorState { walk, interact };
        CursorState cursorState ;

        private void Start()
        {
            cursorState = CursorState.walk;
            Cursor.SetCursor(walkCursor, walkCursorHotspot, CursorMode.Auto);
        }

        // Update is called once per frame
        void Update()
        {
            currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);

            //if the cursor is over some UI element we dont want to perform any raycast
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //default cursor is walk cursor. We always change it if it wasnt already set to it
                if (cursorState != CursorState.walk)
                {
                    Cursor.SetCursor(walkCursor, walkCursorHotspot, CursorMode.Auto);
                    cursorState = CursorState.walk;
                }
            }
            else
            {
                PerformRaycasts();
            }




        }

        /// <summary>
        /// We will do the raycast on the scenario.
        /// </summary>
        void PerformRaycasts()
        {

            // If the mouse is inside the game screen
            if (currentScreenRect.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //the order of layer priorities matters, first interctable and if we dont find any then walkable
                if (RaycastForInteractable(ray)) { return; }
                if (RaycastForPotenciallyWalkable(ray)) { return;  }

                //default cursor is walk cursor. We always change it if it wasnt already set to it
                if (cursorState != CursorState.walk)
                {
                    Cursor.SetCursor(walkCursor, walkCursorHotspot, CursorMode.Auto);
                    cursorState = CursorState.walk;
                }
            }
        }

        /// <summary>
        /// check if the cursor is over an interactable object.
        /// </summary>
        /// <param name="ray">Ray parameter used for the raycast.</param>
        /// <returns>returns true if it found an interctable object.</returns>
        private bool RaycastForInteractable(Ray ray)
        {
            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo, maxRaycastDepth)) { return false; }

            var gameObjectHit = hitInfo.collider.gameObject;
            var interactableHit = gameObjectHit.GetComponent<Interactable>();
            //if we find some interctable object we will call the event
            if (interactableHit)
            {
                if(cursorState != CursorState.interact) { 
                    Cursor.SetCursor(interactCursor, interactCursorHotspot, CursorMode.Auto);
                    cursorState = CursorState.interact;
                }

                onMouseOverInteractable(interactableHit);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the cursor if over terrain we can walk on.
        /// </summary>
        /// <param name="ray">Ray parameter used for the raycast.</param>
        /// <returns>returns true if it found an walkable object.</returns>
        private bool RaycastForPotenciallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potenciallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potenciallyWalkableLayer);
            //if we find some walkable path we will call the event
            if (potentiallyWalkableHit)
            {
                if (cursorState != CursorState.walk)
                {
                    Cursor.SetCursor(walkCursor, walkCursorHotspot, CursorMode.Auto);
                    cursorState = CursorState.walk ;
                }

                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }



    }

}