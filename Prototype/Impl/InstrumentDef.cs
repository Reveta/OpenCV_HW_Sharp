using System.Collections.Generic;
using OpenCvSharp;
using Prototype.Interfaces;

namespace Prototype.Impl {
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
	}
}