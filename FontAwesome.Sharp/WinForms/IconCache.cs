using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// An icon cache for optimizing rendering performance for commonly used icons
    /// </summary>
    public class IconCache<TEnum> : IDisposable
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        private readonly IDictionary<IconKey, Bitmap> _cache = new Dictionary<IconKey, Bitmap>();

        /// <summary>
        /// Get or create icon.
        /// </summary>
        /// <param name="fontFamily">The icon's font family</param>
        /// <param name="icon">Icon to generate</param>
        /// <param name="size">Bitmap size in pixels</param>
        /// <param name="fore">Foreground color</param>
        /// <param name="back">Background color</param>
        /// <param name="useGdi">Use GDI implementation</param>
        /// <returns></returns>
        public Bitmap Get(FontFamily fontFamily, TEnum icon, int size, Color fore, Color back, bool useGdi = false)
        {
            var key = new IconKey(icon, size, fore, back);
            if (_cache.TryGetValue(key, out var bitmap)) return bitmap;
            bitmap = useGdi
                ? fontFamily.ToBitmapGdi(icon, size, fore, back)
                : fontFamily.ToBitmap(icon, size, fore);
            _cache[key] = bitmap;
            return bitmap;
        }

        protected virtual void Dispose(bool disposing)
        {
            var bitmaps = _cache.Values.ToList();
            _cache.Clear();
            bitmaps.ForEach(b => b.Dispose());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class IconKey : IEquatable<IconKey>
        {
            private readonly uint _back;
            private readonly uint _fore;
            private readonly int _hash;
            private readonly TEnum _icon;
            private readonly int _size;

            public IconKey(TEnum icon, int size, Color fore, Color back)
            {
                _icon = icon;
                _size = size;
                _fore = (uint)fore.ToArgb();
                _back = (uint)back.ToArgb();
                unchecked
                {
                    _hash = (int)_back;
                    _hash = (_hash * 397) ^ (int)_fore;
                    _hash = (_hash * 397) ^ _icon.GetHashCode();
                    _hash = (_hash * 397) ^ _size;
                }
            }

            public bool Equals(IconKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _back == other._back && _fore == other._fore
                                            && _icon.CompareTo(other._icon) == 0
                                            && _size == other._size;
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
