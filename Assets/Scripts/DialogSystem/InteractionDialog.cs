using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace JobApplicationGame
{

    /// <summary>
    /// Has to be redone
    /// </summary>
    public class InteractionDialog : MonoBehaviour
    {
        [System.Serializable]
        public struct DialogCallbacks
        {
            public MonoBehaviour callbackScript;
            public string callbackFunction;
            public float delay;
        }

        [System.Serializable]
        public struct Choices
        {
            [System.Serializable]
            public struct ChoiceAnswer
            {
                public int choice;
                public UnityEvent callback;
            }

            public string QuestionID;
            public ChoiceAnswer[] choices;
        }
        /// <summary>
        /// The RPGTalk to be played. Can be left null if you only need to play a timeline director, for instance
        /// </summary>
        public RPGTalk rpgtalkTarget;
        /// <summary>
        /// What is the key that should be used to interact? You can override the Update function and write your 
        /// own conditions, if needed
        /// </summary>
        bool canInteract;

        public Dialog[] dialogs;

        ///// <summary>
        ///// events call when the convertation starts 
        ///// </summary>
        //public UnityEvent callbacksBefore;
        ///// <summary>
        ///// events call when the convertation starts 
        ///// </summary>
        //public UnityEvent callbacksAfter;
        ///// <summary>
        ///// What line to start the talk
        ///// </summary>
        //public string lineToStart = "1";
        ///// <summary>
        ///// What line to finish the talk (leave -1 to read the file until the end)
        ///// </summary>
        //public string lineToBreak = "-1";
        ///// <summary>
        ///// The text file to be parsed into the talks. Leave empty if it is the same as the rpgtalkTarget
        ///// </summary>
        //public TextAsset txtToParse;
        ///// <summary>
        ///// Should this talk/interaction be available only once?
        ///// </summary>
        //public bool happenOnlyOnce;
        //[HideInInspector]
        ///// <summary>
        ///// This talk already happened before?
        ///// </summary>
        //public bool alreadyHappened;
        //public bool shouldStayOnScreen;
        ///// <summary>
        ///// Forbids the talk to be initialized if the rpgtalkTarget is already showing something else.
        ///// </summary>
        //public bool forbidPlayIfRpgtalkIsPlaying;
        ////its a dialog with choices?
        //public bool hasChoices;

        ///// <summary>
        ///// add the dialog choices if there is a dialog with choices
        ///// </summary>
        //public Choices dialogChoices;

        ///// <summary>
        ///// The timeline director can be played with the same rules as the talk.
        ///// </summary>
        //public PlayableDirector timelineDirectorToPlay;

        /// <summary>
        /// The talk will only begin if someone with this tag hits it. Leave blank to accept anyone
        /// </summary>
        public string checkIfColliderHasTag = "";

        [SerializeField]
        private Dialog mNextDialog;


        /// <summary>
        /// Activate talk on trigger enter? (ignored if shouldInteractWithButton is true)
        /// </summary>
        public bool triggerEnter = true;
        /// <summary>
        /// Activate talk on trigger exit? (ignored if shouldInteractWithButton is true)
        /// </summary>
        public bool triggerExit = false;
        /// <summary>
        /// After the talk finishes, the canvas shold stay on the screen?
        /// </summary>

        public bool playOnAwake = false;

        private bool mInteracting = false;



        /// <summary>
        /// Hide anything that shouldn't be showing upon the start
        /// </summary>
        /// 



        public void SetNewDialog(Dialog nextDialog)
        {

            mNextDialog = nextDialog;
        }

        public void SetNewDialog(int id)
        {
            mNextDialog = dialogs[id];
        }

        private void Awake()
        {
            rpgtalkTarget = RPGTalk.Instance;
        }

        protected virtual void Start()
        {
            if (playOnAwake)
            {
                NewTalk();
            }
        }


        /// <summary>
        /// Check for the interaction. Override this method to implement your own rules
        /// </summary>
        protected virtual void Update()
        {

        }

        /// <summary>
        /// Check the rules and put it into rpgtalkTarget, initializing a new talk.
        /// </summary>
        protected virtual void NewTalk()
        {
            if (mNextDialog)
            {

                mNextDialog.StartTalk();
            }
            else
            {
                foreach (Dialog dialog in dialogs)
                {
                    if (!(dialog.AlreadyHappened && dialog.happenOnlyOnce))
                    {
                        dialog.StartTalk();
                        return;
                    }
                }
            }
            //if (rpgtalkTarget == null || (happenOnlyOnce && alreadyHappened) || (forbidPlayIfRpgtalkIsPlaying && rpgtalkTarget.isPlaying))
            //{
            //    return;
            //}



            //alreadyHappened = true;
            //callbacksBefore.Invoke();

            //TextAsset newTxt = rpgtalkTarget.txtToParse;
            //if (txtToParse != null)
            //{
            //    newTxt = txtToParse;
            //}

            //rpgtalkTarget.shouldStayOnScreen = shouldStayOnScreen;


            //rpgtalkTarget.NewTalk(lineToStart, lineToBreak, newTxt, callbacksAfter);

        }


        /// <summary>
        /// Prepare the variables for the interaction or just call a new talk
        /// </summary>
        /// <param name="tagName">Should only work with specifc tag?</param>
        /// <param name="gettingOut">Was called from an OnTriggerExite?</param>
        protected virtual void PrepareInteraction(string tagName, bool gettingOut = false)
        {
            if (tagName == checkIfColliderHasTag || checkIfColliderHasTag == "")
            {
                if ((gettingOut && triggerExit) || (!gettingOut && triggerEnter))
                {
                    StartTalk();
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider col)
        {
            PrepareInteraction(col.tag);
        }

        protected virtual void OnTriggerExit(Collider col)
        {
            PrepareInteraction(col.tag, true);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            PrepareInteraction(col.tag);
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            PrepareInteraction(col.tag, true);
        }

        /// <summary>
        /// Hide anything that should be showing, starts a new talk and plays the timeline director
        /// </summary>
        protected virtual void StartTalk()
        {
            if (rpgtalkTarget != null)
            {

                NewTalk();
            }
            //if (timelineDirectorToPlay != null)
            //{
            //    timelineDirectorToPlay.Play();
            //}
        }

        public virtual void StartInteraction()
        {
            StartTalk();
        }

    }
}