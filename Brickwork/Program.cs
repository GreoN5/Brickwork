using System;

namespace Brickwork
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Brickwork brickwork = new Brickwork(2, 4, "1122 3344");

			brickwork.MakeSecondLayerOfBrickworkAndPrint();
		}
	}
}
