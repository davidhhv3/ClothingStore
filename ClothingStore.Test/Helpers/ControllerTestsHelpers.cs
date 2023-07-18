using ClothingStore.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Test.Helpers
{
    internal static class ControllerTestsHelpers
    {
        public static void checkResponseApi<T>(OkObjectResult okResult, ApiResponse<T> returnedApiResponse, ApiResponse<T> expectedApiResponse)
        {
            Assert.NotNull(okResult);
            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            Assert.Equal(expectedApiResponse.Meta, returnedApiResponse.Meta);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<ApiResponse<T>>(returnedApiResponse);
        }
    }
}
