using UnityEngine;
namespace Assets.Scripts
{
    public abstract class PlayerState
    {
        protected PlayerStateController StateController { get; private set; }
        protected PlayerController PlayerController { get; private set; }
        protected Animator PlayerAnimator { get; private set; }

        public PlayerState(PlayerStateController stateController)
        {
            StateController = stateController;
            PlayerController = stateController.PlayerController;
            PlayerAnimator = PlayerController.GetComponent<Animator>();
        }
        // Since PlayerStateController ChangeState() is called from another state's
        // UpdateStateMethod(), the current state's Update method is not called on first frame of state
        // transition.  Therefore any init or 1st frame logic should go into OnEnter().
        public abstract void OnEnter();

        // As stated in UpdateState() comment, it is recommended to return from a state change immediately,
        // therefore any clean up or logic that must be done when leaving the current state should go into
        // OnExit();
        public abstract void OnExit();

        // UpdateState() is called every Update() to PlayerStateController.
        // Flow in UpdateState() should be:
        //   1. Check for state changes.  If changing state return immediately.
        //   2. State update logic.
        //   3. No state entry or state exit logic should be in UpdateState() as UpdateState() is not run
        //      until the frame after state transition.
        public abstract void UpdateState();

        public abstract void OnCollisionEnter(Collision collision);
    }
}