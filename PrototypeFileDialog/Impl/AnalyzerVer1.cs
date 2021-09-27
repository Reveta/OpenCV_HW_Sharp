using OpenCVAnalyzer;
using OpenCVAnalyzer.Impl.Proj1;
using OpenCvSharp;
using PrototypeFileDialog.Interfaces;

namespace PrototypeFileDialog.Impl {
	public class AnalyzerVer1 : IAnalyzerCV {

		private IMatAnalyzer AnalyzerCv = new MatAnalyzerVer2Zeka();
		private  IDrawCounters DrawCounters = new DrawCountersVer1();

		public Mat SearchArtifacts(string imagePath) {
			Mat image = Cv2.ImRead(imagePath);
			Mat searchArtifacts = this.AnalyzerCv.GetMask(image);
			Mat drawCounters = this.DrawCounters.DrawCounters(image, searchArtifacts);
			return drawCounters;
		}
	}
}