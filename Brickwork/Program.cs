using System;

namespace Brickwork
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Brickwork brickwork = new Brickwork(8, 16, "1122334455667755 1122334455667799 1122334455667744 1122334455667722 1122334455667788 1122334455667799 1122334455667788 1122334455667799");
			Brickwork brickwork1 = new Brickwork(4, 8, "11223344 55667711 11223322 55667788");

			brickwork.MakeSecondLayerOfBrickworkAndPrint();
			//brickwork1.MakeSecondLayerOfBrickworkAndPrint();
		}
	}
}
