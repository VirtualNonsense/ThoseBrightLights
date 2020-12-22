using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SE_Praktikum.Components.Sprites;

namespace SE_Praktikum.Services
{
    public class InteractionManager
    {
        public List<InteractionRule<Actor, Actor>> Rules { get; }
        public readonly List<Actor> Clients;

        public InteractionManager(List<InteractionRule<Actor, Actor>> rules, List<Actor> clients)
        {
            Rules = rules;
            Clients = clients ?? new List<Actor>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                var a = Clients[i];
                if(!a.CollisionEnabled) continue;
                for (var j = i + 1; j < Clients.Count; j++)
                {
                    var b = Clients[j];
                    if(!b.CollisionEnabled) continue;
                    var ab = Rules.Where(rule => rule.A == a.GetType() && rule.B == b.GetType());
                    
                    foreach (var rule in ab)
                    {
                        rule.Action(a, b, gameTime);
                    }
                    if(a.GetType() == b.GetType()) continue;
                    var ba = Rules.Where(rule => rule.A == b.GetType() && rule.B == a.GetType());
                    
                    foreach (var rule in ba)
                    {
                        rule.Action(b, a, gameTime);
                    }
                }
            }
        }
        public void RegisterRule(InteractionRule<Actor, Actor> rule)
        {
            if (Rules.Contains(rule)) return;
            Rules.Add(rule);
        }

        public void RemoveRuleAt(int index)
        {
            Rules.RemoveAt(index);
        }
    }
    
    public class InteractionRule<T, S> where S : Actor where T : Actor 
    {
        public readonly Type A;
        public readonly Type B;
        public readonly Action<T, S, GameTime> Action;

        public InteractionRule(Action<T, S, GameTime> action)
        {
            A = typeof(T);
            B = typeof(S);
            Action = action;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is InteractionRule<Actor, Actor> rule)) return false;
            return Equals(rule);
        }

        private bool Equals(InteractionRule<Actor, Actor> other)
        {
            return A == other.A && B == other.B && Equals(Action, other.Action);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B, Action);
        }
    }
}