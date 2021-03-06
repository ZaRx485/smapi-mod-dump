﻿namespace Better10Hearts.Config
{
    /// <summary>The mod configuration.</summary>
    public class ModConfig
    {
        /// <summary>The amount of stamina the player will gain when talking to an NPC.</summary>
        public int NPCStaminaIncrease { get; set; } = 20;

        /// <summary>The amount of stamina the player will gain when talking to their spouse.</summary>
        public int SpouseStaminaIncrease { get; set; } = 40;

        /// <summary>Determines if the player has to have atleast a 10heart friendship with the NPC to get the stamina gain.</summary>
        public bool OnlyGetStaminaAt10Hearts { get; set; } = true;
    }
}
