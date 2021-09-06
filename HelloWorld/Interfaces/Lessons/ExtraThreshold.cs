using System.Collections.Generic;
using HelloWorld.Impl;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using OpenCvSharp.XImgProc;

namespace HelloWorld.Interfaces.Lessons {
	public class ExtraThreshold : ICodeTester {
		private IInstruments _instruments = new InstrumentDef();

		public List<ShowCont> Run() {
			Mat img = Cv2.ImRead(@"media/edgeDetection/input_image-1.jpg", ImreadModes.Grayscale);
			Mat imgProj = Cv2.ImRead(@"media/project/proj1/Chip_008.tif", ImreadModes.Grayscale);
			Mat imgProj2 = Cv2.ImRead(@"media/project/proj2/Chip_002.jpg", ImreadModes.Grayscale);

			var imgBlur = imgProj.Clone();
			int loops = 17;
			// for (int i = 0; i < loops; i++) {
				// Cv2.GaussianBlur(imgBlur,imgBlur, new Size(3,3), 0);
			// }
			Cv2.GaussianBlur(imgBlur,imgBlur, new Size(13,13), 0);

			
			var thresh = new Mat();
			CvXImgProc.NiblackThreshold(imgBlur, thresh, 200, ThresholdTypes.Binary, 5, -0.14,
				LocalBinarizationMethods.Nick);

			var canny = new Mat();
			Mat inputArray = imgBlur.Clone();
			Cv2.GaussianBlur(inputArray,inputArray, new Size(11,11), 0);
			Cv2.Canny(inputArray, canny, 100, 200);
			return this._instruments.PackResult(
				new ShowCont(
					new ShowCont("org", imgProj),
					new ShowCont("NiBlack", thresh)
				), new ShowCont(
					new ShowCont("org", imgProj),
					new ShowCont("Cann", canny)
				)
			);
		}
	}
}