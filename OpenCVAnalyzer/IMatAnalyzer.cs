using OpenCvSharp;

namespace OpenCVAnalyzer {
	public interface IMatAnalyzer {
		public Mat GetMask(Mat image);
	}
}