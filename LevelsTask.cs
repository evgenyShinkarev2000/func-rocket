using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();

        public static IEnumerable<Level> CreateLevels()
        {
            yield return MakeLevel("Zero");
            
            yield return MakeLevel("heavy", gravity: (size, location) => new Vector(0, 0.9));

            yield return MakeLevel("Up", target: new Vector(700, 500),
                gravity: (size, location) => new Vector(0, -300 / (size.Height - location.Y + 300)));

            var whiteHoleTargetPosition = MakeDefaultTarget();
            yield return MakeLevel("WhiteHole",
                gravity: (size, location) => MakeForceVector(whiteHoleTargetPosition, location, -140)
            );

            var blackHolePosition = MakeDefaultRocket().Location / 2 + MakeDefaultTarget() / 2;
            yield return MakeLevel("BlackHole",
                gravity: (size, location) => MakeForceVector(blackHolePosition, location, 300));

            yield return MakeLevel("BlackAndWhite", gravity: (size, location) =>
                MakeForceVector(whiteHoleTargetPosition, location, -140) / 2
                + MakeForceVector(blackHolePosition, location, 300) / 2);
        }

        /// <summary>
        /// Make vector co-directional to begin, if forceFactor > 0 
        /// </summary>
        private static Vector MakeForceVector(Vector begin, Vector end, int forceFactor)
        {
            var distanceVector = begin - end;
            var distance = distanceVector.Length;
            return distanceVector.Normalize() * forceFactor * distance / (distance * distance + 1);
        }

        private static Rocket MakeDefaultRocket()
            => new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
        
        private static Vector MakeDefaultTarget()
            => new Vector(600, 200);

        private static Level MakeLevel(string name, Rocket rocket = null, Vector target = null,
            Gravity gravity = null, Physics physics = null)
        {
            if (name == null) throw new ArgumentException();
            if (rocket == null) rocket = MakeDefaultRocket();
            if (target == null) target = MakeDefaultTarget();
            if (gravity == null) gravity = (size, v) => Vector.Zero;
            if (physics == null) physics = standardPhysics;
            
            return new Level(name, rocket, target, gravity, physics); 
        }
    }
}