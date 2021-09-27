using System.Collections.Generic;
using HelloWorld.Impl;
using OpenCVAnalyzer.Impl.Proj1;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld.Interfaces.Lessons {
	public class JekaVer1 : ICodeTester {
		public IInstruments Instruments = new InstrumentDef();
		public List<ShowCont> Run() {
			Mat img = this.Instruments.GetImages(@"media/project/proj_2/Chip_%03d.jpg")[1];
			var matAnalyzerVer2Zeka = new MatAnalyzerVer2Zeka();
			Mat analyze = matAnalyzerVer2Zeka.GetMask(img);
			return this.Instruments.PackResult();
		}
	}
}