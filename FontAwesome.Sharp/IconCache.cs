using System;
using System.Collections.Generic;
using System.Drawing;

namespace FontAwesome.Sharp
{
    /// <summary>
    ///     Icon caching logic
    /// </summary>
    public class IconCache
    {
        /// <summary>
        ///   A bitmap cache. Useful when creating bitmaps becomes expensive (<see cref="IconPictureBox"/>).
        /// </summary>
        private readonly Dictionary<IconKey, Bitmap> _cache = new Dictionary<IconKey, Bitmap>();

        /// <summary>
        ///     Get icon from cache. If icon not in cache yet - add this icon to cache.
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
                var k = new IconKey(icon, size, fore, back, flip, rotation);
                if (_cache.TryGetValue(k, out var cachedImage)) return cachedImage;
                cachedImage = icon.ToBitmapGdi(size, fore, back);
                _cache[k] = cachedImage;
                return cachedImage;
            }
        }

        private class IconKey : IEquatable<IconKey>
        {
            private readonly uint _back;
            private readonly IconFlip _flip;
            private readonly uint _fore;
            private readonly int _hash;
            private readonly IconChar _icon;
            private readonly int _rotation;
            private readonly int _size;

            public IconKey(IconChar icon, int size, Color fore, Color back, IconFlip flip, int rotation)
            {
                _icon = icon;
                _size = size;
                _fore = (uint) fore.ToArgb();
                _back = (uint) back.ToArgb();
                _flip = flip;
                _rotation = rotation;
                _hash = (
                    _icon +
                    _size.ToString() +
                    _fore +
                    _back +
                    _flip +
                    _rotation
                ).GetHashCode();
            }

            public bool Equals(IconKey k)
            {
                return
                    _icon == k._icon &&
                    _size == k._size &&
                    _fore == k._fore &&
                    _back == k._back &&
                    _flip == k._flip &&
                    _rotation == k._rotation
                    ;
            }

            public override bool Equals(object k)
            {
                return Equals(k as IconKey);
            }

            public override int GetHashCode()
            {
                return _hash;
            }
        }
    }
}