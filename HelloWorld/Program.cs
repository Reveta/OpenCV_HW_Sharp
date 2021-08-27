using System;
using System.Collections.Generic;
using HelloWorld.Impl;
using HelloWorld.Interfaces;
using OpenCvSharp;

namespace HelloWorld {
	static class Program {
		private static IArtifactsFinder _finder;
		private static IInstruments _instruments;

		private static void Config() {
			_instruments = new InstrumentDef();
			_finder = new ArtifactFinderProj2(_instruments);
		}


		static void Main() {
			Config();

			List<Mat> images = _instruments.GetImages(@"media/project/proj_2/Chip_%03d.jpg");
			images.ForEach(originalPhoto => {
				
				(Mat originalBlobs, Mat maskBlobs) results = _finder.Analise(originalPhoto);
				using Mat orgBLob = _instruments.ResizePhoto(results.originalBlobs, 1000, 800);
				using Mat maskBlob = _instruments.ResizePhoto(results.maskBlobs, 1000, 800);
				using (new Window("org", orgBLob))
				using (new Window("mask", maskBlob)) {
					Cv2.WaitKey();
				}
				
			});

			
		}
	} 
}