﻿using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace The_Ball_Is_Angry
{
    public static class MyTargetSelector
    {
        public static float Range
        {
            get { return  SpellManager.W.Range; }
        }

        public static AIHeroClient Target { get; private set; }
        
        public static IEnumerable<AIHeroClient> ValidEnemies;

        public static IEnumerable<AIHeroClient> ValidEnemiesInRange;

        public static void Initialize()
        {
            ValidEnemies = EntityManager.Heroes.Enemies.Where(h => h.IsValidTarget() && !h.HasUndyingBuff()).OrderByDescending(TargetSelector.GetPriority);
            ValidEnemiesInRange = ValidEnemies.Where(h => Util.MyHero.IsInRange(h, 1700));
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            ValidEnemies = EntityManager.Heroes.Enemies.Where(h => h.IsValidTarget() && !h.HasUndyingBuff()).OrderByDescending(TargetSelector.GetPriority);
            ValidEnemiesInRange = ValidEnemies.Where(h => Util.MyHero.IsInRange(h, 1700));
            var aiHeroClients = ValidEnemiesInRange as IList<AIHeroClient> ?? ValidEnemiesInRange.ToList();
            var rangeSqr = Range.Pow();
            Target = TargetSelector.GetTarget(aiHeroClients.Where(h => Util.MyHero.Distance(h, true) <= rangeSqr), DamageType.Physical);
        }
    }
}