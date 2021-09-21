using OpenCvSharp;

namespace PrototypeFileDialog.Impl {
	public class AnalyzerMock : IAnalyzerCV{
		public Mat SearchArtifacts(string imagePath) => Cv2.ImRead(imagePath, ImreadModes.Color);
	}
}