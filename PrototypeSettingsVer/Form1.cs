using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PrototypeFileDialog;
using PrototypeFileDialog.Impl;
using PrototypeFileDialog.Interfaces;
using Point = OpenCvSharp.Point;

namespace PrototypeSettingsVer {
	public partial class Form1 : Form {
		public AnalyzeWithCustomThreshold Analyzer = new AnalyzeWithCustomThreshold();
		private IDrawCounters _drawCounters = new DrawCountersVer1();
		private Label label1;
		private TaskFactory _factory = new TaskFactory();

		public Form1() {
			InitializeComponent();
			UpdateThreshold(110);

			this.textBox1.Click += (sender, args) => {
				string imagePath = GetImagePathFromCustomer();
				EFileStatus correctPathStatus = GetCorrectPathStatus(imagePath);

				UpdateImage(imagePath, correctPathStatus);
			};

			this.trackBar1.Scroll += (sender, args) => {
				int value = this.trackBar1.Value;

				UpdateThreshold(value);
			};
		}

		private void UpdateThreshold(int threshold) {
			this.Analyzer.Thresh = threshold;
			this.label1.Text = threshold.ToString();
			this.trackBar1.Value = threshold;

			string path = this.textBox1.Text;
			EFileStatus correctPathStatus = GetCorrectPathStatus(path);
			UpdateImage(path, correctPathStatus);
		}

		private void UpdateImage(string path, EFileStatus status) {
			switch (status) {
				case EFileStatus.Ok:
					StatusOkUpdate(path);
					break;
				case EFileStatus.NotFound:
					StatusNotFoundUpdate(path);
					break;
				case EFileStatus.EmptyPath:
					StatusEmptyUpdate();
					break;
			}

			this.textBox1.Text = path;
		}

		private void StatusOkUpdate(string path) {
			this.textBox1.ForeColor = Color.Green;
			this.textBox1.Text = path;

			this.pictureBox2.Image = Image.FromFile(path);

			this._factory.StartNew(() => {
				Mat artifacts = this.Analyzer.SearchArtifacts(path, out int threshLevel);
				Mat drawCounters = this._drawCounters.DrawCounters(Cv2.ImRead(path), artifacts);
				drawCounters.PutText(
					threshLevel.ToString(),
					new Point(10, 500),
					HersheyFonts.HersheyComplex,
					2,
					Scalar.Yellow,
					5,
					LineTypes.AntiAlias);

				this.pictureBox1.Image = drawCounters.ToBitmap();
			});
		}

		private void StatusEmptyUpdate() {
			this.textBox1.ForeColor = Color.Brown;
			this.textBox1.Text = "Empty";
		}

		private void StatusNotFoundUpdate(string path) {
			this.textBox1.ForeColor = Color.Brown;
			this.textBox1.Text = path;
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

		private static string GetImagePathFromCustomer() {
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : "";
		}
	}
}