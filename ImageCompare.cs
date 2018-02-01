using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageCompareClass
{
	class ImageCompare
	{
		public static int SimilarityThreshhold { get; set; } = 0; // not yet checked against in code

		public List<ImageDataSingle> CompareBitmapsMultiple(string directory, Bitmap bitmapOrig, bool makeGrayScale = true)
		{
			if (directory == "" || bitmapOrig == null) return null;
			if (!Directory.Exists(directory)) return null;

			int fileCountInDirectory = Directory.GetFiles(directory, "*.jpg", SearchOption.TopDirectoryOnly).Length;
			if (fileCountInDirectory < 1) return null;
			string[] imagesInDirectory = Directory.GetFiles(directory, "*.jpg", SearchOption.TopDirectoryOnly);

			Bitmap tmpOriginalBitmap = bitmapOrig;

			Stopwatch timeTaken;
			List<ImageDataSingle> tmpList = new List<ImageDataSingle>();
			ImageDataSingle tmpObj;

			List<Color> colorArrayBitmapOrig = GetColorsFromBitmap(tmpOriginalBitmap);
			List<Color> colorArrayImageCompare;

			if (makeGrayScale) tmpOriginalBitmap = MakeGrayScale(tmpOriginalBitmap);

			foreach (string image in imagesInDirectory)
			{
				if (makeGrayScale)
				{
					using (Bitmap tmpCompareBitmap = MakeGrayScale((Bitmap) Image.FromFile(image)))
					{
						timeTaken = new Stopwatch();
						timeTaken.Start();
						tmpObj = new ImageDataSingle();
						colorArrayImageCompare = GetColorsFromBitmap(tmpCompareBitmap);


						tmpObj.CombinedTotalSize = (colorArrayBitmapOrig.Count + colorArrayImageCompare.Count);
						tmpObj.ArraySize1 = 0;
						tmpObj.ArraySize2 = 0;

						if (tmpCompareBitmap.Size == tmpOriginalBitmap.Size)
						{
							tmpObj.AreEqualInSize = true;
						}
						else
						{
							tmpObj.AreEqualInSize = false;
						}


						float diffCount = 0;

						for (int y = 0; y < tmpOriginalBitmap.Height; y++)
						{
							for (int x = 0; x < tmpOriginalBitmap.Width; x++)
							{
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).R - tmpCompareBitmap.GetPixel(x, y).R) / 255;
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).G - tmpCompareBitmap.GetPixel(x, y).G) / 255;
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).B - tmpCompareBitmap.GetPixel(x, y).B) / 255;
							}
						}

						if (diffCount == 0)
						{
							tmpObj.DifferenceCount = 0;
							tmpObj.DifferenceInPercentage = 0;
						}
						else
						{
							tmpObj.DifferenceCount = diffCount;
							tmpObj.DifferenceInPercentage = 100 * diffCount / (tmpOriginalBitmap.Width * tmpCompareBitmap.Height * 3);
						}


						timeTaken.Stop();
						tmpObj.TimeTakenMilliseconds = timeTaken.ElapsedMilliseconds;
						tmpObj.TestSuccessfull = true;

						tmpList.Add(tmpObj);

					}
				}
				else
				{
					using (Bitmap tmpCompareBitmap = (Bitmap)Image.FromFile(image))
					{
						timeTaken = new Stopwatch();
						timeTaken.Start();
						tmpObj = new ImageDataSingle();
						colorArrayImageCompare = GetColorsFromBitmap(tmpCompareBitmap);
			

						tmpObj.CombinedTotalSize = (colorArrayBitmapOrig.Count + colorArrayImageCompare.Count);
						tmpObj.ArraySize1 = 0;
						tmpObj.ArraySize2 = 0;

						if (tmpCompareBitmap.Size == tmpOriginalBitmap.Size)
						{
							tmpObj.AreEqualInSize = true;
						}
						else
						{
							tmpObj.AreEqualInSize = false;
						}


						float diffCount = 0;

						for (int y = 0; y < tmpOriginalBitmap.Height; y++)
						{
							for (int x = 0; x < tmpOriginalBitmap.Width; x++)
							{
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).R - tmpCompareBitmap.GetPixel(x, y).R) / 255;
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).G - tmpCompareBitmap.GetPixel(x, y).G) / 255;
								diffCount += (float)Math.Abs(tmpOriginalBitmap.GetPixel(x, y).B - tmpCompareBitmap.GetPixel(x, y).B) / 255;
							}
						}

						if (diffCount == 0)
						{
							tmpObj.DifferenceCount = 0;
							tmpObj.DifferenceInPercentage = 0;
						}
						else
						{
							tmpObj.DifferenceCount = diffCount;
							tmpObj.DifferenceInPercentage = 100 * diffCount / (tmpOriginalBitmap.Width * tmpCompareBitmap.Height * 3);
						}


						timeTaken.Stop();
						tmpObj.TimeTakenMilliseconds = timeTaken.ElapsedMilliseconds;
						tmpObj.TestSuccessfull = true;

						tmpList.Add(tmpObj);
					}
				}
				
			}

			return tmpList;
		}
		public ImageDataSingle CompareBitmapsSingle(Bitmap bmp1, Bitmap bmp2)
		{
			ImageConverter imgConverter = new ImageConverter();
			ImageDataSingle toReturn = new ImageDataSingle();
			Stopwatch tmpTimer = new Stopwatch();
			tmpTimer.Start();


			List<Color> bmpByteArr1, bmpByteArr2;

			bmpByteArr1 = GetColorsFromBitmap(bmp1);
			bmpByteArr2 = GetColorsFromBitmap(bmp2);


			toReturn.CombinedTotalSize = bmpByteArr1.Count + bmpByteArr2.Count;
			toReturn.ArraySize1 = bmpByteArr1.Count;
			toReturn.ArraySize2 = bmpByteArr2.Count;


			if (bmpByteArr1.Count == bmpByteArr2.Count)
			{
				toReturn.AreEqualInSize = true;
			}
			else
			{
				tmpTimer.Stop();
				toReturn.AreEqualInSize = false;
				toReturn.DifferenceCount = -1;
				toReturn.DifferenceInPercentage = -1;
				toReturn.TestSuccessfull = false;
				toReturn.TimeTakenMilliseconds = tmpTimer.ElapsedMilliseconds;

				tmpTimer = null;
				imgConverter = null;
				// Byte arrays were of different size
				return toReturn;
			}

			float diffCount = 0;

			for (int y = 0; y < bmp1.Height; y++)
			{
				for (int x = 0; x < bmp1.Width; x++)
				{
					diffCount += (float)Math.Abs(bmp1.GetPixel(x, y).R - bmp2.GetPixel(x, y).R) / 255;
					diffCount += (float)Math.Abs(bmp1.GetPixel(x, y).G - bmp2.GetPixel(x, y).G) / 255;
					diffCount += (float)Math.Abs(bmp1.GetPixel(x, y).B - bmp2.GetPixel(x, y).B) / 255;
				}
			}


			if (diffCount > 0)
			{
				toReturn.DifferenceCount = diffCount;
				float tmpDifferenceInPercentage = 100 * diffCount / (bmp1.Width * bmp1.Height * 3);
				tmpTimer.Stop();
				toReturn.DifferenceInPercentage = tmpDifferenceInPercentage;
				toReturn.TestSuccessfull = true;
				toReturn.TimeTakenMilliseconds = tmpTimer.ElapsedMilliseconds;
			}
			else
			{
				tmpTimer.Stop();
				toReturn.DifferenceCount = 0;
				toReturn.DifferenceInPercentage = 0;
				toReturn.TestSuccessfull = true;
				toReturn.TimeTakenMilliseconds = tmpTimer.ElapsedMilliseconds;
			}

			tmpTimer = null;
			imgConverter = null;
			return toReturn;
		}

		#region Support functions
		public void SetSimilarityThreshhold(int value)
		{
			SimilarityThreshhold = value;
		}
		public List<ImageDataSingle> SortListByLowestDifference(List<ImageDataSingle> tmpList)
		{
			if (tmpList.Count > 0)
			{
				List<ImageDataSingle> sortedList = tmpList.OrderBy(o => o.DifferenceInPercentage).ToList();
				return sortedList;
			}
			return null;
		}
		public List<Color> GetColorsFromBitmap(Bitmap bitmapSource)
		{
			if (bitmapSource == null) return null;

			List<Color> toReturn = new List<Color>();

			Bitmap tmp = bitmapSource;
			for (int i = 0; i < tmp.Height * tmp.Width; i++)
			{
				int row = i / tmp.Width;
				int col = i % tmp.Width;
				var pixel = tmp.GetPixel(col, row);
				toReturn.Add(pixel);
			}

			return toReturn;
		}
		public Bitmap ResizeImage(Bitmap bitmapSource,int newWidth, int newHeight)
		{
			if (bitmapSource == null) return null;
			Bitmap resizedBmp = new Bitmap(bitmapSource, new Size(newWidth, newHeight));
			return resizedBmp;
		}
		public Bitmap MakeGrayScale(Bitmap bitmapSource)
		{
			if (bitmapSource == null) return null;
			//create a blank bitmap the same size as original
			Bitmap newBitmap = new Bitmap(bitmapSource.Width, bitmapSource.Height);

			//get a graphics object from the new image
			Graphics g = Graphics.FromImage(newBitmap);

			//create the grayscale ColorMatrix
			ColorMatrix colorMatrix = new ColorMatrix(
				new float[][]
				{
					new float[] {.3f, .3f, .3f, 0, 0},
					new float[] {.59f, .59f, .59f, 0, 0},
					new float[] {.11f, .11f, .11f, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0, 0, 0, 0, 1}
				});

			//create some image attributes
			ImageAttributes attributes = new ImageAttributes();

			//set the color matrix attribute
			attributes.SetColorMatrix(colorMatrix);

			//draw the original image on the new image
			//using the grayscale color matrix
			g.DrawImage(bitmapSource, new Rectangle(0, 0, bitmapSource.Width, bitmapSource.Height),
				0, 0, bitmapSource.Width, bitmapSource.Height, GraphicsUnit.Pixel, attributes);

			//dispose the Graphics object
			g.Dispose();
			return newBitmap;
		}
		#endregion
		#region Misc Stuff
		public class ImageDataSingle
		{
			public bool AreEqualInSize;
			public int ArraySize1;
			public int ArraySize2;
			public int CombinedTotalSize;
			public float DifferenceCount;
			public float DifferenceInPercentage;
			public bool TestSuccessfull;
			public long TimeTakenMilliseconds;
		}
		#endregion
	}
}
