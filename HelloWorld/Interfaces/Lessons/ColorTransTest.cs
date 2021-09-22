using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HelloWorld.Impl;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces.Lessons {
	public class ColorTransTest : ICodeTester {
		private IInstruments _instruments = new InstrumentDef();

		[SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: OpenCvSharp.Vec3b")]
		[SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Double")]
		public List<ShowCont> Run() {
			List<Mat> images = _instruments.GetImages(@"media/project/proj_2/Chip_%03d.jpg");

			List<ShowCont> packResult = this._instruments.PackResult();
			images.ForEach(mat => {
				Cv2.CvtColor(mat,mat, ColorConversionCodes.BGR2GRAY);
				Cv2.GaussianBlur(mat, mat, new Size(9, 9), 0);
				
				(double colorAvr1, double colorAvr2) = GetAvrColor(mat);
				Mat draw1 = DrawDistFromColor(mat, (byte)colorAvr1);
				Mat draw2 = DrawDistFromColor(mat, (byte)colorAvr2);
				double colorAvr3 = (colorAvr2 + colorAvr1) / 2;
				Mat draw3 = DrawDistFromColor(mat, (byte)colorAvr3);				
				
				Console.WriteLine($"{colorAvr1} {colorAvr2} {colorAvr3} ");
				
				var canny = new Mat();
				double st = 200;
				double avrColor = (colorAvr1 + colorAvr2) / 2;
				canny = colorAvr1 > colorAvr2
					? draw2.Canny(st, 255)
					: draw2.Canny(255, st);
				
				packResult.Add(
					new ShowCont(
						new ShowCont("org", mat),
						// new ShowCont("draw1", draw1),
						new ShowCont("draw2", draw2),
						new ShowCont("canm", canny)
						));
			});


			return packResult;
		}


		(double colorAvr1, double colorAvr2) GetAvrColor(Mat originalPhoto) {
			var ver2 = new ArtifactFinderProj2Ver2(this._instruments);
			double colorAvr1 = ver2.GetAvrColorInSquare(originalPhoto, 0, 200);

			var sideLenght = 1000;
			double colorAvr2 = ver2.GetAvrColorInSquare(originalPhoto,
				originalPhoto.Width / 2 - sideLenght / 2,
				originalPhoto.Height / 2 - sideLenght / 2,
				sideLenght);

			return (colorAvr1, colorAvr2);
		}

		Mat DrawDistFromColor(Mat image, byte baseColor) {
			Mat result = image.Clone();

			for (var x = 0; x < 1300; x++)
			for (var y = 0; y < 1300; y++) {
				var pixel = image.Get<Vec3b>(x, y);


				var pixelItem1 = pixel.Item0;
				double abs = pixelItem1 > baseColor ? baseColor - pixelItem1 : pixelItem1 - baseColor;

				result.Set(x, y, abs);
			}

			for (var x = 0; x < 100; x++)
			for (var y = 0; y < 100; y++) {

				result.Set(x, y, baseColor);

			}
			
			return result;
		}
	}
}