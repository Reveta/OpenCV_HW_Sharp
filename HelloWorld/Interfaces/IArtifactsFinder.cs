using OpenCvSharp;

namespace HelloWorld.Interfaces {
	public interface IArtifactsFinder {
		(Mat originalBlobs, Mat maskBlobs) Analise(Mat originalPhoot);

		// bool IsBlobs(Mat ana);
	}
}