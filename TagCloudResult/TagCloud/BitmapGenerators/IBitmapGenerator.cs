using System.Drawing;

namespace TagCloud.BitmapGenerators;

public interface IBitmapGenerator
{
    Bitmap GenerateBitmapFromWords(IEnumerable<CloudWord> words);
}
