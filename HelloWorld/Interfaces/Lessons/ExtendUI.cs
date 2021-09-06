/*using System.Collections.Generic;

namespace HelloWorld.Interfaces.Lessons {
	public class ExtendUI {
		private List<int> _topLeftCorner = new();
		private List<int> _bottomRightCorner = new();

// function which will be called on mouse input
		void DrawRectangle(int action, int x, int y, int flags /**, userdata#1#) {
			// Mark the top left corner when left mouse button is pressed
			if( action == EventEVENT_LBUTTONDOWN )
			{
				top_left_corner = Point(x,y);
			}
			// When left mouse button is released, mark bottom right corner
			else if( action == EVENT_LBUTTONUP)
			{
				bottom_right_corner = Point(x,y);
				rectangle(image, top_left_corner, bottom_right_corner, Scalar(0,255,0), 2, 8 );
				imshow("Window", image);
			}
		}
		}

// Mark the top left corner when left mouse button is pressed
				if action == cv2.EVENT_LBUTTONDOWN:
		top_left_corner = [ (x, y)]

// When left mouse button is released, mark bottom right corner
		elif action == cv2.EVENT_LBUTTONUP:
		bottom_right_corner = [ (x, y)]

// Draw the rectangle
		cv2.rectangle(image, top_left_corner[0], bottom_right_corner[0], (0, 255, 0), 2, 8)
		cv2.imshow("Window",image)

// Read Images
		image = cv2.imread("../Input/sample.jpg")
// Make a temporary image, will be useful to clear the drawing
			temp = image.copy()

// Create a named window
		cv2.namedWindow("Window")
// highgui function called when mouse events occur
			cv2.setMouseCallback("Window", drawRectangle)

		k= 0
// Close the window when key q is pressed
		while k!=113:

// Display the image
		cv2.imshow("Window", image)
		k = cv2.waitKey(0)
// If c is pressed, clear the window, using the dummy image
		if (k == 99):
		image= temp.copy()
			cv2.imshow("Window", image)

		cv2.destroyAllWindows()
	}
}*/