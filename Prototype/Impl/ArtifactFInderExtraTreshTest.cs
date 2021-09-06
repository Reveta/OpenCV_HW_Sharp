using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using OpenCvSharp.XImgProc;
using Prototype.Interfaces;

namespace Prototype.Impl {
	public class ArtifactFInderExtraTreshTest : IArtifactsFinder {
		private IInstruments _instruments = new InstrumentDef();

		public (Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto) {
			Cv2.CvtColor(originalPhoto, originalPhoto, ColorConversionCodes.BGR2GRAY);
			var imgBlur = originalPhoto.Clone();


			Cv2.MedianBlur(originalPhoto, originalPhoto, 9);


			int loops = 2;
			for (int i = 0; i < loops; i++) {
				Cv2.GaussianBlur(originalPhoto, originalPhoto, new Size(3, 3), 0);
			}

			var thresh = new Mat();
			// CvXImgProc.NiblackThreshold(originalPhoto, thresh, 255, ThresholdTypes.Binary, 9, -0.11,
			// LocalBinarizationMethods.Nick);

			var ver2 = new ArtifactFinderProj2Ver2(this._instruments);
			double colorAvr1 = ver2.GetAvrColorInSquare(originalPhoto, 0, 200);

			var sideLenght = 500;
			double colorAvr2 = ver2.GetAvrColorInSquare(originalPhoto,
				originalPhoto.Width / 2 - sideLenght / 2,
				originalPhoto.Height / 2 - sideLenght / 2,
				sideLenght);

			var white = 255;
			var canny = new Mat();
			double avrColor = (colorAvr1 + colorAvr2) / 2;
			canny = colorAvr1 < colorAvr2
				? originalPhoto.Canny(avrColor, white)
				: originalPhoto.Canny(white, avrColor);


			return (originalPhoto, canny);
		}
	}
}