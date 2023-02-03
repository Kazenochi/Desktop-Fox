using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox
{
    public static class VLCParameter
    {
        public static readonly string Loop = "--input-repeat=65545";
        public static readonly string Transform = "--video-filter=transform";
        public static readonly string Rotate = "--transform-type=";
        public static readonly string Rotate90 = "--transform-type=";
        public static readonly string Rotate180 = "--transform-type=180";
        public static readonly string Rotate270 = "--transform-type=270";
        public static readonly string FPS = "--fps-fps=";
        public static readonly string FPS10 = "--fps-fps=10";
        public static readonly string FPS30 = "--fps-fps=30";
        public static readonly string FPS60 = "--fps-fps=60";
        public static readonly string VideoProcessing = "--postproc-q=";
        public static readonly string VideoProcessingLQ = "--postproc-q=0";
    }
}
