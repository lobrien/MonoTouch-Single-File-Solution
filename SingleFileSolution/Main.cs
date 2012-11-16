using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace SingleFileSolution
{
	public class ContentView : UIView
	{
		protected UIColor fillColor;
		protected String extraText; 
		
		public ContentView (UIColor fillColor, String extraText)
		{
			this.fillColor = fillColor;
			this.extraText = extraText;
		}
		
		public override void Draw (System.Drawing.RectangleF rect)
		{
			var frameDescription = String.Format ("ContentView frame is [{0},{1}],[{2},{3}]", Frame.Left, Frame.Top, Frame.Right, Frame.Bottom);
			
			var ulTolr = new PointF[] { rect.Location, rect.Location + rect.Size };
			var urToll = new PointF[] { new PointF (rect.Right, rect.Top), new PointF (rect.Left, rect.Bottom) };
			using (var g = UIGraphics.GetCurrentContext ()) {
				//Fill the view with color
				fillColor.SetFill ();
				g.FillRect (rect);
				
				//Draw diagonals to indicate off-screen geometry
				var red = UIColor.Red;
				red.SetFill ();
				red.SetStroke ();
				g.AddLines (ulTolr);
				g.AddLines (urToll);
				g.DrawPath (CGPathDrawingMode.Stroke);
				
				//Show some explanatory text
				g.ScaleCTM (1f, -1f);
				g.SelectFont ("Arial", 16f, CGTextEncoding.MacRoman);
				UIColor.Black.SetFill ();
				g.SetTextDrawingMode (CGTextDrawingMode.Fill);
				g.ShowTextAtPoint (10, -50, frameDescription);
				g.ShowTextAtPoint (10, -75, extraText);
			}
		}
	}

	public class SimpleViewController : UIViewController
	{
		public SimpleViewController () : base ()
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var view = new ContentView (UIColor.Blue, "A View");
		
			this.View = view;
		}
	}	

	[Register ("AppDelegate")]
	public  class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		SimpleViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new SimpleViewController ();
			window.RootViewController = viewController;

			window.MakeKeyAndVisible ();
			
			return true;
		}
	}

	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
