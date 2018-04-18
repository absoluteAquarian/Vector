using System;

namespace Vectors{
	public class Vector2D{
		//True components of the vector
		public double X;
		public double Y;

		//Endpoints of the vector
		public double X1;
		public double Y1;
		public double X2;
		public double Y2;

		//Other information
		public double Magnitude;
		public double StandardAngle;
		public double QuadrantAngle;
		public Vector2D Hat;

		//Unit vectors
		public static const Vector3D I = new Vector3D(1, 0);
		public static const Vector3D J = new Vector3D(0, 1);

		//QBI[1] is N/S, QBI[2] is E/W
		public string[] QBIndicators = new string[2];
	
		#region Constructors
		public Vector2D(){
			X = 0d;
			Y = 0d;
			X1 = 0d;	Y1 = 0d;
			X2 = 0d;	Y2 = 0d;
			Magnitude = 0d;
			StandardAngle = 0d;
			QuadrantAngle = 90d;
			Hat = new Vector(0, 0);
		}
		public Vector2D(double x, double y) : this(){
			X2 = X = x;
			Y2 = Y = y;
			Magnitude = Math.Sqrt(X * X + Y * Y);
			//Math.Atan() returns a radian measure
			if(X != 0)
				StandardAngle = Math.Atan(Y / X) * 180d / Math.PI;
			else
				StandardAngle = 0d;
			QuadrantAngle = GetQAngle();
			Hat = new Vector(X / Magnitude, Y / Magnitude);
		}
		public Vector2D(double x1, double y1, double x2, double y2) : this(){
			X1 = x1;	Y1 = y1;
			X2 = x2;	Y2 = y2;
			X = X2 - X1;
			Y = Y2 - Y1;
			Magnitude = Math.Sqrt(X * X + Y * Y);
			if(X != 0)
				StandardAngle = Math.Atan(Y / X) * 180d / Math.PI;
			else if(Y > 0)
				StandardAngle = 90d;
			else if(Y < 0)
				StandardAngle = 270d;
			QuadrantAngle = GetQAngle();
			Hat = new Vector(X / Magnitude, Y / Magnitude);
		}
		public Vector2D(Vector2D v) : this(v.X, v.Y){
			X = v.X;	Y = v.Y;
			X1 = v.X1;	Y1 = v.Y1;
			X2 = v.X2;	Y2 = v.Y2;
			Magnitude = v.Magnitude;
			StandardAngle = v.StandardAngle;
			QuadrantAngle = v.QuadrantAngle;
			Hat = v.Hat;
		}
		public Vector2D(Vector2D[] vectorList) : this(){
			Vector2D temp = new Vector2D();
			for(int i = 0; i < vectorList.Length; i++)
				temp += vectorList[i];
			new Vector2D(temp);
		}
		#endregion

		public double GetQAngle(){
			/*
				Finds the quadrant-bearing angle
				for the standard angle variable.
				Also places the direction in
				the Quadrant-Bearing Indicators
				string array.
			*/
			double angle = 0;
			//Get a coterminal angle in the [0, 360) degrees range
			if(StandardAngle >= 360d)
				for( ; StandardAngle >= 360d ; )
					StandardAngle -= 360d;
			else
				for( ; StandardAngle < 0d ; )
					StandardAngle += 360d;

			//Find which quadrant it is in
			//Skip (0, 90) degrees; we're finding the reference angle
			if(StandardAngle > 0d && StandardAngle < 90d){
				angle = 90d - StandardAngle;
				QBIndicators[0] = "N";
				QBIndicators[1] = "E";
			}else if(StandardAngle < 180d && StandardAngle > 90d){
				angle = 90d - (180d - StandardAngle);
				QBIndicators[0] = "N";
				QBIndicators[1] = "W";
			}else if(StandardAngle < 270d && StandardAngle > 180d){
				angle = 270d - StandardAngle;
				QBIndicators[0] = "S";
				QBIndicators[1] = "W";
			}else if(StandardAngle < 360d && StandardAngle > 270d){
				angle = 90d - (360d - StandardAngle);
				QBIndicators[0] = "S";
				QBIndicators[1] = "E";
			}

			//If the angle is on one of the axes, find which one it's on
			if(angle == 0d){
				if(StandardAngle == 0d){
					angle = 90d;
					QBIndicators[0] = "";
					QBIndicators[1] = "E";
				}else if(StandardAngle == 90d){
					angle = 0d;
					QBIndicators[0] = "N";
					QBIndicators[1] = "";
				}else if(StandardAngle == 180d){
					angle = 90d;
					QBIndicators[0] = "";
					QBIndicators[1] = "W";
				}else if(StandardAngle == 270d){
					angle = 0;
					QBIndicators[0] = "S";
					QBIndicators[1] = "";
				}
			}
			return angle;
		}
		public double AngleBetween(Vector2D v){
			return Math.Acos((this * v) / (this.Magnitude * v.Magnitude)) * 180d / Math.PI;
		}
	
		#region Operators
		public static Vector2D operator -(Vector2D v){
			return -1 * v;
		}
		public static Vector2D operator +(Vector2D v1, Vector2D v2){
			return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
		}
		public static Vector2D operator -(Vector2D v1, Vector2D v2){
			return v1 + -v2;
		}
		public static double operator *(Vector2D v1, Vector2D v2){
			return v1.X * v2.X + v1.Y * v2.Y;
		}
		public static Vector2D operator *(int scalar, Vector2D v){
			return (double)scalar * v;
		}
		public static Vector2D operator *(double scalar, Vector2D v){
			return new Vector2D(v.X * scalar, v.Y * scalar);
		}
		public static Vector2D operator *(Vector2D v, int scalar){
			return scalar * v;
		}
		public static Vector2D operator *(Vector2D v, double scalar){
			return scalar * v;
		}
		#endregion

		public override string ToString(){
			string output = "";
			output += "Vector Data\n";
			output += string.Format("v = <{0}, {1}>\nv = ({2}, {3}) to ({4}, {5})\n||v|| = {6:N3}\n",
				X, Y, X1, Y1, X2, Y2, Magnitude);
			if(X == 0 && Y == 0)
				output += "Standard Direction:\n\tN/A\n";
			else
				output += string.Format("Standard Direction:\n\t{0:N3}°\n", StandardAngle);
			output += "Quadrant-Bearing Direction:\n\t";
			if(X == 0 && Y == 0)
				output += "N/A\n";
			else if(QuadrantAngle != 0d && QuadrantAngle != 90d)
				output += string.Format("{0:N3}[{1}{2:N3}°{3}]\n",
					Magnitude, QBIndicators[0], QuadrantAngle, QBIndicators[1]);
			else if(QuadrantAngle == 0d || QuadrantAngle == 90d)
				output += string.Format("{0:N3}[{1}]\n", Magnitude, QBIndicators[0]);
			return output;
		}
	}
}