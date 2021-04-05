using Hahn.Mobile.Properties;

namespace Hahn.Mobile.Validators.Rules
{
    public class NotNullOrEmptyRule : ValidationRule<string>
    {
        public NotNullOrEmptyRule() : base(false) { }

        public override bool Check(string value)
        {
            var result = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
            Message = Resources.FieldRequired;
            return result;
        }
    }
}
