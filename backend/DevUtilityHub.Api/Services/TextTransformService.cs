using DevUtilityHub.Api.Core.Transformers;
using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Models.Responses;
using DevUtilityHub.Api.Services.Interfaces;

namespace DevUtilityHub.Api.Services
{
	public class TextTransformService : ITextTransformService
	{
		private readonly CaseTransformer _case = new();
		private readonly LineTransformer _line = new();
		private readonly CleanupTransformer _cleanup = new();

		public TextTransformResponse Transform(TextTransformRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Input))
				return new TextTransformResponse { IsValid = false, ErrorMessage = "Input is required." };

			var current = request.Input;
			var appliedOps = new List<string>();

			foreach (var op in request.Operations)
			{
				var result = op switch
				{
					"uppercase" => _case.ToUpperCase(current),
					"lowercase" => _case.ToLowerCase(current),
					"titlecase" => _case.ToTitleCase(current),
					"camelcase" => _case.ToCamelCase(current),
					"snakecase" => _case.ToSnakeCase(current),
					"kebabcase" => _case.ToKebabCase(current),
					"trim"      => _cleanup.Trim(current),
					"sort"      => _line.Sort(current),
					"dedup"     => _line.Dedup(current),
					"reverse"   => _line.Reverse(current),
					"count"     => _line.Count(current),
					_           => null
				};

				if (result is not null)
				{
					current = result;
					appliedOps.Add(op);
				}
			}

			return new TextTransformResponse
			{
				IsValid = true,
				Output = current,
				AppliedOperations = appliedOps
			};
		}
	}
}
