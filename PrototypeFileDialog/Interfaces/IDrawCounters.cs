using OpenCvSharp;

namespace PrototypeFileDialog.Interfaces {
	public interface IDrawCounters {
		public Mat DrawCounters(Mat imgOrg, Mat mask);
	}
}