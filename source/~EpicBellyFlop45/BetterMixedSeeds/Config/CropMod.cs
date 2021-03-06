﻿namespace BetterMixedSeeds.Config
{
    /// <summary>Data about an integrated mod.</summary>
    public class CropMod
    {
        /// <summary>The season object that contains spring crops.</summary>
        public Season Spring { get; set; }

        /// <summary>The season object that contains summer crops.</summary>
        public Season Summer { get; set; }

        /// <summary>The season object that contains fall crops.</summary>
        public Season Fall { get; set; }

        /// <summary>The season object that contains winter crops.</summary>
        public Season Winter { get; set; }

        /// <summary>Construct an instance.</summary>
        /// <param name="spring">The season object that contains spring crops.</param>
        /// <param name="summer">The season object that contains summer crops.</param>
        /// <param name="fall">The season object that contains fall crops.</param>
        /// <param name="winter">The season object that contains winter crops.</param>
        public CropMod(Season spring, Season summer, Season fall, Season winter)
        {
            Spring = spring;
            Summer = summer;
            Fall = fall;
            Winter = winter;
        }
    }
}
