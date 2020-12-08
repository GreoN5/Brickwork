using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brickwork
{
	public class Brickwork
	{
		private string _firstLayerInput; // the input layer
		private int[,] _secondLayerOutput; // the output layer
		private int[,] _firstLayer; // the input layer but has int values
		private int _n; // the amount of lines that the second layer will receive
		private int _m; // the amount of bricks/2 (1 brick consists of 2 numbers)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n">The amount of lines that the layers will receive(must be equal to the lines of the firstLayer)</param>
		/// <param name="m">The amount of bricks(each brick has 2 equal numbers in it)</param>
		/// <param name="firstLayerLinesBricks">Add lines of bricks with space in between each line</param>
		public Brickwork(int n, int m, string firstLayerInput)
		{
			this._n = n;
			this._m = m;
			this._firstLayerInput = firstLayerInput;
			this._firstLayer = new int[n, m];
			this._secondLayerOutput = new int[n, m];
		}

		public void MakeSecondLayerOfBrickworkAndPrint()
		{
			PrintFirstLayer();

			Console.WriteLine();

			var firstLayer = GetFirstLayer();
			var lastColumnBricks = GetLastColumnOfBricks(firstLayer);
			var withoutLastColumns = GetWithoutLastTwoColumnsOfBricks(firstLayer);
			var separatedColumn = SeparateLastColumnOfBricksAndGetBricks(lastColumnBricks);
			this._secondLayerOutput = MakeSecondLayer(withoutLastColumns, separatedColumn);

			Console.WriteLine();
		}

		private void PrintFirstLayer()
		{
			if (CheckIfValidLinesAndBrickSize())
			{
				var firstLayer = GetFirstLayer();

				if (firstLayer == null)
				{
					Console.WriteLine(-1);
					return;
				}

				for (int i = 0; i < firstLayer.GetLength(0); i++)
				{
					for (int j = 0; j < firstLayer.GetLength(1); j++)
					{
						Console.Write(firstLayer[i, j]);
					}

					Console.WriteLine();
				}

				return;
			}

			Console.WriteLine(-1);
		}

		private int[,] GetFirstLayer()
		{
			string[] lines = this._firstLayerInput.Split(" ");

			if (!CheckIfFirstLayerLinesValid(lines) || !CheckIfValidBricksForFirstLayer(lines))
			{
				return null;
			}

			for (int i = 0; i < lines.Length; i++)
			{
				char[] numberBricks = lines[i].ToCharArray();

				if (numberBricks.Length != this._m)
				{
					return null;
				}

				for (int j = 0; j < numberBricks.Length; j++)
				{
					this._firstLayer[i, j] = int.Parse(numberBricks[j].ToString());
				}
			}

			return this._firstLayer;
		}

		private int[,] MakeSecondLayer(int[,] firstLayer, int[,] separatedBricks)
		{
			for (int i = 0; i < this._secondLayerOutput.GetLength(0); i++)
			{
				for (int j = 0; j < this._secondLayerOutput.GetLength(1); j++)
				{
					if (j == 0 && this._secondLayerOutput[i, j] == 0)
					{
						this._secondLayerOutput[i, j] = separatedBricks[i, j];
						this._secondLayerOutput[i + 1, j] = separatedBricks[i, j];
					}

					if (j == this._secondLayerOutput.GetLength(1) - 1)
					{
						if (this._secondLayerOutput[i, 0] == separatedBricks[i, 0])
						{
							this._secondLayerOutput[i, j] = separatedBricks[i + 1, 0];
							this._secondLayerOutput[i + 1, j] = _secondLayerOutput[i, j];
						}
					}

					if (j != 0 && j != this._secondLayerOutput.GetLength(1) - 1)
					{
						this._secondLayerOutput[i, j] = firstLayer[i, j - 1];
					}
				}
			}

			return this._secondLayerOutput;
		}

		private int[,] GetLastColumnOfBricks(int[,] firstLayer)
		{
			int[,] lastColumnOfBricks = new int[firstLayer.GetLength(0), 2];

			for (int i = 0; i < firstLayer.GetLength(0); i++)
			{
				for (int j = firstLayer.GetLength(1); j > 1; j--)
				{
					if (j == firstLayer.GetLength(1) - 2)
					{
						break;
					}

					lastColumnOfBricks[i, j - (firstLayer.GetLength(1) - 1)] = firstLayer[i, j - 1];
				}
			}

			return lastColumnOfBricks;
		}

		private int[,] GetWithoutLastTwoColumnsOfBricks(int[,] firstLayer)
		{
			int[,] withoutLastTwoColumnsOfBricks = new int[firstLayer.GetLength(0), firstLayer.GetLength(1) - 2];

			for (int i = 0; i < firstLayer.GetLength(0); i++)
			{
				for (int j = 0; j < firstLayer.GetLength(1) - 2; j++)
				{
					withoutLastTwoColumnsOfBricks[i, j] = firstLayer[i, j];
				}
			}

			return withoutLastTwoColumnsOfBricks;
		}

		private int[,] SeparateLastColumnOfBricksAndGetBricks(int[,] lastTwoColumns)
		{
			int[,] separatedColumn = new int[lastTwoColumns.GetLength(0), 1];

			for (int i = 0; i < lastTwoColumns.GetLength(0); i++)
			{
				for (int j = 0; j < separatedColumn.GetLength(1); j++)
				{
					separatedColumn[i, j] = lastTwoColumns[i, j];
				}
			}

			return separatedColumn;
		}

		private bool CheckIfFirstLayerLinesValid(string[] lines)
		{
			if (lines.Length != this._n && this._n % 3 == 0 || this._n < 2) // if the lines are not even numbers or there are less than 2 lines
			{ 
				return false; // returns false for invalid layer
			}

			return true;
		}

		private bool CheckIfValidBricksForFirstLayer(string[] bricks)
		{
			for (int i = 0; i < bricks.Length; i++)
			{
				char[] validBricks = bricks[i].ToCharArray();

				for (int j = 0; j < validBricks.Length; j += 2)
				{
					if (j == validBricks.Length - 1)
					{
						return true;
					}

					if (validBricks[j] != validBricks[j + 1])
					{
						return false;
					}

					if (j > 0 && validBricks[j] == validBricks[j + 1] && validBricks[j] == validBricks[j - 1])
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool CheckIfValidLinesAndBrickSize()
		{
			if (this._n < 100 && this._m < 100 && this._m / 2 == this._n)
			{
				if (CheckBrickSize() && CheckLinesSize())
				{
					return true;
				}
			}

			return false;
		}

		private bool CheckBrickSize()
		{
			if (this._firstLayer.GetLength(1) == this._m)
			{
				return true;
			}

			return false;
		}

		private bool CheckLinesSize()
		{
			if (this._firstLayer.GetLength(0) == this._n)
			{
				return true;
			}

			return false;
		}
	}
}
