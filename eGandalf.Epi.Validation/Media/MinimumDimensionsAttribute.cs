using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eGandalf.Epi.Helpers.File.Image;

namespace eGandalf.Epi.Validation.Media
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinimumDimensionsAttribute : ValidationAttribute
    {
        private Injected<IContentRepository> _contentRepository;

        public int Width { get; set; }
        public int Height { get; set; }

        public MinimumDimensionsAttribute(int width, int height)
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

            using (var image = mediaContent.ToSystemImage())
            {
                if (image == null) throw new Exception("Unable to load image file. The file may be unavailable or in an incorrect format.");
                return image.Width >= this.Width && image.Height >= this.Height;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} may only contain an image that is at least {Width}px wide and {Height}px tall.";
        }
    }
}
