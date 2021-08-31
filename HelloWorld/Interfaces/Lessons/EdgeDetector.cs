using System.Collections.Generic;
using HelloWorld.Impl;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces.Lessons {
	public class EdgeDetector : ICodeTester {

		private IInstruments _instruments = new InstrumentDef();
		public List<ShowCont> Run() {
			// Read the original image
			Mat img = Cv2.ImRead(@"media/edgeDetection/input_image-1.jpg");
			Mat imgProt = Cv2.ImRead(@"media/edgeDetection/Chip_002.jpg", ImreadModes.Grayscale);

			// Convert to graycsale
			var imageGray = new Mat();
			Cv2.CvtColor(img, imageGray, ColorConversionCodes.BGR2GRAY);
			// Blur the image for better edge detection
			var imageGrayBlur = new Mat();
			Cv2.GaussianBlur(imageGray, imageGrayBlur, new Size(9, 9), 0);
			Cv2.GaussianBlur(imgProt,  imgProt, new Size(9, 9), 0);

			// Sobel Edge Detection
			var sobelx = new Mat();
			Cv2.Sobel(imageGrayBlur, sobelx, MatType.CV_64F, 1, 0, 5); // Sobel Edge Detection
			// on the X axis
			var sobely = new Mat();
			Cv2.Sobel(imageGrayBlur, sobely, MatType.CV_64F, 0, 1, 5); // Sobel Edge Detection
			// on the Y axis
			var sobelxy = new Mat();
			Cv2.Sobel(imageGrayBlur, sobelxy, MatType.CV_64F, 1, 1, 5); // Combined X and Y Sobel

			// Canny Edge Detection
			var edgesWOBlur = new Mat();
			var edges = new Mat();
			Cv2.Canny(imageGray, edgesWOBlur, 100, 200); // Canny Edge Detection
			Cv2.Canny(imageGrayBlur, edges, 100, 200); // Canny Edge Detection
			var cannyProj = new Mat();
			Cv2.Canny(imgProt, cannyProj, 100, 200); // Canny Edge Detection


			return this._instruments.PackResult(
				new ShowCont("org", img),
				new ShowCont(
					new ShowCont("sobelx", sobelx),
					new ShowCont("sobely", sobely),
					new ShowCont("sobelxy", sobelxy)),
				new ShowCont(
					new ShowCont("'Canny WO blur'", edgesWOBlur),
					new ShowCont("'Canny with blur'", edges)),
				new ShowCont(
					new ShowCont("proj", imgProt),
					new ShowCont("'Canny proj'", cannyProj))
			);
		}
	}
}