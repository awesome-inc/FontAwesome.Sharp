using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FontAwesome.Sharp
{
    /// <inheritdoc />
    /// <summary>
    ///     Icon caching logic.
    /// </summary>
    public class IconCache : IDisposable
    {
        private readonly IDictionary<IconKey, Bitmap> _cache = new Dictionary<IconKey, Bitmap>();

        /// <summary>
        ///     Get or create icon.
        /// </summary>
        /// <param name="icon">Icon to generate</param>
        /// <param name="size">Bitmap size in pixels</param>
        /// <param name="fore">Foreground color</param>
        /// <param name="back">Background color</param>
        /// <returns></returns>
        public Bitmap Get(IconChar icon, int size, Color fore, Color back)
        {
            var key = new IconKey(icon, size, fore, back);
            if (_cache.TryGetValue(key, out var bitmap)) return bitmap;
            bitmap = icon.ToBitmapGdi(size, fore, back);
            _cache[key] = bitmap;
            return bitmap;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            var bitmaps = _cache.Values.ToList();
            _cache.Clear();
            bitmaps.ForEach(b => b.Dispose());
        }

        private class IconKey : IEquatable<IconKey>
        {
            private readonly uint _back;
            private readonly uint _fore;
            private readonly int _hash;
            private readonly IconChar _icon;
            private readonly int _size;

            public IconKey(IconChar icon, int size, Color fore, Color back)
            {
                _icon = icon;
                _size = size;
                _fore = (uint)fore.ToArgb();
                _back = (uint)back.ToArgb();
                unchecked
                {
                    _hash = (int)_back;
                    _hash = (_hash * 397) ^ (int)_fore;
                    _hash = (_hash * 397) ^ (int)_icon;
                    _hash = (_hash * 397) ^ _size;
                }
            }

            public bool Equals(IconKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _back == other._back && _fore == other._fore && _icon == other._icon && _size == other._size;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((IconKey) obj);
            }

            public override int GetHashCode()
            {
                return _hash;
            }

            public static bool operator ==(IconKey left, IconKey right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(IconKey left, IconKey right)
            {
                return !Equals(left, right);
            }
        }
    }
}