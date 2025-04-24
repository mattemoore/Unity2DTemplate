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
    
    public class CharacterMove
    {
        public string Name { get; set; }
        public CharacterMoveState State { get; set; }
        public CharacterMovementDirection TriggerDirection { get; set; }
        public CharacterAction TriggerAction { get; set; }
        public string AnimationName { get; set; }
        public float AnimationSpeed { get; set; }
        public CharacterMoveAnimationDirection AnimationDirection { get; set; }
        public bool IsFeintable { get; set; }
        public int LastFeintFrame { get; set; }
        public CharacterMovementDirection MovementDirection { get; set; }
        public int MovementStartFrame { get; set; }
        public int MovementEndFrame { get; set; }
        public float MovementSpeedMultiplier { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}