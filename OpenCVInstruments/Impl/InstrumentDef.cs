using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace OpenCVInstruments.Impl {
	public class InstrumentDef : IInstruments {
		public Mat ResizePhoto(Mat image, int width, int height) {
			Mat resize = image.Resize(new Size(width, height), 0, 0, InterpolationFlags.Linear);
			return resize;
		}

		public Mat GenKernel(double[,] array) {
			int row = array.GetLength(0);
			int col = array.GetLength(1);

			var kernel = new Mat(row, col, MatType.CV_8U, data: array);
			return kernel;
		}

		/**
		 searchPatter - pattern example https://learnopencv.com/reading-and-writing-videos-using-opencv/
		 */
		public List<Mat> GetImages(string searchPattern) {
			using var capture = new VideoCapture(searchPattern);
			var images = new List<Mat>();

			while (capture.IsOpened()) {
				var image = new Mat();
				capture.Read(image);

				if (image.Empty()) {
					break;
				}

				images.Add(image);
			}

			return images;
		}

		public List<ShowCont> PackResult(params ShowCont[] conts) { return conts.ToList(); }

		public List<ShowCont> PackResult() { return new List<ShowCont>(); }

		public void PrintResultStats(List<ShowCont> showConts) {
			if (showConts.Count > 0) {
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"Got {showConts.Count} packs to show ");
				Console.ResetColor();
			} else {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine($"Got result has nothing to show ");
				Console.ResetColor();
			}
		}

		/** * Carefully! Import .Net Framework 4.8 (!!!) */
		public void ShowResults(List<ShowCont> showConts) {
			showConts.ForEach(cont => {
				if (cont.IsMulti) {
					// Imported function
					int displayWidth = Screen.PrimaryScreen.WorkingArea.Width - 20 * cont.Count;
					// ReSharper disable once PossibleLossOfFraction
					double winWidth = displayWidth / cont.Count;

					var count = 0;
					cont.Conts.ForEach(showCont => {
						if (showCont.IsMulti) {
							Console.ForegroundColor = ConsoleColor.DarkRed;
							Console.WriteLine("Found third or higher level in show container! " +
							                  "It`s not supported! " +
							                  "Result can be incorrect!"
							);
							Console.ResetColor();
						} else {
							ShowOneResult((int)winWidth, showCont, count);
							count++;
						}
					});

					Cv2.WaitKey();
				} else {
					ShowOneResult((int)(Screen.PrimaryScreen.WorkingArea.Width * 0.7), cont, 0);
					Cv2.WaitKey();
				}

				Cv2.DestroyAllWindows();
			});
		}

		private void ShowOneResult(int winWidth, ShowCont showCont, int count) {
			// ReSharper disable once PossibleLossOfFraction
			double coof = (double)winWidth / (double)showCont.Image.Width;
			Mat showContImage = ResizePhoto(showCont.Image,
				winWidth,
				(int)(showCont.Image.Height * coof)
			);
			var window = new Window(showCont.Name, showContImage);

			window.Move(((int)winWidth + 20) * count, 0);
		}
		
		public (Scalar avrColorGrb, Mat searchedLocation) GetAvrColorInCenter(Mat img, int sideLenght) {
			int halfSide = sideLenght / 2;
			int widthStart = img.Width / 2 - halfSide;
			int widthEnd = img.Width / 2 + halfSide;
			int heightStart = img.Height / 2 - halfSide;
			int heightEnd = img.Height / 2 + halfSide;
			Mat searchedLocation = img[widthStart, widthEnd, heightStart, heightEnd];

			Scalar scalar = Cv2.Mean(searchedLocation);
			return (scalar, searchedLocation);
		}
	}
}