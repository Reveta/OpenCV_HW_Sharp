using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace OpenCVInstruments.Containers {
	public class ShowCont {
		public string Name { get; set; }
		public Mat Image { get; set; }
		
		public int Count { get; set; }
		public bool IsMulti { get; set; }

		public List<ShowCont> Conts = new List<ShowCont>();

		public ShowCont(string name, Mat image) {
			this.Name = name;
			this.Image = image;
			this.Count = 1;
			this.IsMulti = false;
		}
		
		public ShowCont(params ShowCont[] conts) {
			this.Conts = conts.ToList();
			this.Count = conts.Length;
			this.IsMulti = true;
		}

		
		public override string ToString() => $"{nameof(this.Name)}: {this.Name}, {nameof(this.Image)}: {this.Image}";
	}
}