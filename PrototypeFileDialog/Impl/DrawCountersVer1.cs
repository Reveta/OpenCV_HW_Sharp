using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenCvSharp;
using PrototypeFileDialog.Interfaces;

namespace PrototypeFileDialog.Impl {
	public class DrawCountersVer1 : IDrawCounters {
		public Mat DrawCounters(Mat imgOrg, Mat mask) {
			Point[][] contours; //vector<vector<Point>> contours;
			HierarchyIndex[] hierarchyIndexes; //vector<Vec4i> hierarchy;
			Cv2.FindContours(
				mask,
				out contours,
				out hierarchyIndexes,
				mode: RetrievalModes.CComp,
				method: ContourApproximationModes.ApproxSimple);

			var markers = new Mat(mask.Size(), MatType.CV_32S, s: Scalar.All(0));

			var componentCount = 0;
			var contourIndex = 0;

			while ((contourIndex >= 0)) {
				Cv2.DrawContours(
					markers,
					contours,
					contourIndex,
					color: Scalar.All(componentCount + 1),
					thickness: -1,
					lineType: LineTypes.Link8,
					hierarchy: hierarchyIndexes,
					maxLevel: int.MaxValue);
				componentCount++;
				contourIndex = hierarchyIndexes[contourIndex].Next;
			}

			var colorTable = new List<Vec3b>();
			for (var i = 0; i < componentCount; i++) {
				Task task = Task.Factory.StartNew(() => {
					colorTable.Add(new Vec3b(0, 0, 255));
				});

				task.Wait();
			}

			Mat clone = imgOrg.Clone();

			var watershedImage = new Mat(markers.Size(), MatType.CV_8UC3);
			for (var i = 0; i < markers.Rows; i++) {
				for (var j = 0; j < markers.Cols; j++) {
					var idx = markers.At<int>(i, j);
					if (idx == -1) {
						watershedImage.Set(i, j, new Vec3b(255, 255, 255));
					} else if (idx <= 0 || idx > componentCount) {
						watershedImage.Set(i, j, new Vec3b(0, 0, 0));
					} else {
						watershedImage.Set(i, j, colorTable[idx - 1]);
						
						Cv2.Circle(clone, j, i, 30, new Scalar(0, 0, 255), 1, LineTypes.Link8, 0);
					}
					
				}
			}

			// for (var x = 0; x < watershedImage.Rows; x++)
			// for (var y = 0; y < watershedImage.Cols; y++) {
				// var pixel = watershedImage.Get<Vec3b>(x, y);

				// if (pixel.Item0 + pixel.Item1 + pixel.Item2 != 0)
					// clone.Set(x, y, pixel);
			// }


			return clone;
		}
	}
}