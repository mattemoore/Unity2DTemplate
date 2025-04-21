using UnityEngine;
namespace Assets.Scripts
{
    public abstract class CharacterState
    {
        public readonly CharacterStateMachine StateController;
        public Character Character => StateController.Character;
        public readonly Animator CharacterAnimator; 
        public readonly CharacterMove CharacterMove; 
        public readonly bool IsInterruptible;
        
        public CharacterState(CharacterStateMachine stateController, CharacterMove characterMove, bool isInterruptible)
        {
            CharacterMove = characterMove;
            StateController = stateController;
            CharacterAnimator = Character.GetComponent<Animator>();
            CharacterMove = characterMove;
            IsInterruptible = isInterruptible;
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

        public void PlayAnimation()
        {
            int speedMult = CharacterMove.AnimationDirection == CharacterMoveAnimationDirection.Normal ? 1 : -1;
            float animationStart = CharacterMove.AnimationDirection == CharacterMoveAnimationDirection.Normal ? 0 : 1;
            CharacterAnimator.SetFloat(Animator.StringToHash("AnimationSpeed"), CharacterMove.AnimationSpeed * speedMult);
            CharacterAnimator.speed = 1.0f;
            CharacterAnimator.CrossFade(CharacterMove.AnimationName, 0.0f, -1, animationStart);
        }

        public bool IsAnimationFinished()
        {
            AnimatorStateInfo stateInfo = CharacterAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(CharacterMove.AnimationName) && stateInfo.normalizedTime >= 0.99999f;
        }

        public int GetCurrentAnimationFrame()
        {
            AnimatorStateInfo stateInfo = CharacterAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(CharacterMove.AnimationName))
            {
                float frame = stateInfo.normalizedTime * stateInfo.length * CharacterAnimator.runtimeAnimatorController.animationClips[0].frameRate;
                return Mathf.RoundToInt(frame);
            }
            return -1; // Return -1 if the animation is not currently playing
        }
    }
}