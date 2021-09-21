using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;
using Prototype.Impl;
using Prototype.Interfaces;

namespace Prototype {
	static class Program {
		private static IArtifactsFinder _finder;
		private static IInstruments _instruments;

		private static void Config() {
			_instruments = new InstrumentDef();
			_finder = new ArtifactFinederColorTransTest();
		}


		static void Main() {
			var fileContent = string.Empty;
			var filePath = string.Empty;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					filePath = openFileDialog.FileName;

					//Read the contents of the file into a stream
					var fileStream = openFileDialog.OpenFile();

					using (StreamReader reader = new StreamReader(fileStream))
					{
						fileContent = reader.ReadToEnd();
					}
				}
			}

			MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
			
			Config();

			List<Mat> images = _instruments.GetImages(@"media/project/proj_2/Chip_%03d.jpg");
			// List<Mat> images = _instruments.GetImages(@"media/project/proj_1/Chip_%03d.tif");
			images.ForEach(originalPhoto => {
				(Mat originalBlobs, Mat maskBlobs) results = _finder.Analise(originalPhoto);

				using Mat orgBLob = _instruments.ResizePhoto(results.originalBlobs, 1000, 800);
				using Mat maskBlob = _instruments.ResizePhoto(results.maskBlobs, 1000, 800);

				using (new Window("org", orgBLob))
				using (new Window("mask", maskBlob)) 
					Cv2.WaitKey();
			});
		}
	}
}