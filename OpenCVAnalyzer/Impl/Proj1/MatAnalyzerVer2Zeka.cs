using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace OpenCVAnalyzer.Impl.Proj1 {
	public class MatAnalyzerVer2Zeka : IMatAnalyzer {
		public Mat GetMask(Mat image) {
			Mat cvtColor = image.CvtColor(ColorConversionCodes.BGR2GRAY);
			IInstruments inst = new InstrumentDef();
			Mat resizePhoto = inst.ResizePhoto(cvtColor, cvtColor.Width / 2, cvtColor.Height / 2);
			Mat gaussianBlur = resizePhoto.GaussianBlur(new Size(31, 31), 0);

			var outputArray = new Mat(new int[] { resizePhoto.Height, resizePhoto.Width }, MatType.CV_32F);
			Cv2.Subtract(resizePhoto, gaussianBlur, outputArray);
			MatExpr matExpr = outputArray.Abs();


			Mat threshold = matExpr.ToMat().Threshold(80, 255, ThresholdTypes.Binary);

			Mat reversSize = inst.ResizePhoto(threshold, threshold.Width * 2, threshold.Height * 2);
			// inst.ShowResults(inst.PackResult(new ShowCont(
				// new ShowCont("org", gaussianBlur),
				// new ShowCont("image", threshold)
			// )));

			return reversSize;
		}
	}
}