using OpenCvSharp;

namespace Prototype.Interfaces {
	public interface IArtifactsFinder {
		(Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto);

		// bool IsBlobs(Mat ana);
	}
}