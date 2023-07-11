using ClothingStore.Core.Exceptions;

namespace ClothingStore.Core.Helpers
{
    internal static  class ObjectVerifier
    {
        internal static T VerifyExistence<T>(T? entity,string BusinessExceptionMessage,int entityCount = 1) where T : class
        {           
            if (entity == null || entityCount == 0)
                throw new BusinessException(BusinessExceptionMessage);
            return entity;
        }

    }
}
