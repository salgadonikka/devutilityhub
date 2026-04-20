using DevUtilityHub.Api.Core.Diff;
using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Models.Responses;
using DevUtilityHub.Api.Services.Interfaces;

namespace DevUtilityHub.Api.Services
{
	public class DiffService : IDiffService
	{
		private readonly TextDiffEngine _engine = new();

		public DiffResponse Compare(DiffRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.TextA) || string.IsNullOrWhiteSpace(request.TextB))
				return new DiffResponse { IsValid = false, ErrorMessage = "Both TextA and TextB are required." };

			try
			{
				var (lines, addedCount, removedCount) = _engine.Compute(request.TextA, request.TextB);
				return new DiffResponse { IsValid = true, Lines = lines, AddedCount = addedCount, RemovedCount = removedCount };
			}
			catch (Exception ex)
			{
				return new DiffResponse { IsValid = false, ErrorMessage = ex.Message };
			}
		}
	}
}
