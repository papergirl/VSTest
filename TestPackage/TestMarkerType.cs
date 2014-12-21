using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Guid(GuidList.GUIDTestMarkerString)]
    internal class TestMarkerType : IVsPackageDefinedTextMarkerType, IVsMergeableUIItem
    {
        public static int ID;

        #region IVsMergeableUIItem Members

        public int GetCanonicalName(out string pbstrNonLocalizeName)
        {
            pbstrNonLocalizeName = "GTestMarker";
            return VSConstants.S_OK;
        }

        public int GetDescription(out string pbstrDesc)
        {
            pbstrDesc = "Allows quick access to run a test through the gui";
            return VSConstants.S_OK;
        }

        public int GetDisplayName(out string pbstrDisplayName)
        {
            //TODO Localize and move to resource
            pbstrDisplayName = "GTestMarker";
            return VSConstants.S_OK;
        }

        public int GetMergingPriority(out int piMergingPriority)
        {
            piMergingPriority = 0x2001;
            return VSConstants.S_OK;
        }

        #endregion IVsMergeableUIItem Members

        public int GetVisualStyle(out uint pdwVisualFlags)
        {
            pdwVisualFlags = (int)(MARKERVISUAL.MV_GLYPH | MARKERVISUAL.MV_TIP_FOR_GLYPH);
            return VSConstants.S_OK;
        }

        public int GetDefaultColors(COLORINDEX[] piForeground, COLORINDEX[] piBackground)
        {
            
            return VSConstants.S_OK;
        }

        public int GetDefaultLineStyle(COLORINDEX[] piLineColor, LINESTYLE[] piLineIndex)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int GetDefaultFontFlags(out uint pdwFontFlags)
        {
            pdwFontFlags = (int)FONTFLAGS.FF_DEFAULT;
            return VSConstants.S_OK;
        }

        public int DrawGlyphWithColors(IntPtr hdc, RECT[] pRect, int iMarkerType, IVsTextMarkerColorSet pMarkerColors, uint dwGlyphDrawFlags, int iLineHeight)
        {

            Image image = Resources.MarkerImage;
            Graphics graphics = Graphics.FromHdc(hdc);
           
            graphics.DrawImage(image,pRect[0].left, pRect[0].top, pRect[0].left + pRect[0].right, pRect[0].bottom - pRect[0].top);

            return VSConstants.S_OK;
        }

        public int GetBehaviorFlags(out uint pdwFlags)
        {
            pdwFlags = (uint)(MARKERBEHAVIORFLAGS.MB_DEFAULT | MARKERBEHAVIORFLAGS.MB_LINESPAN);
            return VSConstants.S_OK;
        }

        public int GetPriorityIndex(out int piPriorityIndex)
        {
            piPriorityIndex = (int)MARKERTYPE.MARKER_READONLY;
            return VSConstants.S_OK;
        }
    }
}