using eGandalf.Epi.Validation.Internal;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eGandalf.Epi.Validation.General
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredForPublishAttribute : ValidationAttribute
    {
        public bool IsRequired { get; set; }

        public RequiredForPublishAttribute() : this(true) { }

        public RequiredForPublishAttribute(bool isRequired)
        {
            IsRequired = isRequired;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value is string str) return !string.IsNullOrEmpty(str);
            if (value is ContentReference reference) return reference != null && reference != ContentReference.EmptyReference;
            if (value is ContentArea area) return area?.Items?.Any() == true;
            if (value is IEnumerable<object> enumer) return enumer?.Any() == true;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationLocalization.GetFormattedErrorMessage("requiredforpublish", new[] { name });
        }
    }
}
