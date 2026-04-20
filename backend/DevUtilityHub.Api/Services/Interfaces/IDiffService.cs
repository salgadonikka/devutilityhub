using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Models.Responses;

namespace DevUtilityHub.Api.Services.Interfaces
{
	public interface IDiffService
	{
		DiffResponse Compare(DiffRequest request);
	}
}
