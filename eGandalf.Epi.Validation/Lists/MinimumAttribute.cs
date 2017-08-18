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
    public class MinimumAttribute : ValidationAttribute
    {
        public int Limit { get; }
        public MinimumAttribute(int limit)
        {
            Limit = limit;
        }

        public override bool IsValid(object value)
        {
            if (value == null && Limit > 0) return false;

            if (value is ContentArea area) return ValidateContentArea(area);

            if (value is IList list) return ValidateList(list);

            throw new TypeMismatchException("Minimum Item validation can only be used with ContentArea or IList types.");
        }

        private bool ValidateList(IList list)
        {
            return list?.Count >= Limit;
        }

        private bool ValidateContentArea(ContentArea area)
        {
            return area?.Items?.Count >= Limit;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Too few items in '{name}'. Minimum required is {Limit}";
        }
    }
}
