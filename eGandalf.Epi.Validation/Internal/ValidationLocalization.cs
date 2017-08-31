using EPiServer.Framework.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGandalf.Epi.Validation.Internal
{
    internal static class ValidationLocalization
    {
        internal static string GetErrorMessageFormat(string node)
        {
            return LocalizationService.Current.GetString(
                GetFullPath(node)
                );
        }

        internal static string GetFormattedErrorMessage(
            string nodeName, 
            params object[] args)
        {
            var format = GetErrorMessageFormat(nodeName);
            return string.Format(format, args);
        }

        private static string GetFullPath(string node)
        {
            return $"/egandalf/validation/{node}/errormessage";
        }
    }
}
