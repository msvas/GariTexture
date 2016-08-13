#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <string.h>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>

using namespace cv;

int main(int argc, char *argv[]) {
  CvCapture *capture = 0;
  IplImage *frame = 0;
  int key = 0;
  /* initialize camera */
  capture = cvCaptureFromCAM(0);
  /* always check */
  if ( !capture ) {
    printf("Cannot open initialize webcam!\n" );
    exit(0);
  }

/* create a window for the video */
  cvNamedWindow("result", CV_WINDOW_AUTOSIZE);
  while(key != 'q') {
    /* get a frame */
    frame = cvQueryFrame(capture);

    /* always check */
    if(!frame)
      break;

    Mat image(frame);
    Mat mask = Mat::zeros(image.size(), CV_8UC1);
    Point circleCenter(mask.cols / 2, mask.rows / 2);

    int radius = 50;
    circle(mask, circleCenter, radius, CV_RGB(255, 255, 255), CV_FILLED);

    Mat imagePart = cv::Mat::zeros(image.size(), image.type());
    image.copyTo(imagePart, 255 - mask);

    IplImage newImg = imagePart;
    frame = &newImg;

    /* sets the Region of Interest*/
    //cvSetImageROI(frame, cvCircle(150, 50, 150, 250));
    /* create destination image */
    IplImage *img2 = cvCreateImage(cvGetSize(frame),
                                    frame->depth,
                                    frame->nChannels);

    /*
    * do the main processing with subimage here.
    * in this example, we simply invert the subimage
    */
    //cvNot(frame, frame);
    /* copy subimage */
    //cvCopy(frame, img2, NULL);
    /* always reset the Region of Interest */
    //cvResetImageROI(frame);

    /* display current frame */
    cvShowImage("result", frame);
    /* exit if user press 'q' */
    key = cvWaitKey( 1 );
  }

  /* free memory */
  cvDestroyWindow( "result" );
  cvReleaseCapture( &capture );
  return 0;
}
