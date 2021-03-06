using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions
{
    public class ListMerchItemsCountZeroException : Exception
    {
        public ListMerchItemsCountZeroException(string message) : base(message)
        {
        }

        public ListMerchItemsCountZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}