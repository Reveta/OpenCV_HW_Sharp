using OpenCvSharp;

namespace PrototypeFileDialog {
	public interface IAnalyzerCV {
		Mat SearchArtifacts(string imagePath);
	}
}