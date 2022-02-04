using AutoMapper;
using GISA.ChangeDataCapture.Providers.Application.Contracts;
using GISA.ChangeDataCapture.Providers.Domain.Entities;

namespace GISA.ChangeDataCapture.Providers.Application.Services
{
    public abstract class ServiceBase
    {
        private readonly IMapper _mapper;
        private readonly INotificationContext _notificationContext;

        public ServiceBase(IMapper mapper, INotificationContext notificationContext)
        {
            _mapper = mapper;
            _notificationContext = notificationContext;
        }

        protected TModel BuildMapper<TModel>(object source)
        {
            return _mapper.Map<TModel>
                (source, mapperOptions => mapperOptions
                .AfterMap((_, instance) =>
                {
                    if (instance is EntityBase) (instance as EntityBase).Validate(instance);
                }));
        }

        protected void AddNotification(string key, string message)
        {
            _notificationContext.AddNotification(key, message);
        }

        protected void CheckAndRegisterInvalidNotifications(EntityBase entityBase)
        {
            if (!entityBase.Valid) _notificationContext.AddNotifications(entityBase.ValidationResult);
        }
    }
}
