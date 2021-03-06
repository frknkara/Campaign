using Data.Entities;
using Data.Repositories;
using Model.Shared;
using Service.Contracts;
using System;
using System.Linq;

namespace Service.Managers
{
    public class TimeManager : ITimeManager
    {
        private readonly IRepository<SystemConfig> _repository;

        public TimeManager(IRepositoryFactory repositoryFactory)
        {
            _repository = repositoryFactory.GetRepository<SystemConfig>();
        }

        public int GetTimeValue()
        {
            int result;
            if (int.TryParse(GetValue(Constraints.TIME_SYSTEM_CONFIG_KEY), out result))
                return result;
            throw new Exception("Time value is not valid.");
        }

        public int IncreaseTimeValue(int hour)
        {
            if (hour <= 0)
                throw new Exception("Hour increment value is not valid.");
            var updatedHour = GetTimeValue() + hour;
            Update(Constraints.TIME_SYSTEM_CONFIG_KEY, updatedHour.ToString());
            return updatedHour;
        }

        private string GetValue(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new Exception("System config key is not valid.");
            if (code.Length > Constraints.CODE_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of system config key should be {Constraints.CODE_COLUMN_MAX_LENGTH}.");

            var systemConfig = _repository.GetByCondition(x => x.Code == code).FirstOrDefault();
            if (systemConfig == null)
                throw new Exception("System config not found.");
            return systemConfig.Value;
        }

        private void Update(string code, string value)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new Exception("System config key is not valid.");
            if (code.Length > Constraints.CODE_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of system config key should be {Constraints.CODE_COLUMN_MAX_LENGTH}.");
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("System config value is not valid.");
            if (code.Length > Constraints.TEXT_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of system config value should be {Constraints.TEXT_COLUMN_MAX_LENGTH}.");

            var systemConfig = _repository.GetByCondition(x => x.Code == code).FirstOrDefault();
            if (systemConfig == null)
                throw new Exception("System config not found.");

            systemConfig.Value = value;
            _repository.Update(systemConfig);
        }
    }
}
