using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EOTools.Tools.AssetParser;

public class AssetPartViewModel
{
    public ImageSource? Image { get; set; }

    public required AssetFrameModel FrameData { get; set; }

    public required string SourcePath { get; set; }

    public void Load()
    {
        Uri myUri = new(SourcePath, UriKind.RelativeOrAbsolute);
        PngBitmapDecoder decoder = new(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
        BitmapSource bitmapSource = decoder.Frames[0];

        Image = new CroppedBitmap(bitmapSource, new Int32Rect()
        {
            X = FrameData.FrameDimensions.PositionX,
            Y = FrameData.FrameDimensions.PositionY,
            Width = FrameData.FrameDimensions.Width,
            Height = FrameData.FrameDimensions.Height,
        });
    }
}
