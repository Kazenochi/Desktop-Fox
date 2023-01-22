using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    public static class VLCCommands
    {
        public static readonly string Loop = "--input-repeat=65545";
        public static readonly string Transform = "--video-filter=transform";
        public static readonly string Rotate90 = "--transform-type";
        public static readonly string Rotate180 = "--transform-type=180";
        public static readonly string Rotate270 = "--transform-type=270";
        public static readonly string Mute = "--key-vol-mute";
    }
}
