using System.Diagnostics.Contracts;
using Microsoft.VisualStudio.Shell;

namespace KittyAltruistic.CPlusPlusTestRunner
{

    public class ProvideTextMarker : RegistrationAttribute
    {
        private readonly string _markerName, _markerGUID, _markerProviderGUID;

        public ProvideTextMarker(string markerName, string markerGUID, string markerProviderGUID)
        {
            Contract.Requires(markerName != null);
            Contract.Requires(markerGUID != null);
            Contract.Requires(markerProviderGUID != null);

            _markerName = markerName;
            _markerGUID = markerGUID;
            _markerProviderGUID = markerProviderGUID;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            Key markerkey = context.CreateKey("Text Editor\\External Markers\\{" + _markerGUID + "}");
            markerkey.SetValue("", _markerName);
            markerkey.SetValue("Service", "{" + _markerProviderGUID + "}");
            markerkey.SetValue("DisplayName", "My Custom Text Marker");
            markerkey.SetValue("Package", "{" + context.ComponentType.GUID + "}");
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            context.RemoveKey("Text Editor\\External Markers\\" + _markerGUID);
        }
    }

}
