using GISA.ChangeDataCapture.MessageBroker.Contracts;
using GISA.ChangeDataCapture.Worker.Contracts;

namespace GISA.ChangeDataCapture.Providers.Application.Services
{
    public class ProviderObserverService : IChangeDataCaptureObserver
    {
        private readonly IChangeDataCaptureNotification _changeDataCaptureNotification;

        public ProviderObserverService(IChangeDataCaptureNotification changeDataCaptureNotification)
        {
            _changeDataCaptureNotification = changeDataCaptureNotification;
        }

        public void Update<T>(T notification)
        {
            _changeDataCaptureNotification
                .PublishAsync(notification).ConfigureAwait(false);
        }
    }
}
