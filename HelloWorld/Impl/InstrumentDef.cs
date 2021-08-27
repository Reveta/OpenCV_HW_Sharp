using System.Collections.Generic;
using HelloWorld.Interfaces;
using OpenCvSharp;


namespace HelloWorld.Impl {
	public class InstrumentDef : IInstruments {
		public Mat ResizePhoto(Mat image, int width, int height) {
			Mat resize = image.Resize(new Size(width, height), 0, 0, InterpolationFlags.Linear);
			return resize;
		}

		public Mat GenKernel(double[,] array, MatType matType) {
			// TODO maybe bugged   
			int row = array.GetLength(0);
			int col = array.GetLength(1);
			var kernel = new Mat(row, col, matType, array);
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