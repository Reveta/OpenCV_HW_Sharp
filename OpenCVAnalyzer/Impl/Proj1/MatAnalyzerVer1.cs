using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace OpenCVAnalyzer.Impl.Proj1 {
	public class MatAnalyzerVer1 : IMatAnalyzer {
		private IInstruments _inst = new InstrumentDef();

		public Mat GetMask(Mat img) {
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
						(byte)this._inst.GetAvrColorInCenter(distanceImg, sideLenght: 20).avrColorGrb.Val0 + 35,
						maxval: 255,
						ThresholdTypes.Binary)
				;

			Mat morph = threshold
					.Clone()
				.Erode(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Erode(GetKernel())
				;

			// morph.MorphologyEx(MorphTypes.Open, GetKernel(), iterations: 23);


			Mat result = morph.Clone().Canny(0, 255, 3)
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				// .Dilate(GetKernel())
				;

			
			// this._inst.ShowResults(
			// 	this._inst.PackResult(
			// 		new ShowCont(
			// 			new ShowCont("org", img),
			// 			new ShowCont("org", img)
			// 			// new ShowCont("counters", drawCounters)
			// 		)
			// 	));
			Mat drawCounters = DrawCounters(img, result);

			return img;
		}


		private Mat GetKernel() {
			byte[] kernelValues = {
				1, 1, 1, 
				1, 1, 1, 
				1, 1, 1, 
			
			};
			return new Mat(3, 3, MatType.CV_8UC1, kernelValues);
		}

		private Mat DrawCounters(Mat img, Mat result) {
			Stopwatch watch = new Stopwatch();
			watch.Start();

			Point[][] contours; //vector<vector<Point>> contours;
			HierarchyIndex[] hierarchyIndexes; //vector<Vec4i> hierarchy;
			Cv2.FindContours(
				result,
				out contours,
				out hierarchyIndexes,
				mode: RetrievalModes.CComp,
				method: ContourApproximationModes.ApproxSimple);

			watch.Stop();
			// Console.WriteLine(watch.Elapsed.TotalSeconds.ToString() + " T0");

			// Console.WriteLine(contours.Length + " contours.Length");
			// Console.WriteLine(hierarchyIndexes.Length + " hierarchyIndexes.Length");
			var markers = new Mat(result.Size(), MatType.CV_32S, s: Scalar.All(0));

			watch = new Stopwatch();
			watch.Start();
			var componentCount = 0;
			var contourIndex = 0;
			
			while ((contourIndex >= 0)) {
				Cv2.DrawContours(
					markers,
					contours,
					contourIndex,
					color: Scalar.All(componentCount + 1),
					thickness: -1,
					lineType: LineTypes.Link8,
					hierarchy: hierarchyIndexes,
					maxLevel: int.MaxValue);
				componentCount++;
				contourIndex = hierarchyIndexes[contourIndex].Next;
			}

			watch.Stop();
			// Console.WriteLine(watch.Elapsed.TotalSeconds.ToString() + " T1");
			watch = new Stopwatch();
			watch.Start();
			var colorTable = new List<Vec3b>();
			// Console.WriteLine($"componens" + componentCount);
			for (var i = 0; i < componentCount; i++) {
				Task task = Task.Factory.StartNew(() => {
					var rnd = new Random();
					var b = rnd.Next(150, 255); //Cv2.TheRNG().Uniform(0, 255);
					var g = rnd.Next(150, 255); //Cv2.TheRNG().Uniform(0, 255);
					var r = rnd.Next(150, 255); //Cv2.TheRNG().Uniform(0, 255);

					colorTable.Add(new Vec3b((byte)b, (byte)g, (byte)r));
				});

				task.Wait();
			}

			watch.Stop();
			// Console.WriteLine(watch.Elapsed.TotalSeconds.ToString() + " T2");
			var watershedImage = new Mat(markers.Size(), MatType.CV_8UC3);
			watch = new Stopwatch();
			watch.Start();
			// paint the watershed image
			for (var i = 0; i < markers.Rows; i++) {
				for (var j = 0; j < markers.Cols; j++) {
					var idx = markers.At<int>(i, j);
					if (idx == -1) {
						watershedImage.Set(i, j, new Vec3b(255, 255, 255));
					} else if (idx <= 0
					           || idx > componentCount) {
						watershedImage.Set(i, j, new Vec3b(0, 0, 0));
					} else {
						watershedImage.Set(i, j, colorTable[idx - 1]);
					}
				}
			}

			watch.Stop();
			// Console.WriteLine(watch.Elapsed.TotalSeconds.ToString() + " T3");
			watch = new Stopwatch();
			watch.Start();
// watershedImage = watershedImage * 0.5 + img.CvtColor(ColorConversionCodes.BGR2GRAY) * 0.5;
			Mat clone = img.Clone();
			for (var x = 0; x < watershedImage.Rows; x++)
			for (var y = 0; y < watershedImage.Cols; y++) {
				var pixel = watershedImage.Get<Vec3b>(x, y);

				if (pixel.Item0 + pixel.Item1 + pixel.Item2 != 0)
					clone.Set(x, y, pixel);
			}

			watch.Stop();
			// Console.WriteLine(watch.Elapsed.TotalSeconds.ToString() + " T4");

			return clone;
		}

		private double GetColorDistance(Vec3b color, Vec3b baseColor) {
			var d = new Vec3b((byte)Math.Abs(color.Item0 - baseColor.Item0),
				(byte)Math.Abs(color.Item1 - baseColor.Item1),
				(byte)Math.Abs(color.Item2 - baseColor.Item2));

			var distance = Math.Sqrt(d.Item0 * d.Item0 + d.Item1 * d.Item1 +
			                         d.Item2 * d.Item2);
			return distance;
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
	}
}