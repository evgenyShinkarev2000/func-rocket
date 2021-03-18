using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace func_rocket
{
    public class LevelsTask
    {
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
        
        private static Gravity MakeZeroGravity()
            => (size, location) => Vector.Zero;

        private static Level MakeLevel(string name, Rocket rocket = null, Vector target = null,
            Gravity gravity = null, Physics physics = null)
            => new Level(
                name ?? throw new ArgumentException(),
                rocket ?? MakeDefaultRocket(),
                target ?? MakeDefaultTarget(),
                gravity ?? MakeZeroGravity(),
                physics ?? new Physics());
    }
}