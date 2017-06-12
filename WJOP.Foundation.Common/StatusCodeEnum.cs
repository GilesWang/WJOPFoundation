using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common
{
    public enum StatusCodeEnum
    {
        Init = -1,
        Error = 0,
        Success = 1,
        UploadFail = 11,
        HttpActionNotAllowed = 13,
        ServiceCurrentlyUnavailable = 15,
        InsufficientUserPermissions = 17,
        MissingMethod = 19,
        InvalidMethod = 21,
        InvalidFormat = 23,
        MissingSession = 25,
        InvalidSession = 27,
        MissingAppKey = 29,
        InvalidAppKey = 31,
        MissingTimestamp = 33,
        InvalidTimestamp = 35,
        MissingVersion = 37,
        InvalidVersion = 39,
        InvalidAccessToken = 41,
        ExpiredAccessToken = 43,
        InvalidEncoding = 45,
        InvalidParameter = 51,
        EndOfPlatform = 100,
        DbError = 101,
        DbTimeout = 103,
        DbException = 105,
        DbNoExistData = 107,
        EndOfDatabase = 150,
        KeyNotFound = 10001,
        KeyExists = 10002,
        ValueTooLarge = 10003,
        InvalidArguments = 10004,
        ItemNotStored = 10005,
        IncrDecrOnNonNumericValue = 10006,
        VBucketBelongsToAnotherServer = 10007,
        AuthenticationError = 10032,
        AuthenticationContinue = 10033,
        InvalidRange = 10034,
        UnknownCommand = 10129,
        OutOfMemory = 10130,
        NotSupported = 10131,
        InternalError = 10132,
        Busy = 10133,
        TemporaryFailure = 10134,
        ClientFailure = 10409,
        OperationTimeout = 10512,
        NoReplicasFound = 10768,
        NodeUnavailable = 11024,
        TransportFailure = 11280,
        DocumentMutationLost = 11536,
        InvalidCacheKey = 11792,
        InvalidCacheValue = 12048
    }
}
