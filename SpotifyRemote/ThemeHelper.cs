using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.PlatformUI;

using MediaColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;

namespace SpotifyRemoteNS
{
    static class ThemeHelper
    {

        public enum eVSTheme
        {
            kDark = 1,
            kBlue = 2,
            kLight = 3,
            kUnknown = 0
        }


        public static eVSTheme GetTheme()
        {
            System.Drawing.Color c = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);

            if (c == System.Drawing.Color.FromArgb(255, 37, 37, 38))
            {
                //Dark..
                return eVSTheme.kDark;
            }
            else if (c == System.Drawing.Color.FromArgb(255, 255, 255, 255))
            {
                return eVSTheme.kBlue;
            }
            else if (c == System.Drawing.Color.FromArgb(255, 245, 245, 245))
            {
                return eVSTheme.kLight;
            }
            else
            {

                return eVSTheme.kUnknown;
            }

        }
        //thnx
        public static MediaColor ToMediaColor(this DrawingColor color)
        {

            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static DrawingColor ToDrawingColor(this MediaColor color)
        {

            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }

    }
}
