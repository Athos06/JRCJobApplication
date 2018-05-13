using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JobApplicationGame
{
    public class PlayerControl : MonoBehaviour
    {
        private bool controlsLocked = false;
        private bool invalidClick = false;

        Character character;
        InteractionSystem interactionSys;
        //test
        //private float interactDistance = 2.0f;
        

        private Interactable target;

        // Use this for initialization
        void Start()
        {
            character = GetComponent<Character>();
            interactionSys = GetComponent<InteractionSystem>();

            RegisterForMouseEvents();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void RegisterForMouseEvents()
        {
            var cameraRayCaster = FindObjectOfType<CameraRayCaster>();
            cameraRayCaster.onMouseOverInteractable += OnMouseOverInteractable;
            cameraRayCaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }


        //bool IsTargetInRange(GameObject target)
        //{
        //    float distanceToTarget = (target.transform.position - transform.position).magnitude;
        //    return distanceToTarget <= interactDistance;
        //}

       
        // We want to ignore the click used to close to dialog and not using it as a movement click. 
        bool CheckInvalidClick()
        {
            //the first click after lock is always invalid (because is the one we use to close interaction) only after you release the button it allow us to click again to move
            if (!Input.GetMouseButton(0) && invalidClick)
            {
                invalidClick = false;
                return false;
            }

            return invalidClick ? true : false; 
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
    
            if (controlsLocked){
                return;
            }

            if (CheckInvalidClick()) { return; }


            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }
        
        void OnMouseOverInteractable(Interactable interactable)
        {
            if (controlsLocked) { return; }

            if (CheckInvalidClick()) { return; }

            if (Input.GetMouseButton(0) && interactable.OnRange) //IsTargetInRange(interactable.gameObject))
            {
                interactionSys.StartInteraction(interactable);
                //if we are moving and we click on something in range to interact we stop in the current position
                character.SetDestination(transform.position);
                transform.LookAt(interactable.transform.position);
            }
            else if (Input.GetMouseButton(0) && !interactable.OnRange)//!IsTargetInRange(interactable.gameObject))
            {
                StartCoroutine(MoveAndInteract(interactable));
            }
            
        }

        public void LockControl()
        {
            controlsLocked = true;
            invalidClick = true;
        }

        public void UnlockControl()
        {
            controlsLocked = false;
        }

        IEnumerator MoveToTarget(Interactable interactionTarget)
        {
            target = interactionTarget;

            character.SetDestination(interactionTarget.transform.position);

            //We keep moving to the destination until we reach the destination in range
            while (!interactionTarget.OnRange)
            {
                yield return new WaitForEndOfFrame();
                // If the target changed (because the player clicked on another one) stop the coroutine
                if (target != interactionTarget) { yield break; }
            }
            //then we stop when we are in range to interact
            character.SetDestination(transform.position);

            transform.LookAt(interactionTarget.transform.position);
            //yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndInteract(Interactable interactable)
        {
            yield return StartCoroutine(MoveToTarget(interactable));
            // If the target changed (because the player clicked on another one) stop the coroutine
            if (target != interactable) { yield break; }

            interactionSys.StartInteraction(interactable);
        }
    }
}