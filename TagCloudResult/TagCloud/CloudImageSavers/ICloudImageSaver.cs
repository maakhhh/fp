using System.Drawing;

namespace TagCloud.CloudImageSavers;

public interface ICloudImageSaver
{
    string Save(Bitmap image);
}
