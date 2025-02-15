﻿using System;
using System.Collections.Generic;
using HelloWorld.Impl;
using HelloWorld.Interfaces.Lessons;
using OpenCVInstruments.Containers;
using OpenCVInstruments.Impl;
using OpenCVInstruments.Interfaces;
using OpenCvSharp;

namespace HelloWorld {
	static class Program {
		private static IInstruments _instruments;
		private static ICodeTester _tester;

		private static void Config() {
			// _tester = new EdgeDetector();
			// _tester = new ExtraThreshold();
			// _tester = new ColorTransTest();
			// _tester = new ColorTransTestVer2();
			_tester = new JekaVer1();
			// _tester = new MorphologySample();
			_instruments = new InstrumentDef();
		}


		static void Main() {
			Config();

			List<ShowCont> showConts = _tester.Run();

			_instruments.PrintResultStats(showConts);
			_instruments.ShowResults(showConts);
			
			Cv2.DestroyAllWindows();
		}
	}
}