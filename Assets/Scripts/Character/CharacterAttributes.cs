using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class CharacterAttributes
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float Stamina { get; set; }
        public float Strength { get; set; }
        public float Speed { get; set; }
        public float Defense { get; set; }
        public List<CharacterMove> Moves { get; set; } = new List<CharacterMove>();
    }
}