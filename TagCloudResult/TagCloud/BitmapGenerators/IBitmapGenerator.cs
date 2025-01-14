using System.Drawing;
using ResultTools;

namespace TagCloud.BitmapGenerators;

public interface IBitmapGenerator
{
    Result<Bitmap> GenerateBitmapFromWords(IEnumerable<CloudWord> words);
}
