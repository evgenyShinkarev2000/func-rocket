using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var targetDirectAngle = (target - rocket.Location).Angle;
            var currentSpeedAngle = rocket.Velocity.Angle;
            var maxSpeedAngleDelta = Math.PI * 0.6;
            var epsilon = 3e-2;

            var wantedRocketDirectionAngle = targetDirectAngle - (currentSpeedAngle - targetDirectAngle) % maxSpeedAngleDelta;

            if (Math.Abs(rocket.Direction - wantedRocketDirectionAngle) < epsilon)
                return Turn.None;

            if (rocket.Direction < wantedRocketDirectionAngle)
                return Turn.Right;

            return Turn.Left;

            //return rocket.Direction < (2 * (target - rocket.Location).Angle - rocket.Velocity.Angle) % (Math.PI * 0.9) ? Turn.Right : Turn.Left;

            //return (Turn)(int)(((target - rocket.Location).Angle - (rocket.Direction + rocket.Velocity.Angle) / 2) / Math.Abs((target - rocket.Location).Angle - (rocket.Direction + rocket.Velocity.Angle) / 2));
        }
	}
}