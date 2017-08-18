using System;
using System.Collections;
using EPiServer.Core;
using System.ComponentModel.DataAnnotations;

namespace eGandalf.Epi.Validation.Lists
{
    /// <summary>
    /// Detects whether the minimum required items within a ContentArea or IList condition has been met.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaximumAttribute : ValidationAttribute
    {
        public int Limit { get; }
        public MaximumAttribute(int limit)
        {
            Limit = limit;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            if (value is ContentArea area) return ValidateContentArea(area);
            if (value is IList list) return ValidateList(list);

            throw new TypeMismatchException("Maximum Item validation can only be used with ContentArea or IList types.");
        }

        private bool ValidateList(IList list)
        {
            return list?.Count <= Limit;
        }

        private bool ValidateContentArea(ContentArea area)
        {
            return area?.Items?.Count <= Limit;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Too many items in '{name}'. Maximum required is {Limit}";
        }
    }
}
