using System;
using System.Collections.Generic;
using System.Text;

namespace aplikacja_dziekanat
{
    public interface IFingerprintManager
    {
        bool IsFingerprintAvailable();
        void AuthenticateFingerprint(Action onSucceed, Action onCancel = null);
    }
}
