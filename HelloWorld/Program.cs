using System;
using System.Collections.Generic;
using HelloWorld.Impl;
using HelloWorld.Interfaces.Lessons;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;

namespace HelloWorld {
	static class Program {
		private static IInstruments _instruments;
		private static ICodeTester _tester;

		private static void Config() {
			_tester = new EdgeDetector();
			_instruments = new InstrumentDef();
		}


		static void Main() {
			Config();

			List<ShowCont> showConts = _tester.Run();

			_instruments.PrintResultStats(showConts);
			_instruments.ShowResults(showConts);
		}

		
	}
}