using System;
using System.IO;
using System.Windows.Forms;
using OpenCvSharp;
using PrototypeFileDialog.Impl;
using PrototypeFileDialog.Interfaces;

namespace PrototypeFileDialog {
	static class Program {
		// private static readonly IAnalyzerCV AnalyzerCv = new AnalyzerMock();
		private static readonly IAnalyzerCV AnalyzerCv = new AnalyzerVer1();

		
		[STAThread]
		static void Main() {
			InitVisual();

			string filePath = GetImagePathFromCustomer();
			var fileStatus = GetCorrectPathStatus(filePath);
			
			if (fileStatus != EFileStatus.Ok) {
				ShowWarningMessage(fileStatus);
				return;
			}

			Mat searchArtifacts = AnalyzerCv.SearchArtifacts(filePath);
			ShowAnalyzerResult(searchArtifacts, filePath);
		}

		private static void InitVisual() {
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}

		private static string GetImagePathFromCustomer() {
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : "";
		}

		private static EFileStatus GetCorrectPathStatus(string path) {
			if (path.Equals("")) {
				return EFileStatus.EmptyPath;
			}

			if (!File.Exists(path)) {
				return EFileStatus.NotFound;
			}

			return EFileStatus.Ok;
		}

		private static void ShowWarningMessage(EFileStatus status) {
			switch (status) {
				case EFileStatus.NotFound:
					MessageBox.Show($"Status {status}: Photo not exist!");
					break;
				case EFileStatus.EmptyPath:
					MessageBox.Show($"Status {status}: Path was empty");
					break;
			}
		}

		private static void ShowAnalyzerResult(Mat result, string filePath) {
			Cv2.ImShow($"Result of '{filePath}'", result);
			Cv2.WaitKey();			
		}
	}

	internal enum EFileStatus {
		Ok,
		NotFound,
		EmptyPath
	}
}