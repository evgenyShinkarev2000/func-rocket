using System;
using System.Diagnostics;
using System.Drawing;

namespace func_rocket
{
	public class ControlTask
	{
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var directToTarget = target - rocket.Location;
			var reflectionRocketSpeed = FindSpeedReflection(rocket, directToTarget);
			var noseDirection = new Vector(Math.Sin(-rocket.Direction + Math.PI / 2),
				Math.Cos(-rocket.Direction + Math.PI / 2));
			
			var angleTargetNose = FindAngle(directToTarget, noseDirection);
			var angleTargetReflection = FindAngle(directToTarget, reflectionRocketSpeed);
			
			var (isNoseDown, isReflectionDown, isNoseRight) =
				FindNoseReflectionSpeedPosition(directToTarget, noseDirection, reflectionRocketSpeed);

			return SelectTurn(isNoseDown, isReflectionDown, isNoseRight, angleTargetNose, angleTargetReflection);
		}

		private static double ScalarMultiply(Vector v1, Vector v2)
			=> v1.X * v2.X + v1.Y * v2.Y;

		private static double FindAngle(Vector v1, Vector v2)
			=> Math.Acos(ScalarMultiply(v1, v2) / v1.Length / v2.Length);

		private static Vector FindSpeedReflection(Rocket rocket, Vector directTarget)
		{
			var projectionRocketSpeedOnDirectToTarget = ScalarMultiply( rocket.Velocity, directTarget)
				/ (directTarget.Length * directTarget.Length) * directTarget;
			var reflectionRocketSpeed = 2 * projectionRocketSpeedOnDirectToTarget -  rocket.Velocity;

			return ScalarMultiply(reflectionRocketSpeed, directTarget) < 0
				? -1 *  rocket.Velocity
				: reflectionRocketSpeed;
		}

		private static (bool, bool, bool) FindNoseReflectionSpeedPosition(Vector directToTarget, Vector noseDirection,
			Vector reflectionRocketSpeed)
		{
			var oXRight = directToTarget;
			var oXLeft = -1 * directToTarget;
			var oYUp = directToTarget.Rotate(-Math.PI / 2);
			var oYDown = directToTarget.Rotate(Math.PI / 2);
			
			var isNoseDown = FindAngle(noseDirection, oYUp) > FindAngle(noseDirection, oYDown);
			var isReflectionDown = FindAngle(reflectionRocketSpeed, oYUp)
			                       > FindAngle(reflectionRocketSpeed, oYDown);
			var isNoseRight = FindAngle(noseDirection, oXRight) < FindAngle(noseDirection, oXLeft);
			return (isNoseDown, isReflectionDown, isNoseRight);
		}

		private static Turn SelectTurn(bool isNoseDown, bool isReflectionDown, bool isNoseRight,
			double angleTargetNose, double angleTargetReflection)
		{
			if (isNoseRight)
			{
				if (isNoseDown)
				{
					if (isReflectionDown)
						return angleTargetNose < angleTargetReflection ? Turn.Right : Turn.Left;
					return Turn.Left;
				}
				if (isReflectionDown)
					return Turn.Right;
				return angleTargetNose < angleTargetReflection ? Turn.Left : Turn.Right;
			}
			if (isNoseDown)
			{
				if (isReflectionDown)
					return Turn.Left;
				return angleTargetReflection + angleTargetNose < Math.PI ? Turn.Left : Turn.Right;
			}
			if (isReflectionDown)
				return angleTargetReflection + angleTargetNose < Math.PI ? Turn.Right : Turn.Left;
			return Turn.Right;
		}
	}
}