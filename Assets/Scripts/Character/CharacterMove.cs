using System;
namespace Assets.Scripts
{
    public enum CharacterMoveState
    {
        Idle,
        Movement,
        Attack,
        Block
    }

    public enum CharacterMoveAnimationDirection
    {
        Normal,
        Reverse
    }
    
    // TODO: Convert this to a class, structs are dumb in C#
    public struct CharacterMove
    {
        public string Name;
        public CharacterMoveState State;
        public CharacterMovementDirection TriggerDirection;
        public CharacterAction TriggerAction;
        public string AnimationName;
        public float AnimationSpeed;
        public CharacterMoveAnimationDirection AnimationDirection;
        public bool IsFeintable;
        public int LastFeintFrame;
        public CharacterMovementDirection MovementDirection;
        public int MovementStartFrame;
        public int MovementEndFrame;
        public float MovementSpeedMultiplier;
        
        public override string ToString()
        {
            return Name;
        }
    }
}