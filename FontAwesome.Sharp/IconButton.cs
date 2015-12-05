using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Packaging;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconButton : PictureBox
    {
        public IconButton()
            : this(IconChar.Star, 16, Color.DimGray, Color.Black, false, null)
        {
        }

        public IconButton(IconChar icon, int size, Color normalColor, Color hoverColor, bool selectable, string toolTip)
        {
            IconFont = null;
            // ReSharper disable once VirtualMemberCallInContructor
            BackColor = Color.Transparent;

            // need more than this to make picturebox selectable
            if (selectable)
            {
                SetStyle(ControlStyles.Selectable, true);
                TabStop = true;
            }

            Width = size;
            Height = size;

            IconChar = icon;

            InActiveColor = normalColor;
            ActiveColor = hoverColor;

            ToolTipText = toolTip;

            MouseEnter += Icon_MouseEnter;
            MouseLeave += Icon_MouseLeave;
        }

        public IconChar IconChar
        {
            get { return _iconChar; }
            set
            {
                _iconChar = value;
                IconText = char.ConvertFromUtf32((int)_iconChar);
                Invalidate();
            }
        }

        [Localizable(true)]
        public string ToolTipText
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                if (value == null) return;
                _toolTip.IsBalloon = true;
                _toolTip.ShowAlways = true;
                _toolTip.SetToolTip(this, value);
            }
        }

        public Color ActiveColor
        {
            get { return _activeColor; }
            set
            {
                _activeColor = value;
                ActiveBrush = new SolidBrush(value);
                Invalidate();
            }
        }

        public Color InActiveColor
        {
            get { return _inActiveColor; }
            set
            {
                _inActiveColor = value;
                InActiveBrush = new SolidBrush(value);
                IconBrush = InActiveBrush;
                Invalidate();
            }
        }

        public new int Width
        {
            get { return base.Width; }
            set
            {
                // force the font size to be recalculated & redrawn
                base.Width = value;
                IconFont = null;
                Invalidate();
            }
        }

        public new int Height
        {
            get { return base.Height; }
            set
            {
                // force the font size to be recalculated & redrawn
                base.Height = value;
                IconFont = null;
                Invalidate();
            }
        }

        private IconChar _iconChar = IconChar.Star;
        private string _tooltip;
        private Color _activeColor = Color.Black;
        private Color _inActiveColor = Color.Black;
        private readonly ToolTip _toolTip = new ToolTip();
        private string IconText { get; set; }
        private Font IconFont { get; set; }
        private Brush IconBrush { get; set; } // brush currently in use
        private Brush ActiveBrush { get; set; } // brush to use when hovered over
        private Brush InActiveBrush { get; set; } // brush to use when not hovered over

        protected void Icon_MouseLeave(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            IconBrush = InActiveBrush;
            Invalidate();
        }

        protected void Icon_MouseEnter(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            IconBrush = ActiveBrush;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // is the font ready to go?
            if (IconFont == null)
            {
                SetFontSize(graphics);
            }

            // Measure string so that we can center the icon.
            var stringSize = graphics.MeasureString(IconText, IconFont, Width);
            var w = stringSize.Width;
            var h = stringSize.Height;

            // center icon
            var left = (Width - w) / 2;
            var top = (Height - h) / 2;

            // Draw string to screen.
            graphics.DrawString(IconText, IconFont, IconBrush, new PointF(left, top));

            base.OnPaint(e);

            if (!Focused) return;
            var rc = ClientRectangle;
            rc.Inflate(-2, -2);
            ControlPaint.DrawFocusRectangle(e.Graphics, rc);
        }

        private void SetFontSize(Graphics g)
        {
            IconFont = GetAdjustedFont(g, IconText, Width, Height, 4, true);
        }

        private static Font GetIconFont(float size)
        {
            return new Font(Fonts.Families[0], size, GraphicsUnit.Point);
        }

        private static Font GetAdjustedFont(Graphics g, string graphicString, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            for (double adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize = adjustedSize - 0.5)
            {
                var testFont = GetIconFont((float)adjustedSize);
                // Test the string with the new size
                var adjustedSizeNew = g.MeasureString(graphicString, testFont);
                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Fits! return it
                    return testFont;
                }
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(smallestOnFail ? minFontSize : maxFontSize);
        }

        static IconButton()
        {
            InitialiseFont();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

        private static void InitialiseFont()
        {
            try
            {
                var fontBytes = GetFontBytes();
                unsafe
                {
                    fixed (byte* pFontData = fontBytes)
                    {
                        uint dummy = 0;
                        Fonts.AddMemoryFont((IntPtr)pFontData, fontBytes.Length);
                        AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBytes.Length, IntPtr.Zero, ref dummy);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Could not load FontAwesome: {ex}");
                // log?
            }
        }

        private static byte[] GetFontBytes()
        {
            //cf: http://stackoverflow.com/questions/6005398/uriformatexception-invalid-uri-invalid-port-specified
            // NOTE: this line is important to not have the compiler optimize out the type initialization because of unused variable!
            if (string.IsNullOrWhiteSpace(PackUriHelper.UriSchemePack))
                Trace.TraceInformation("pack:// uri scheme not supported");

            var streamInfo = System.Windows.Application.GetResourceStream(
                                new Uri(@"pack://application:,,,/FontAwesome.Sharp;component/fonts/fontawesome-webfont.ttf", UriKind.Absolute));
            using (streamInfo.Stream)
                return ReadAllBytes(streamInfo.Stream);
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream != null)
                return memoryStream.ToArray();

            using (memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}