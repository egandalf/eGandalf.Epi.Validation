using eGandalf.Epi.Validation.Internal;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGandalf.Epi.Validation.Lists
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExactCountAttribute : ValidationAttribute
    {
        public int Limit { get; set; }

        public ExactCountAttribute(int limit)
        {
            Limit = limit;
        }

        public override bool IsValid(object value)
        {
            if (value == null && Limit == 0) return true;

            if (value is IList list) return ValidateList(list);
            if (value is ContentArea area) return ValidateContentArea(area);

            throw new TypeMismatchException("ExactCount validation is only supported with IList or ContentArea property types.");
        }

        private bool ValidateContentArea(ContentArea area)
        {
            return area?.Items?.Count == Limit;
        }

        private bool ValidateList(IList list)
        {
            return list?.Count == Limit;
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationLocalization.GetFormattedErrorMessage("exactcount", new object[] { name, Limit });
        }
    }
}
