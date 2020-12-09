using System;

namespace Brickwork
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Brickwork firstBrickwork = new Brickwork(2, 4, "1122 3344");
			Brickwork secondBrickwork = new Brickwork(4, 8, "11223344 55667788 99112233 44556677");

			firstBrickwork.MakeSecondLayer();
			secondBrickwork.MakeSecondLayer();
		}
	}
}
