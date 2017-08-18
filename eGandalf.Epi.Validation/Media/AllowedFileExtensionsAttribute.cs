using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGandalf.Epi.Validation.Media
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {

        public List<string> AllowedExtensions { get; set; }

        public AllowedFileExtensionsAttribute(string extension)
        {
            AllowedExtensions = new List<string>
            {
                $".{extension.Trim('.')}"
            };
        }

        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            AllowedExtensions = new List<string>();
            PopulateExtensionsFromArray(extensions);
        }

        public AllowedFileExtensionsAttribute(List<string> extensions)
        {
            AllowedExtensions = new List<string>();
            PopulateExtensionsFromArray(extensions.ToArray());
        }

        public override bool IsValid(object value)
        {
            if (value is ContentReference reference) return ValidateFileByReference(reference);

            throw new TypeMismatchException("Allowed Extensions validation can only be applied to ContentReference or compatible fields.");
        }

        private bool ValidateFileByReference(ContentReference reference)
        {
            var media = GetMediaContent(reference);

            if (media == null) return false;

            var matches = AllowedExtensions.Select(ext => media.BinaryDataContainer.Fragment.EndsWith(ext));
            return matches.Any();
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
            return $"Files placed in '{name}' must be of one of these types: {string.Join(", ", AllowedExtensions)}";
        }

        private void PopulateExtensionsFromArray(string[] extensionsArray)
        {
            AllowedExtensions.AddRange(extensionsArray.Select(e => $".{e.Trim('.')}"));
        }
    }
}
