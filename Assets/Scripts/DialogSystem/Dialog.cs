using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace JobApplicationGame
{
    /// <summary>
    /// The whole dialog system is just a prototype mess that will have to be redone for a serious project
    /// </summary>
    [System.Serializable]
    public class Dialog : MonoBehaviour
    {

        [System.Serializable]
        public struct DialogCallbacks
        {
            public MonoBehaviour callbackScript;
            public string callbackFunction;
            public float delay;
        }

        [System.Serializable]
        public struct ChoiceAnswer
        {
            public int myChoiceID;
            public UnityEvent callback;
        }

        [System.Serializable]
        public struct Choices
        {
            public int myQuestionID;
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




        /// <summary>
        /// events call when the convertation starts 
        /// </summary>
        public UnityEvent callbacksBefore;
        /// <summary>
        /// events call when the convertation starts 
        /// </summary>
        public UnityEvent callbacksAfter;
        /// <summary>
        /// What line to start the talk
        /// </summary>
        public string lineToStart = "1";
        /// <summary>
        /// What line to finish the talk (leave -1 to read the file until the end)
        /// </summary>
        public string lineToBreak = "-1";
        /// <summary>
        /// The text file to be parsed into the talks. Leave empty if it is the same as the rpgtalkTarget
        /// </summary>
        public TextAsset txtToParse;
        /// <summary>
        /// Should this talk/interaction be available only once?
        /// </summary>
        public bool happenOnlyOnce;
        /// <summary>
        /// This talk already happened before?
        /// </summary>
        private bool mAlreadyHappened = false;
        public bool AlreadyHappened
        {
            get { return mAlreadyHappened; }
            set { mAlreadyHappened = value; }
        }
        /// <summary>
        /// After the talk finishes, the canvas shold stay on the screen?
        /// </summary>
        public bool shouldStayOnScreen;
        /// <summary>
        /// Forbids the talk to be initialized if the rpgtalkTarget is already showing something else.
        /// </summary>
        public bool forbidPlayIfRpgtalkIsPlaying;
        //its a dialog with choices?
        public bool hasChoices;

        /// <summary>
        /// add the dialog choices if there is a dialog with choices
        /// </summary>
        public Choices dialogChoices;

        /// <summary>
        /// The timeline director can be played with the same rules as the talk.
        /// </summary>
        public PlayableDirector timelineDirectorToPlay;

        private void Awake()
        {
            rpgtalkTarget = RPGTalk.Instance;
        }
        private void Start()
        {
            rpgtalkTarget.OnMadeChoice += OnMadeChoice;
        }

        /// <summary>
        /// Check the rules and put it into rpgtalkTarget, initializing a new talk.
        /// </summary>
        protected virtual void NewTalk()
        {
            if (rpgtalkTarget == null || (happenOnlyOnce && mAlreadyHappened) || (forbidPlayIfRpgtalkIsPlaying && rpgtalkTarget.isPlaying))
            {
                return;
            }

            mAlreadyHappened = true;
            callbacksBefore.Invoke();

            TextAsset newTxt = rpgtalkTarget.txtToParse;
            if (txtToParse != null)
            {
                newTxt = txtToParse;
            }

            rpgtalkTarget.shouldStayOnScreen = shouldStayOnScreen;

            rpgtalkTarget.NewTalk(lineToStart, lineToBreak, newTxt, callbacksAfter);

        }


        /// <summary>
        /// Hide anything that should be showing, starts a new talk and plays the timeline director
        /// </summary>
        public virtual void StartTalk()
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


        void OnMadeChoice(int questionID, int choiceID)
        {
            if (hasChoices)
            {
                if (dialogChoices.myQuestionID == questionID)
                {
                    foreach (ChoiceAnswer choice in dialogChoices.choices)
                    {
                        if (choice.myChoiceID == choiceID) { choice.callback.Invoke(); return; }
                    }
                }
            }
        }
    }
}