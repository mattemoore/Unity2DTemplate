using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class KarateMan : CharacterAttributes
    {
        public KarateMan()
        {
            Name = "Karate Man";
            Stamina = 100;
            Speed = 2;
            Health = 1000;
            Strength = 10;
            Moves = new List<CharacterMove>
            {
                new CharacterMove
                {
                    Name = "Idle",
                    State = CharacterMoveState.Idle,
                    TriggerDirection = CharacterMovementDirection.None,
                    TriggerAction = CharacterAction.None,
                    AnimationName = "Idle",
                    AnimationSpeed = 1.0f,
                    AnimationDirection = CharacterMoveAnimationDirection.Normal,
                    IsFeintable = false,
                    LastFeintFrame = 0,
                    MovementDirection = CharacterMovementDirection.None,
                    MovementStartFrame = 0,
                    MovementEndFrame = 0,
                    MovementSpeedMultiplier = 0.0f
                },
                new CharacterMove
                {
                    Name = "Walk Forward",
                    State = CharacterMoveState.Movement,
                    TriggerDirection = CharacterMovementDirection.Forward,
                    TriggerAction = CharacterAction.None,
                    AnimationName = "Walk",
                    AnimationSpeed = 1.0f,
                    AnimationDirection = CharacterMoveAnimationDirection.Normal,
                    IsFeintable = false,
                    LastFeintFrame = 0,
                    MovementDirection = CharacterMovementDirection.Forward,
                    MovementStartFrame = 0,
                    MovementEndFrame = 0,
                    MovementSpeedMultiplier = 1.0f
                },
                new CharacterMove
                {
                    Name = "Walk Backward",
                    State = CharacterMoveState.Movement,
                    TriggerDirection = CharacterMovementDirection.Backward,
                    TriggerAction = CharacterAction.None,
                    AnimationName = "Walk",
                    AnimationSpeed = 1.0f,
                    AnimationDirection = CharacterMoveAnimationDirection.Reverse,
                    IsFeintable = false,
                    LastFeintFrame = 0,
                    MovementDirection = CharacterMovementDirection.Backward,
                    MovementStartFrame = 0,
                    MovementEndFrame = 0,
                    MovementSpeedMultiplier = 1.0f,
                },
                new CharacterMove
                {
                    Name = "High Kick",
                    State = CharacterMoveState.Attack,
                    TriggerDirection = CharacterMovementDirection.None,
                    TriggerAction = CharacterAction.Action1,
                    AnimationName = "HighKick",
                    AnimationSpeed = 1.0f,
                    AnimationDirection = CharacterMoveAnimationDirection.Normal,
                    IsFeintable = true,
                    LastFeintFrame = 4,
                    MovementDirection = CharacterMovementDirection.None,
                    MovementStartFrame = 0,
                    MovementEndFrame = 0,
                    MovementSpeedMultiplier = 1.0f
                },
                new CharacterMove
                {
                    Name = "Front Kick",
                    State = CharacterMoveState.Attack,
                    TriggerDirection = CharacterMovementDirection.Forward,
                    TriggerAction = CharacterAction.Action1,
                    AnimationName = "FrontKick",
                    AnimationSpeed = 1.0f,
                    AnimationDirection = CharacterMoveAnimationDirection.Normal,
                    IsFeintable = true,
                    LastFeintFrame = 4,
                    MovementDirection = CharacterMovementDirection.Forward,
                    MovementStartFrame = 0,
                    MovementEndFrame = 3,
                    MovementSpeedMultiplier = 2.0f,
                }
            };
        }
    }
}