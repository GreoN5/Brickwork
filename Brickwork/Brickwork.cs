﻿using System;
using System.Text.RegularExpressions;

namespace Brickwork
{
	/// <summary>
	/// Make a block of bricks
	/// </summary>
	public class Brickwork
	{
		private string _firstLayerInput; // the input layer
		private int[,] _secondLayerOutput; // the output layer
		private int[,] _firstLayer; // the input layer but has int values
		private int _n; // the amount of lines that the second layer will receive
		private int _m; // the amount of bricks/2 (1 brick consists of 2 numbers)

		/// <summary>
		/// Make the first layer of the block
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

		/// <summary>
		/// Prints the second layer
		/// </summary>
		public void MakeSecondLayer()
		{
			Console.WriteLine("First layer:");
			Console.WriteLine();
			PrintFirstLayerIfThereIsOne();

			Console.WriteLine();

			var firstLayer = GetFirstLayer();
			var lastColumnBricks = GetLastColumnOfBricks(firstLayer);
			var withoutLastColumns = GetWithoutLastTwoColumnsOfBricks(firstLayer);
			var separatedColumn = SeparateLastColumnOfBricksAndGetBricks(lastColumnBricks);
			this._secondLayerOutput = MakeSecondLayer(withoutLastColumns, separatedColumn);

			Console.WriteLine();

			Console.WriteLine("Second layer:");
			Console.WriteLine();

			int result = PrintSecondLayerIfThereIsOne();

			if (result == -1)
			{
				Console.WriteLine(result);
			}

			Console.WriteLine();
		}

		/// <summary>
		/// Prints first layer if it exists
		/// </summary>
		private void PrintFirstLayerIfThereIsOne()
		{
			var firstLayer = GetFirstLayer(); // get the first input layer

			if (firstLayer == null)
			{
				Console.WriteLine("Invalid layer!");
				return;
			}

			string asteriks = GetAsteriksAroundBricks(firstLayer); // get the number of asteriks needed for the grid

			for (int i = 0; i < firstLayer.GetLength(0); i++)
			{
				for (int j = 0; j < firstLayer.GetLength(1); j++)
				{
					if (j % 2 == 0)
					{
						Console.Write("*");
					}

					Console.Write(firstLayer[i, j]);
				}

				Console.Write("*");
				Console.WriteLine();
				Console.WriteLine(asteriks);
			}
		}

		/// <summary>
		/// Prints the second layer if there is one
		/// </summary>
		/// <returns>Returs -1 if the layer is invalid, 0 if the layer can be made</returns>
		private int PrintSecondLayerIfThereIsOne()
		{
			if (this._secondLayerOutput == null)
			{
				return -1;
			}
			else
			{
				string asteriks = GetAsteriksAroundBricks(this._secondLayerOutput); // get the needed asteriks for the second layer

				for (int i = 0; i < this._secondLayerOutput.GetLength(0); i++)
				{
					Console.Write("*");

					for (int j = 0; j < this._secondLayerOutput.GetLength(1); j++)
					{
						if (j == 1)
						{
							Console.Write("*");
						}

						Console.Write(this._secondLayerOutput[i, j]);

						if (j > 1 && j % 2 == 0)
						{
							Console.Write("*");
						}
					}

					Console.Write("*");
					Console.WriteLine();

					if ((i + 1) % 2 == 0)
					{
						asteriks = asteriks.Remove(1, 1).Insert(i, "*");
						asteriks = asteriks.Remove(asteriks.Length - 1, 1).Insert(asteriks.Length - 1, "*");
						Console.WriteLine(asteriks);
					}
					else if (i == 0)
					{
						asteriks = asteriks.Remove(i + 1, 1).Insert(i + 1, " ");
						asteriks = asteriks.Remove(asteriks.Length - 1, 1).Insert(asteriks.Length - 1, " ");
						Console.WriteLine(asteriks);
					}
					else if (i == 1)
					{
						asteriks = asteriks.Replace(" ", "*");
						Console.WriteLine(asteriks);
					}
					else
					{
						asteriks = asteriks.Remove(1, 1).Insert(1, " ");
						asteriks = asteriks.Remove(asteriks.Length - 1, 1).Insert(asteriks.Length - 1, " ");
						Console.WriteLine(asteriks);
					}
				}

				return 0;
			}
		}

		/// <summary>
		/// Get the number of asteriks need to surround the layer and the bricks
		/// </summary>
		/// <param name="layer">The layer that will be surrounded</param>
		/// <returns>Returns the number of asterisk for the particular layer</returns>
		private string GetAsteriksAroundBricks(int[,] layer)
		{
			string asteriks = "";

			for (int i = 0; i < layer.GetLength(1) + (layer.GetLength(1) / 2) + 1; i++)
			{
				asteriks += "*";
			}

			return asteriks;
		}

		/// <summary>
		/// Get the first input layer
		/// </summary>
		/// <returns>Returns the input layer or null if the layer is invalid</returns>
		private int[,] GetFirstLayer()
		{
			string[] lines = this._firstLayerInput.Split(" "); // get the lines into an array in order to loop through them and assign them to the first layer

			if (!CheckIfFirstLayerLinesValid(lines) || !CheckIfValidBricksForFirstLayer(lines))
			{
				return null;
			}

			for (int i = 0; i < lines.Length; i++)
			{
				char[] numberBricks = lines[i].ToCharArray();

				if (!CheckIfValidLinesAndBrickSize())
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

		/// <summary>
		/// Make the second layer of the first one and the last column of bricks of the first layer
		/// </summary>
		/// <param name="firstLayer">The input layer</param>
		/// <param name="separatedBricks">The last column of bricks from the first layer</param>
		/// <returns>Returns the second layer if the first one is valid, otherwise null</returns>
		private int[,] MakeSecondLayer(int[,] firstLayer, int[,] separatedBricks)
		{
			if (firstLayer == null || separatedBricks == null)
			{
				return null;
			}

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

		/// <summary>
		/// Get the last column of bricks from the input layer if valid
		/// </summary>
		/// <param name="firstLayer">The input layer</param>
		/// <returns>Returns the last column of bricks from the input layer if valid, otherwise null</returns>
		private int[,] GetLastColumnOfBricks(int[,] firstLayer)
		{
			if (firstLayer == null)
			{
				return null;
			}

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

		/// <summary>
		/// Get the first layer without the last column of bricks
		/// </summary>
		/// <param name="firstLayer">The input first layer</param>
		/// <returns>Returns the input layer without the last column of bricks if valid, otherwise null</returns>
		private int[,] GetWithoutLastTwoColumnsOfBricks(int[,] firstLayer)
		{
			if (firstLayer == null)
			{
				return null;
			}

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

		/// <summary>
		/// Separates the last column of bricks
		/// </summary>
		/// <param name="lastColumn">The last column of bricks</param>
		/// <returns>Returns separated last column if valid, otherwise null</returns>
		private int[,] SeparateLastColumnOfBricksAndGetBricks(int[,] lastColumn)
		{
			if (lastColumn == null)
			{
				return null;
			}

			int[,] separatedColumn = new int[lastColumn.GetLength(0), 1];

			for (int i = 0; i < lastColumn.GetLength(0); i++)
			{
				for (int j = 0; j < separatedColumn.GetLength(1); j++)
				{
					separatedColumn[i, j] = lastColumn[i, j];
				}
			}

			return separatedColumn;
		}

		/// <summary>
		/// Checks for the length of the input layer
		/// </summary>
		/// <param name="lines"></param>
		/// <returns>Returns true if the lines are valid, otherwise false</returns>
		private bool CheckIfFirstLayerLinesValid(string[] lines)
		{
			if (lines.Length != this._n || this._n % 3 == 0 || this._n < 2) // if the lines are not even numbers or there are less than 2 lines
			{
				return false; // returns false for invalid layer
			}

			return true;
		}

		/// <summary>
		/// Checks if the bricks are valid
		/// </summary>
		/// <param name="bricks"></param>
		/// <returns>Returns true if the bricks are valid, otherwise false</returns>
		private bool CheckIfValidBricksForFirstLayer(string[] bricks)
		{
			for (int i = 0; i < bricks.Length; i++)
			{
				char[] validBricks = bricks[i].ToCharArray();

				for (int j = 0; j < validBricks.Length; j += 2)
				{
					if (Regex.IsMatch(validBricks[i].ToString(), @"^\d+$"))
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
			}

			return true;
		}

		/// <summary>
		/// Checks if the size of the bricks and lines are valid
		/// </summary>
		/// <returns>Returns true if are valid, otherwise false</returns>
		private bool CheckIfValidLinesAndBrickSize()
		{
			if (this._n < 100 && this._m < 100 && this._m / 2 == this._n && this._m % 2 == 0)
			{
				if (CheckBrickSize() && CheckLinesSize())
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the size of the bricks matches the size of the input
		/// </summary>
		/// <returns>Returns true if the size matches, otherwise false</returns>
		private bool CheckBrickSize()
		{
			if (this._firstLayer.GetLength(1) == this._m)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if the size of the lines matches the size of the input
		/// </summary>
		/// <returns>Returns true if the size matches, otherwise false</returns>
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
