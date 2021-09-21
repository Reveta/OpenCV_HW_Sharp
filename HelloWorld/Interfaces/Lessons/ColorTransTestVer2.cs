using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HelloWorld.Impl;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces.Lessons {
	public class ColorTransTestVer2 : ICodeTester {
		private readonly IInstruments _inst = new InstrumentDef();

		[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
		public List<ShowCont> Run() {
			List<Mat> images = this._inst.GetImages(@"media/project/proj_2/Chip_%03d.jpg");

			images.ForEach(img => {
				Mat distanceImg = img.Clone();
				(Scalar avrColorGrb, Mat searchedLocation) = this._inst.GetAvrColorInCenter(img, sideLenght: 700);

				for (var x = 0; x < img.Rows; x++)
				for (var y = 0; y < img.Cols; y++) {
					var pixel = img.Get<Vec3b>(x, y);

					var pixelColorDistance = (byte)GetColorDistance(pixel, avrColorGrb.ToVec3b());
					distanceImg.Set(x, y, new Vec3b(pixelColorDistance, pixelColorDistance, pixelColorDistance));
				}
 
				Mat distanceImgInv = distanceImg.Clone();
				Cv2.BitwiseNot(distanceImgInv, distanceImgInv);
				Mat threshold = distanceImg
						.Clone()
						.Threshold(
							(byte)this._inst.GetAvrColorInCenter(distanceImg, sideLenght: 200).avrColorGrb.Val0 + 35,
							maxval: 255,
							ThresholdTypes.Binary)
					;

				Mat morph = threshold
						.Clone()
						.Erode(GetKernel())
						.Erode(GetKernel())
						.Dilate(GetKernel())
						.Erode(GetKernel())
						.Dilate(GetKernel())
						.Dilate(GetKernel())
						.Dilate(GetKernel())
						.Dilate(GetKernel())
						.Dilate(GetKernel())
					;

				// Mat morphTest = new Mat();
				// Cv2.MorphologyEx(threshold, morphTest, );


				Mat result = morph.Clone().Canny(0, 255, 7)
					.Dilate(GetKernel())
					.Dilate(GetKernel())
					.Dilate(GetKernel())
					.Dilate(GetKernel());

				BlobDetection(result.Clone(), ref img);

				this._inst.ShowResults(
					this._inst.PackResult(
						new ShowCont(
							new ShowCont("org", img),
							new ShowCont("dist", distanceImg),
							new ShowCont("morph", morph)
						),
						new ShowCont(
							new ShowCont("org", img),
							new ShowCont("result", result)
						)
					));
			});
			return this._inst.PackResult();
		}

		private void BlobDetection(Mat mask, ref Mat org) {
			var detectorParams = new SimpleBlobDetector.Params {
				MinDistBetweenBlobs = 50, // 10 pixels between blobs
				//MinRepeatability = 1,

				//MinThreshold = 100,
				//MaxThreshold = 255,
				//ThresholdStep = 5,

				// FilterByArea = false,
				FilterByArea = true,
				// MinArea = 0.011f, // 10 pixels squared
				MaxArea = 6000,

				FilterByCircularity = false,
				//FilterByCircularity = true,
				//MinCircularity = 0.001f,

				// FilterByConvexity = false,
				FilterByConvexity = true,
				MinConvexity = 0.001f,
				//MaxConvexity = 10,

				// FilterByInertia = false,
				FilterByInertia = true,
				MinInertiaRatio = 0.001f,

				FilterByColor = false
				//FilterByColor = true,
				//BlobColor = 255 // to extract light blobs
			};

			var simpleBlobDetector = SimpleBlobDetector.Create(detectorParams);
			KeyPoint[] keyPoints = simpleBlobDetector.Detect(mask);
			Cv2.DrawKeypoints(
				image: org,
				keypoints: keyPoints,
				outImage: org,
				color: Scalar.FromRgb(255, 0, 0),
				flags: DrawMatchesFlags.Default);
		}

		private Mat GetKernel() {
			byte[] kernelValues = {
				0, 1, 0,
				1, 1, 1,
				0, 1, 0
			};
			return new Mat(3, 3, MatType.CV_8UC1, kernelValues);
		}

		private double GetColorDistance(Vec3b color, Vec3b baseColor) {
			var d = new Vec3b((byte)Math.Abs(color.Item0 - baseColor.Item0),
				(byte)Math.Abs(color.Item1 - baseColor.Item1),
				(byte)Math.Abs(color.Item2 - baseColor.Item2));

			var distance = Math.Sqrt(d.Item0 * d.Item0 + d.Item1 * d.Item1 +
			                         d.Item2 * d.Item2);
			return distance;
		}
	}
}