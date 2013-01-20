using System;
using System.ComponentModel;

namespace StasisEditor.Views.Controls
{
    [TypeConverter(typeof(XNAColorConverter))]
    public class XNAColor
    {
        private byte _red;
        private byte _green;
        private byte _blue;

        public byte red { get { return _red; } set { _red = value; } }
        public byte green { get { return _green; } set { _green = value; } }
        public byte blue { get { return _blue; } set { _blue = value; } }

        public XNAColor(byte red = 0, byte green = 0, byte blue = 0)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }

        public XNAColor(byte[] rgb)
        {
            if (rgb.Length != 3)
                throw new Exception("Array must have a length of 3.");
            _red = rgb[0];
            _green = rgb[1];
            _blue = rgb[2];
        }

        public XNAColor(int argb)
        {
            byte[] bytes = BitConverter.GetBytes(argb);
            _red = bytes[2];
            _green = bytes[1];
            _blue = bytes[0];
        }

        public XNAColor(string rgb)
        {
            string[] parts = rgb.Split(' ');
            if (parts.Length != 3)
                throw new Exception("Array must have a length of 3.");
            _red = Convert.ToByte(parts[0]);
            _green = Convert.ToByte(parts[1]);
            _blue = Convert.ToByte(parts[2]);
        }

        public new string ToString()
        {
            return String.Format("{0} {1} {2}", _red, _green, _blue);
        }

        public byte[] GetRGB()
        {
            return new byte[] { _red, _green, _blue };
        }

        public int GetARGB()
        {
            return BitConverter.ToInt32(new byte[] { _blue, _green, _red, 255 }, 0);
        }
    }
}
