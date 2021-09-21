using System.Collections.Generic;
using OpenCVInstruments.Containers;
using OpenCvSharp;

namespace OpenCVInstruments.Interfaces {
	public interface IInstruments {
		Mat ResizePhoto(Mat image, int width, int height);
		Mat GenKernel(double[,] array);

		List<Mat> GetImages(string searchPattern);

		public void ShowResults(List<ShowCont> showConts);

		public List<ShowCont> PackResult(params ShowCont[] conts);

		public List<ShowCont> PackResult();

		public void PrintResultStats(List<ShowCont> showConts);

		public (Scalar avrColorGrb, Mat searchedLocation) GetAvrColorInCenter(Mat img, int sideLenght);
	}
}