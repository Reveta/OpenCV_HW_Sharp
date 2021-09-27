using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using PrototypeFileDialog;

namespace PrototypeSettingsVer {
	public class AnalyzeWithCustomThreshold : IAnalyzerCV {
		public int Thresh { get; set; } = 70;
		private string _lastImage = "";
		private Mat loadedMat;


		public Mat SearchArtifacts(string imagePath) {
			UpdateIfNeed(imagePath);

			Mat image = this.loadedMat.Clone();
			Mat process = Process(image, this.Thresh);
			
			return process;
		}

		public Mat SearchArtifacts(string imagePath, out int threshold) {
			UpdateIfNeed(imagePath);

			threshold = this.Thresh;
			Mat image = this.loadedMat.Clone();
			Mat process = Process(image, threshold);
			
			return process;
		}

		private Mat Process(Mat image, int threshLevel) {
			Mat cvtColor = image.CvtColor(ColorConversionCodes.BGR2GRAY);
			IInstruments inst = new InstrumentDef();
			Mat resizePhoto = inst.ResizePhoto(cvtColor, cvtColor.Width / 2, cvtColor.Height / 2);
			Mat gaussianBlur = resizePhoto.GaussianBlur(new Size(31, 31), 0);

			var outputArray = new Mat(new int[] { resizePhoto.Height, resizePhoto.Width }, MatType.CV_32F);
			Cv2.Subtract(resizePhoto, gaussianBlur, outputArray);
			MatExpr matExpr = outputArray.Abs();


			Mat threshold = matExpr.ToMat().Threshold(threshLevel, 255, ThresholdTypes.Binary);

			Mat reversSize = inst.ResizePhoto(threshold, threshold.Width * 2, threshold.Height * 2);
			return reversSize;
		}
 
		private void UpdateIfNeed(string imagePath) {
			if (!imagePath.Equals(this._lastImage)) {
				this._lastImage = imagePath;
				this.loadedMat = Cv2.ImRead(this._lastImage);
			}
		}
		
		
	}
}