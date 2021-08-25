
using System.Drawing;
using OpenCvSharp;

namespace HelloWorld {
	static class Program 
	{
		static void Main() 
		{
			var path = @"C:\Users\gfdsr\OneDrive\prFiles\job\Freelance\OpenCVOrigin\cSharp\HelloWorld\HelloWorld\media\project\proj_2\Chip_001.jpg";
			using var src = new Mat(path, ImreadModes.Grayscale);
			using var dst = new Mat();
        
			Cv2.Canny(src, dst, 50, 200);
			using (new Window("src image", src)) 
			using (new Window("dst image", dst)) 
			{
				Cv2.WaitKey();
			}
		}
	}
}