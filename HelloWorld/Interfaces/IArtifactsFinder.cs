using OpenCvSharp;

namespace HelloWorld.Interfaces {
	public interface IArtifactsFinder {
		(Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoto);

		// bool IsBlobs(Mat ana);
	}
}