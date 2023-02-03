using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopFox.Helper
{
    public static class VLCCommandBuilder
    {
        public static string[] BuildCommands(Wallpaper wallpaper)
        {
            if (wallpaper == null) return  new string[0];

            List<string> vlcParameters = new();

            switch (wallpaper.myRotation)
            {
                case VLCRotation.None:
                    break;
                case VLCRotation.Rotate90:
                    vlcParameters.Add(VLCParameter.Transform);
                    vlcParameters.Add(VLCParameter.Rotate90);
                    break;
                case VLCRotation.Rotate180:
                    vlcParameters.Add(VLCParameter.Transform);
                    vlcParameters.Add(VLCParameter.Rotate180);
                    break;
                case VLCRotation.Rotate270:
                    vlcParameters.Add(VLCParameter.Transform);
                    vlcParameters.Add(VLCParameter.Rotate270);
                    break;
            }

            switch (wallpaper.myFPS)
            {
                case FPS.Min:
                    vlcParameters.Add(VLCParameter.FPS+"1");
                    vlcParameters.Add(VLCParameter.VideoProcessingLQ);
                    break;
                case FPS.Preview:
                    vlcParameters.Add(VLCParameter.FPS10);
                    vlcParameters.Add(VLCParameter.VideoProcessingLQ);
                    break;
                case FPS.FPS_30:
                    vlcParameters.Add(VLCParameter.FPS30);
                    break;
                case FPS.FPS_60:
                    vlcParameters.Add(VLCParameter.FPS60);
                    break;
            }
            #region Default Values die aktuell nicht geändert werden können

            vlcParameters.Add(VLCParameter.Loop);

            #endregion

            string[] allVLCParameter = vlcParameters.ToArray();
            return allVLCParameter;
        }
    }
}
