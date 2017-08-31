using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.ComponentModel.DataAnnotations;
using eGandalf.Epi.Validation.Internal;

namespace eGandalf.Epi.Validation.Media
{
    /// <summary>
    /// Matches a placed image against maximum dimension (width and height) requirements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaximumDimensionsAttribute : ValidationAttribute
    {
        private Injected<IContentRepository> _contentRepository;

        public int Width { get; set; }
        public int Height { get; set; }

        public MaximumDimensionsAttribute(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override bool IsValid(object value)
        {
            var reference = value as ContentReference;
            if (reference == null) return true; // Will let Required or RequiredForPublish determine whether this fails.

            var mediaContent = _contentRepository.Service.Get<ImageData>(reference);
            if (mediaContent == null) throw new TypeMismatchException("Dimension validation can only be used with Episerver ImageData or inheriting types.");

            using (var image = ImageDataMethods.ToSystemImage(mediaContent))
            {
                if (image == null) throw new Exception("Unable to load image file. The file may be unavailable or in an incorrect format.");
                return image.Width <= this.Width && image.Height <= this.Height;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationLocalization
                .GetFormattedErrorMessage("maximumdimensions",
                new object[] { name, Width, Height });
        }
    }
}
