using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
            => rocket.Direction<(2 * (target - rocket.Location).Angle - rocket.Velocity.Angle) % (Math.PI* 0.9) ? Turn.Right : Turn.Left;
	}
}