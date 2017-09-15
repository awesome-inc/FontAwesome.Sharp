using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// Icon cachig logic
    /// </summary>
    public class IconCache
    {
        /// <summary>
        /// Global cache
        /// </summary>
        private static Dictionary<IconKey, Bitmap> cache = new Dictionary<IconKey, Bitmap>();
        
        /// <summary>
        /// Icon key for dictionary
        /// </summary>
        private class IconKey : IEquatable<IconKey>
        {
            public readonly IconChar icon;
            public readonly int size = 0;
            public readonly uint fore;
            public readonly uint back;
            public readonly IconFlip flip;
            public readonly int rotation;
            public readonly int hash;

            public IconKey(IconChar icon, int size, Color fore, Color back, IconFlip flip, int rotation)
            {
                this.icon = icon;
                this.size = size;
                this.fore = (uint)fore.ToArgb();
                this.back = (uint)back.ToArgb();
                this.flip = flip;
                this.rotation = rotation;
                hash = (
                    this.icon.ToString() +
                    this.size.ToString() +
                    this.fore.ToString() +
                    this.back.ToString() +
                    this.flip.ToString() +
                    this.rotation.ToString()
                ).GetHashCode();
            }
            public override bool Equals(object k)
            {
                return Equals(k as IconKey);
            }

            public bool Equals(IconKey k)
            {
                return 
                    (icon == k.icon) && 
                    (size == k.size) && 
                    (fore == k.fore) && 
                    (back == k.back) &&
                    (flip == k.flip) &&
                    (rotation == k.rotation)
                ;
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }
        
        /// <summary>
        /// Get icon from cache. If icon not in cache yet - add this icon to cache.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="size"></param>
        /// <param name="fore"></param>
        /// <param name="back"></param>
        /// <param name="flip"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Bitmap this[IconChar icon, int size, Color fore, Color back, IconFlip flip, int rotation]
        {
            get
            {
                IconKey k = new IconKey(icon, size, fore, back, flip, rotation);

                if (!cache.TryGetValue(k, out Bitmap cachedImage))
                {
                    //Debug.WriteLine(
                    //    "Image cahed: " + icon + " / " + 
                    //    size + " / " +
                    //    fore.ToArgb().ToString("x") + " / " +
                    //    back.ToArgb().ToString("x") + " / " +
                    //    flip.ToString() + " / " +
                    //    rotation 
                    //);
                    cachedImage = icon.ToBitmap1(size, fore, back);
                    cache[k] = cachedImage;
                }
                return cachedImage;
            }
        }
    }
}
