using System;
using System.Reflection.Metadata;
using HelloWorld.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Impl {
	public class ArtifactFinderProj2 : IArtifactsFinder {
		private readonly IInstruments _instruments;
		public ArtifactFinderProj2(IInstruments instruments) { this._instruments = instruments; }

		public (Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto) {
			Mat image = originalPhoto.Clone();

			image = image.Threshold(100, 255, ThresholdTypes.Binary);
			// image = FilteringSharp(image);

			//TODO normal bluring and sharping
			var imageWidth = image.Width;
			var imageHeight = image.Height;
			image = this._instruments.ResizePhoto(image, 600, 480);
			image = this._instruments.ResizePhoto(image, imageWidth, imageHeight);

			//Invert color
			// Cv2.BitwiseNot(image, image);

			// Cv2.BilateralFilter(image, image, MatType.CV_32F, 75, 75);


			KeyPoint[] keyPoints = BlobDetect(image);

			Cv2.DrawKeypoints(image, keyPoints, image, Scalar.Red, DrawMatchesFlags.Default);
			Cv2.DrawKeypoints(originalPhoto, keyPoints, originalPhoto, Scalar.Red, DrawMatchesFlags.Default);

			return (originalPhoto, image);
		}

		private KeyPoint[] BlobDetect(Mat image) {
			var detector = SimpleBlobDetector.Create(
				new SimpleBlobDetector.Params() {
					// MinThreshold = 100,
					FilterByArea = true,
					MinArea = 200,
					FilterByConvexity = true,
					MinConvexity = 0.01f,
					// FilterByInertia = true,
					// MaxInertiaRatio = 0
				});

			KeyPoint[] keyPoints = detector.Detect(image);
			return keyPoints;
		}

		private Mat FilteringSharp(Mat image) {
			// TODO maybe bugged
			/*double[,] array = {
				{ -1, -1, -1 },
				{ -1,  9, -1 },
				{ -1, -1, -1 }
			}*/
			;

			double[,] array = {
				{ 0, 0, 0 },
				{ 0, 1, 0 },
				{ 0, 0, 0 }
			};
			Mat kernel = this._instruments.GenKernel(array, image.Type());

			var dst = new Mat();
			image.CopyTo(dst);

			Cv2.Filter2D(src: image, dst: dst, image.Type(), kernel, new Point(0, 0));
			return dst;
		}
	}
}