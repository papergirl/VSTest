using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Guid(GuidList.GUIDTestMarkerProviderString)]
    internal class TestMarkerProvider : IVsTextMarkerTypeProvider
    {
        public int GetTextMarkerType(ref Guid pguidMarker, out IVsPackageDefinedTextMarkerType ppMarkerType)
        {
            if (pguidMarker == GuidList.GUIDTestMarker)
            {
                ppMarkerType = new TestMarkerType();
                return VSConstants.S_OK;
            }
            ppMarkerType = null;
            return VSConstants.S_FALSE;
        }

        internal static void InitializeMarkerIds(IVsTextManager textManager)
        {
            // Retrieve the Text Marker IDs. We need them to be able to create instances.
            int markerId;
            Guid markerGuid = GuidList.GUIDTestMarker;
            
            ErrorHandler.ThrowOnFailure(textManager.GetRegisteredMarkerTypeID(ref markerGuid, out markerId));
            TestMarkerType.ID = markerId;
        }
    }
}
