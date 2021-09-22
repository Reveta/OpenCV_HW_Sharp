using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces {
	public class ArtifactFinderProj2Ver2 {
		private readonly IInstruments _instruments;

		public ArtifactFinderProj2Ver2(IInstruments instruments) { this._instruments = instruments; }


		public (Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto) {
			Mat image = originalPhoto.Clone();
			Cv2.CvtColor(originalPhoto, image, ColorConversionCodes.BGR2GRAY);
			double colorAvr1 = GetAvrColorInSquare(image, 0, 200);

			var sideLenght = 500;
			double colorAvr2 = GetAvrColorInSquare(image,
				originalPhoto.Width / 2 - sideLenght / 2,
				originalPhoto.Height / 2 - sideLenght / 2,
				sideLenght);

			var white = 255;
			double avrColor = (colorAvr1 + colorAvr2) / 2;
			image = colorAvr1 < colorAvr2
				? image.Threshold(avrColor, white, ThresholdTypes.Binary)
				: image.Threshold(avrColor, white, ThresholdTypes.BinaryInv);


			return (originalPhoto, image);
		}

		public double GetAvrColorInSquare(Mat imageGrayScale, int x1, int y1, int sideLenght) {
			Rect rectCrop = new Rect(x1, y1, sideLenght, sideLenght);
			Mat croppedImage = new Mat(imageGrayScale, rectCrop);

			Scalar mean = Cv2.Mean(croppedImage);

			return mean.Val0;
		}

		public double GetAvrColorInSquare(Mat imageGrayScale, int xy1, int sideLenght) {
			return GetAvrColorInSquare(imageGrayScale, xy1, xy1, sideLenght);
		}
	}
}