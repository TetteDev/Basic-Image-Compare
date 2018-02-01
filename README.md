# Basic-Image-Compare
Compare 1 bitmap against one or more bitmaps and get difference in image color ratios in %

Usage

```cs
ImageCompare ic = new ImageCompare();
Bitmap originalBMP = (Bitmap)Image.FromFile("path to bitmap");

// Will compare bitmap loaded in variable 'originalBitmap' to all images in the directory specified in the parameters
List<ImageDataSingle> results = ic.CompareBitmapsMultiple(directorywithimages, originalBMP, true);


// Wil compare one bitmap to another
Bitmap bitmapToCompareWith = (Bitmap)Image.FromFile("path to bitmap");
ImageDataSingle result = ic.CompareBitmapsSingle(originalBMP, bitmapToCompareWith);
```

