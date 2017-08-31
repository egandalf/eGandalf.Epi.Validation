using eGandalf.Epi.Validation.Internal;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGandalf.Epi.Validation.Text
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DoesNotContainAttribute : ValidationAttribute
    {
        public string MatchValue { get; set; }
        public DoesNotContainAttribute(string matchValue)
        {
            MatchValue = matchValue;
        }

        public override bool IsValid(object value)
        {
            var xhtml = value as string;
            if (xhtml == null) throw new TypeMismatchException("'Does Not Contain' validation rules can only be applied to string or compatible types.");

            return !(xhtml.IndexOf(MatchValue, 0, StringComparison.OrdinalIgnoreCase) > -1);
        }

        public override string FormatErrorMessage(string name)
        {
            return ValidationLocalization
                .GetFormattedErrorMessage("doesnotcontain",
                new object[] { name, MatchValue });
        }
    }
}
