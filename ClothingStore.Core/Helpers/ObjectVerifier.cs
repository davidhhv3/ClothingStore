using ClothingStore.Core.Exceptions;

namespace ClothingStore.Core.Helpers
{
    internal static  class ObjectVerifier
    {
        internal static T VerifyExistence<T>(T? entity,string BusinessExceptionMessage) where T : class
        {           
            if (entity == null)
                throw new BusinessException(BusinessExceptionMessage);
            return entity;
        }

    }
}
