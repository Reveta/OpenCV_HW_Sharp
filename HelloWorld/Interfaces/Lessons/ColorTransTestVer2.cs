using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using HelloWorld.Impl;
using OpenCVAnalyzer;
using OpenCVAnalyzer.Impl.Proj1;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces.Lessons {
	public class ColorTransTestVer2 : ICodeTester {
		private readonly IInstruments _inst = new InstrumentDef();

		private readonly IMatAnalyzer _analyzer = new MatAnalyzerVer1();

		private readonly ProgressBar _progress = new();
		private int _count = 0;


		[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
		public List<ShowCont> Run() {
			Mat img = this._inst.GetImages(@"media/project/proj_2/Chip_%03d.jpg")[0];

			Console.Write("Performing some task... ");

			List<ShowCont> packResult = this._inst.PackResult();
			// images.ForEach(img => {
				Mat clone = img.Clone();
				packResult.Add(
					new ShowCont(
						// new ShowCont("org", img),
						new ShowCont("Result", this._analyzer.GetMask(clone)
						)));


				// NestProgress(images.Count);
			// });

			return packResult;
		}

		private void NestProgress(int max) {
			this._progress.Report((double) ++this._count / max);
			if (this._count == max) {
				Thread.Sleep(200);
				this._progress.Dispose();
				Console.WriteLine();
			}
		}
	}
}