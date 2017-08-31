using eGandalf.Epi.Validation.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace eGandalf.Epi.Validation.Media
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaximumFileSizeAttribute : ValidationAttribute
    {

        public long Limit { get; set; }

        private long _actualBytes = 0;

        public MaximumFileSizeAttribute(long byteLimit)
        {
            Limit = byteLimit;
        }

        public override bool IsValid(object value)
        {
            if (value is ContentReference reference) return ValidateFileByReference(reference);

            throw new TypeMismatchException("Max File Size validation can only be applied to ContentReference or compatible fields.");
        }

        private bool ValidateFileByReference(ContentReference reference)
        {
            var media = GetMediaContent(reference);

            if (media == null) throw new TypeMismatchException("Max File Size validation cannot be applied to non-media types.");

            using (Stream str = media.BinaryData.OpenRead())
            {
                _actualBytes = str.Length;
            }

            return _actualBytes <= Limit;
        }

        private MediaData GetMediaContent(ContentReference reference)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            try
            {
                return contentLoader.Get<MediaData>(reference);
            }
            catch
            {
                return null;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationLocalization
                .GetFormattedErrorMessage("maxfilesize",
                new object[] { name, FormatBytes(_actualBytes), FormatBytes(Limit) });
        }

        const double gb = 2 ^ 30;
        const double mb = 2 ^ 20;
        const double kb = 2 ^ 10;

        private string FormatBytes(long bytes)
        {
            if (bytes > gb) return $"{Math.Round(bytes / gb, 2)} {LocalizationService.Current.GetString("/egandalf/byteformats/gb")}";
            if (bytes > mb) return $"{Math.Round(bytes / mb, 2)} {LocalizationService.Current.GetString("/egandalf/byteformats/mb")}";
            if (bytes > kb) return $"{Math.Round(bytes / kb, 2)} {LocalizationService.Current.GetString("/egandalf/byteformats/kb")}";
            return $"{bytes} {LocalizationService.Current.GetString("/egandalf/byteformats/bytes")}";
        }
    }
}
