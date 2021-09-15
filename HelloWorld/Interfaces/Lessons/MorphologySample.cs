using System.Collections.Generic;
using HelloWorld.Impl;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using OpenCvSharp.ML;

namespace HelloWorld.Interfaces.Lessons {
	/// <summary>
	/// 
	/// </summary>
	class MorphologySample : ICodeTester {
		public IInstruments _inst = new InstrumentDef();

		public List<ShowCont> Run() {
			var pathImg = @"media/lenna.png";

			var gray = new Mat(pathImg, ImreadModes.Grayscale);
			var binary = new Mat();
			var dilate1 = new Mat();
			var dilate2 = new Mat();
			byte[] kernelValues = { 
				0, 1, 0,
				1, 1, 1,
				0, 1, 0
			}; // cross (+)
			
			var kernel = new Mat(3, 3, MatType.CV_8UC1, kernelValues);

			// Binarize
			Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Otsu);

			// empty kernel
			Cv2.Dilate(binary, dilate1, null);
			// + kernel
			Cv2.Dilate(binary, dilate2, kernel);


			return _inst.PackResult(
				new ShowCont(
					new ShowCont("binary", binary),
					new ShowCont("dilate (kernel = null)", dilate1)
				),
				new ShowCont(
					new ShowCont("binary", binary),
					new ShowCont("dilate (kernel = +)", dilate2))
			);
		}
	}
}