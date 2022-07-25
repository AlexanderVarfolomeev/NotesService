using FluentValidation.Results;
using Notes.Common.Exceptions;
using Notes.Common.Responses;

namespace Notes.Common.Extensions
{
    /// <summary>
    /// Extenstion for exceptions
    /// </summary>
    public static class ExceptionsException
    { 
        /// <summary>
        /// Convert validation exception to ErrorResponse
        /// </summary>
        /// <param name="data">Process exception</param>
        /// <returns></returns>
        public static ErrorResponse ToErrorResponse(this ValidationResult data)
        {
            var res = new ErrorResponse()
            {
                Message = "",
                FieldErrors = data.Errors.Select(x =>
                {
                    var elems = x.ErrorMessage.Split('&');
                    var errorName = elems[0];
                    var errorMessage = elems.Length > 0 ? elems[1] : errorName;
                    return new ErrorResponseFieldInfo()
                    {
                        FieldName = x.PropertyName,
                        Message = errorMessage,
                    };
                })
            };

            return res;
        }

        /// <summary>
        /// Convert process exception to ErrorResponse
        /// </summary>
        /// <param name="data">Process exception</param>
        /// <returns></returns>
        public static ErrorResponse ToErrorResponse(this ProcessException data)
        {
            var res = new ErrorResponse()
            {
                Message = data.Message
            };

            return res;
        }

        /// <summary>
        /// Convert exception to ErrorResponse
        /// </summary>
        /// <param name="data">Exception</param>
        /// <returns></returns>
        public static ErrorResponse ToErrorResponse(this Exception data)
        {
            var res = new ErrorResponse()
            {
                Message = data.Message
            };

            return res;
        }
    }


}
