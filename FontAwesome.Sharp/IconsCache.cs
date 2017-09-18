using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    /// <summary>
    ///     Icon caching logic
    /// </summary>
    public static class IconsCache
    {
        /// <summary>
        ///    Global bitmap icon cache - contains all icons. 
        ///    Useful when creating bitmaps becomes expensive (<see cref="IconPictureBox"/>),
        ///    and in cases, when you have icons dublicates - it allow to save memory 
        ///    and use one icon in several controls.
        /// </summary>
        private readonly static Dictionary<IconKey, CachedBitmap> cache = new Dictionary<IconKey, CachedBitmap>();

        /// <summary>
        ///     Get icon from cache. If icon not in cache yet - add this icon to cache and return bitmap.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="size"></param>
        /// <param name="fore"></param>
        /// <param name="back"></param>
        /// <param name="flip"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Bitmap Get(Control container, IconChar icon, int size, Color fore, Color back, IconFlip flip, float rotation)
        {
            IconKey k = new IconKey(icon, size, fore, back, flip, rotation);
            if (!cache.TryGetValue(k, out CachedBitmap cachedImage))
            {
                cachedImage = new CachedBitmap(icon.ToBitmapGdi(size, fore, back), container);
                cache[k] = cachedImage;
            }
            return cachedImage.Bitmap;
        }
        
        /// <summary>
        /// Dispose all icons, associated with this control
        /// </summary>
        /// <param name="container"></param>
        public static void Dispose(Control container)
        {
            // List of keys for removing: C# can't remove item from dictionary while iterating it
            List<IconKey> removeItems = new List<IconKey>();
            foreach (var p in cache)
            {
                p.Value.Remove(container);
                if (p.Value.IsDisposeRequired())
                {
                    removeItems.Add(p.Key);
                    p.Value.Dispose();
                }
            }
            foreach (var k in removeItems)
            {
                cache.Remove(k);
            }
        }

        class IconKey : IEquatable<IconKey>
        {
            readonly uint _back;
            readonly IconFlip _flip;
            readonly uint _fore;
            readonly int _hash;
            readonly IconChar _icon;
            readonly float _rotation;
            readonly int _size;

            public IconKey(IconChar icon, int size, Color fore, Color back, IconFlip flip, float rotation)
            {
                _icon = icon;
                _size = size;
                _fore = (uint) fore.ToArgb();
                _back = (uint) back.ToArgb();
                _flip = flip;
                _rotation = rotation;
                _hash = string.Concat(  //  Concat 6 items to string in 2 steps. For 5 and more strings generated
                    string.Concat(      //  code is less effective. See more: https://www.dotnetperls.com/string-concat
                        _icon, _size, _fore, _back
                    ), _flip, _rotation
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

        /// <summary>
        ///   Cached icon item
        /// </summary>
        class CachedBitmap : IDisposable
        {
            /// <summary>
            ///   Cached icon bitmap
            /// </summary>
            public Bitmap Bitmap = null;
            /// <summary>
            ///   List of controls, which still use this icon
            /// </summary>
            public List<Control> Controls = new List<Control>();

            public CachedBitmap(Bitmap b, Control c)
            {
                Bitmap = b;
                Controls.Add(c);
            }

            /// <summary>
            ///   Add control to controls list for this icon
            /// </summary>
            /// <param name="c"></param>
            public void Add(Control c)
            {
                if (Controls.Contains(c)) { return; }
                Controls.Add(c);
            }

            /// <summary>
            ///   Remove control from list of controls
            /// </summary>
            /// <param name="c"></param>
            public void Remove(Control c)
            {
                Controls.Remove(c);
            }

            /// <summary>
            ///   Return true, if controls list are empty
            /// </summary>
            /// <returns></returns>
            public bool IsDisposeRequired()
            {
                return Controls.Count == 0;
            }

            /// <summary>
            ///   Dispose this cached bitmap
            /// </summary>
            public void Dispose()
            {
                Bitmap.Dispose();
                Controls = null;
            }
            
            /// <summary>
            ///   Dispose icons for this container
            /// </summary>
            /// <param name="container"></param>
            public void Dispose(Control container)
            {
                List<IconKey> removeItems = new List<IconKey>();
                foreach (var p in cache)
                {
                    p.Value.Remove(container);
                    if (p.Value.IsDisposeRequired())
                    {
                        removeItems.Add(p.Key);
                        p.Value.Dispose();
                    }
                }
                foreach (var k in removeItems)
                {
                    cache.Remove(k);
                }
            }
        }
    }
}