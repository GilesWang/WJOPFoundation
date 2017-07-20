using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    public enum Status
    {
        None = -1,
        Success = 0,
        KeyNotFound = 1,
        KeyExists = 2,
        ValueTooLarge = 3,
        InvalidArguments = 4,
        ItemNotStored = 5,
        IncrDecrOnNonNumericValue = 6,
        VBucketBelongsToAnotherServer = 7,
        AuthenticationError = 32,
        AuthenticationContinue = 33,
        InvalidRange = 34,
        UnknownCommand = 129,
        OutOfMemory = 130,
        NotSupported = 131,
        InternalError = 132,
        Busy = 133,
        TemporaryFailure = 134,
        ClientFailure = 409,
        OperationTimeout = 512,
        NoReplicasFound = 768,
        NodeUnavailable = 1024,
        TransportFailure = 1280,
        DocumentMutationLost = 1536
    }
}
