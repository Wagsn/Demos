using AnimatedGif;
using System.Drawing;
using System;
using System.Linq;

namespace AnimatedGifDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var imagesPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Images");
            if (!System.IO.Directory.Exists(imagesPath)) return;
            var files = System.IO.Directory.EnumerateFiles(imagesPath, "*.jpg");  // png
            if (!files.Any()) return;
            // 33ms delay (~30fps 帧每秒) 
            using (var gif = AnimatedGif.AnimatedGif.Create("gif.gif", 1000))
            {
                foreach(var path in files)
                {
                    var img = Image.FromFile(path); 
                    gif.AddFrame(img, delay: -1, quality: GifQuality.Default);
                }
            }
        }
    }
}
