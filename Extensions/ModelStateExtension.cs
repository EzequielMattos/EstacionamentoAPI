using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EstacionamentoAPI.Extensions
{
    public static class ModelStateExtension 
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();

            foreach (var item in modelState.Values)
                errors.AddRange(item.Errors.Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
