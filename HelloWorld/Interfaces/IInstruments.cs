using System.Collections.Generic;
using OpenCvSharp;

namespace HelloWorld.Interfaces {
	public interface IInstruments {
		Mat ResizePhoto(Mat image, int width, int height);
		Mat GenKernel(double[,] array, MatType matType);

		List<Mat> GetImages(string searchPattern);
	}
}