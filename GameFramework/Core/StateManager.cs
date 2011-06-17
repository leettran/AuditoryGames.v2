using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace LSRI.AuditoryGames.GameFramework
{
    /// <summary>
    /// Data structure for registering information about logical states
    /// </summary>
    public class StateChangeInfo
    {
        /// <summary>
        /// Definition of a delegate for the state functions.
        /// </summary>
        public delegate void StateFunction();

        /// <summary>
        /// Reference to the entry function associated with the state
        /// </summary>
        public StateFunction enterState = null;

        /// <summary>
        /// Reference to the exit function associated with the state
        /// </summary>
        public StateFunction exitState = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="enterState">A reference to the state entry function</param>
        /// <param name="exitState">A reference to the state exit function</param>
        public StateChangeInfo(StateFunction enterState, StateFunction exitState)
        {
            this.enterState = enterState;
            this.exitState = exitState;
        }
    }

    /// <summary>
    /// The different logical states of the game.
    /// 
    /// The State Manager requires two default state: START_STATE to initialise the game, END_STATE to end the game.
    /// Each game will have to define its own states on top of the default ones.
    /// </summary>
    public class States
    {
        public const string START_STATE = "start";  ///< Game is starting
        public const string END_STATE = "end";      ///< Game is ending
    }

    /// <summary>
    /// Manages the transitions between the different logical states of the game
    /// </summary>
    public class StateManager
    {
        protected static StateManager instance = null;

        /// <summary>
        /// The name of the current state.
        /// </summary>
	    protected string currentState =  string.Empty;

        /// <summary>
        /// The name of the new state to switch to.
        /// </summary>
	    protected string newState = string.Empty;

	    /// <summary>
        /// The list of state functions to be called when a state is changed
	    /// </summary>
	    Dictionary<string, List<StateChangeInfo>> stateChangeMap = new Dictionary<string,List<StateChangeInfo>>();

	    /// <summary>
        /// TRUE if subsequent calles to SetState are queued (to allow state change function to be called in the correct order) 
	    /// </summary>
        bool stateChangeLocked;

        /// <summary>
        /// List of pending requests for state change
        /// </summary>
	    List<string> pendingStateChangeRequests = new List<string>();
	    
        /// <summary>
        /// List of new state changes
        /// </summary>
	    List<string> newStateChangeRequests = new List<string>();

        /// <summary>
        /// Access to the singleton of the State Manager
        /// </summary>
        public static StateManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new StateManager();
                return instance;
            }
        }

        /// <summary>
        /// The name of the current state
        /// </summary>
        public string CurrentState
        {
            get
            {
                return currentState;
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        protected StateManager()
        {

        }

        /// <summary>
        /// Initialise the state manager. 
        /// </summary>
        public void startupStateManager()
        {
	        setState(States.START_STATE);
        }

        /// <summary>
        /// Shut down the state manager
        /// </summary>
        public void shutdown()
        {
            endCurrentState();
        }
        
        /// <summary>
        /// End the current state.
        /// </summary>
        public void endCurrentState()
        {
	        setState(States.END_STATE);
        }

        /// <summary>
        /// Process the requests for changing states
        /// </summary>
        protected void updateStates()
        {
	        // we can only change the states if stateChangeLocked is false (i.e. the state change
	        // isn't called from one of the enter or exit state functions)
	        if (!stateChangeLocked)
	        {
		        // set the lock
		        stateChangeLocked = true;

		        // push all our new state changes onto the pending state change list
                foreach (string info in newStateChangeRequests)
			        pendingStateChangeRequests.Add(info);

		        newStateChangeRequests.Clear();

		        // now process the pending state changes
		        while (pendingStateChangeRequests.Count != 0)
		        {
			        foreach (string stateChangeRequest in pendingStateChangeRequests)
			        {
				        if (!stateChangeRequest.Equals(currentState))
				        {
					        // call the exit function for all state listeners that are in the current state
                            if (stateChangeMap.ContainsKey(currentState))
                            {
                                foreach (StateChangeInfo info in stateChangeMap[currentState])
				                {
					                info.exitState();
				                }
                            }

					        if (!stateChangeRequest.Equals(States.END_STATE))
					        {						    
                                if (stateChangeMap.ContainsKey(stateChangeRequest))
                                {
                                    foreach (StateChangeInfo info in stateChangeMap[stateChangeRequest])
				                    {
					                    info.enterState();
				                    }
                                }
					        }
				        }

				        currentState = stateChangeRequest;
			        }

			        // all current pending state changes have been processed, so clear the list
			        pendingStateChangeRequests.Clear();

			        // push all our new state changes onto the pending state change list
                    foreach (string info in newStateChangeRequests)
			            pendingStateChangeRequests.Add(info);

			        newStateChangeRequests.Clear();
		        }

		        stateChangeLocked = false;
	        }
        }

        /// <summary>
        /// Request a transition to the specified state
        /// </summary>
        /// <param name="newState">the name of the state to switch to</param>
        public void setState(string newState)
        {
	        // we have to jump through some hoops to ensure that all state change requests are
	        // processed in order because it is possible to change the state from within
	        // an enter or exit state function. To do this we add all state requests to 
	        // newStateChangeRequests, lock the state change loop, copy all requests
	        // from newStateChangeRequests to pendingStateChangeRequests and then
	        // process the state changes in pendingStateChangeRequests.
        	
	        // add our requested state change to the newStateChangeRequests vector
	        newStateChangeRequests.Add(newState);
        	
	        updateStates();
        }

        /// <summary>
        /// Register the different states of the game.
        /// 
        /// Each state is defined by a unique name and associated with two functions: enterState and exitState. 
        /// These function will be called repsectively when a transition into and away from the defined state occurs.
        /// </summary>
        /// <param name="state">The name of the state to register</param>
        /// <param name="enterState">The function to be called when the State Manager switches to this state</param>
        /// <param name="exitState">The function to be called when the State Managet switches leaves this state</param>
        public void registerStateChange(string state, StateChangeInfo.StateFunction enterState, StateChangeInfo.StateFunction exitState)
        {
            if (!state.Equals(States.END_STATE))
            {
                if (!isRegisteredState(state))
                    stateChangeMap.Add(state, new List<StateChangeInfo>());
                stateChangeMap[state].Add(new StateChangeInfo(enterState, exitState));
            }
        }

        /// <summary>
        /// check whether the given name is a proper state registered with the State Manager
        /// </summary>
        /// <param name="state">The name of a state to check</param>
        /// <returns>TRUE if the state is properly registered, FALSE otherwise</returns>
        public bool isRegisteredState(string state)
        {
	        return stateChangeMap.ContainsKey(state);
        }   
    }
}
