using System;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using Prototype.Interfaces;

namespace Prototype.Impl {
	public class ArtifactFinederColorTransTest : IArtifactsFinder {
		private IInstruments _instruments = new InstrumentDef();

		public (Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto) {
			var gray = new Mat();
			Cv2.CvtColor(originalPhoto, gray, ColorConversionCodes.BGR2GRAY);

			(double colorAvr1, double colorAvr2) = GetAvrColor(gray);

			Mat draw1 = DrawDistFromColor(gray, colorAvr1);
			Mat draw2 = DrawDistFromColor(gray, colorAvr2);
			Mat draw3 = DrawDistFromColor(gray, (colorAvr2 + colorAvr1) / 2);


			return (draw1, draw2);
		}

		(double colorAvr1, double colorAvr2) GetAvrColor(Mat originalPhoto) {
			var ver2 = new ArtifactFinderProj2Ver2(this._instruments);
			double colorAvr1 = ver2.GetAvrColorInSquare(originalPhoto, 0, 200);

			var sideLenght = 500;
			double colorAvr2 = ver2.GetAvrColorInSquare(originalPhoto,
				originalPhoto.Width / 2 - sideLenght / 2,
				originalPhoto.Height / 2 - sideLenght / 2,
				sideLenght);

			return (colorAvr1, colorAvr2);
		}

		Mat DrawDistFromColor(Mat image, double baseColor) {
			Mat result = image.Clone();

			for (var x = 0; x < 1000; x++)
			for (var y = 0; y < 1000; y++) {
				var pixel = image.Get<Vec3b>(x, y);

				double abs = Math.Abs(pixel.Item0 - baseColor);

				result.Set(x, y, abs);
			}

			return result;
		}
	}
}