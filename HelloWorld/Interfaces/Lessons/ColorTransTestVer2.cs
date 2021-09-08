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
				Scalar avrColorGrb = GetAvrColorInCenter(img, sideLenght: 600);

				for (var x = 0; x < img.Rows; x++)
				for (var y = 0; y < img.Cols; y++) {
					var pixel = img.Get<Vec3b>(x, y);

					var pixelColorDistance = (byte)GetColorDistance(pixel, avrColorGrb.ToVec3b());
					distanceImg.Set(x, y, new Vec3b(pixelColorDistance, pixelColorDistance, pixelColorDistance));
				}

				Mat distanceImgInv = distanceImg.Clone();
				Cv2.BitwiseNot(distanceImgInv, distanceImgInv);

				Cv2.GaussianBlur(distanceImg, distanceImg, new Size(7, 7), 0);
				Cv2.GaussianBlur(distanceImg, distanceImg, new Size(7, 7), 0);
				Scalar avrColorDistanceImg = GetAvrColorInCenter(distanceImg, sideLenght: 200);
				Mat threshold = distanceImg.Clone().Threshold(avrColorDistanceImg.Val0+50, 255, ThresholdTypes.Binary);

				this._inst.ShowResults(
					this._inst.PackResult(
						new ShowCont(
							// new ShowCont("org", img),
							new ShowCont("threshold", threshold),
							new ShowCont("distanceImg", distanceImg)
							// new ShowCont("distanceImgInv", distanceImgInv)
						)
					));
			});

			return this._inst.PackResult();
		}

		private double GetColorDistance(Vec3b color, Vec3b baseColor) {
			var d = new Vec3b((byte)Math.Abs(color.Item0 - baseColor.Item0),
				(byte)Math.Abs(color.Item1 - baseColor.Item1),
				(byte)Math.Abs(color.Item2 - baseColor.Item2));

			var distance = Math.Sqrt(d.Item0 * d.Item0 + d.Item1 * d.Item1 +
			                         d.Item2 * d.Item2);

			return distance;
		}

		private Scalar GetAvrColorInCenter(Mat img, int sideLenght) {
			int halfSide = sideLenght / 2;

			int widthStart = img.Width / 2 - halfSide;
			int widthEnd = img.Width / 2 + halfSide;
			int heightStart = img.Height / 2 - halfSide;
			int heightEnd = img.Height / 2 + halfSide;

			Scalar scalar = Cv2.Mean(img[
				widthStart, widthEnd,
				heightStart, heightEnd]
			);

			return scalar;
		}
	}
}