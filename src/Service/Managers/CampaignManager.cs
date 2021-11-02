using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Campaign;
using Model.Shared;
using Service.Contracts;
using System;
using System.Linq;

namespace Service.Managers
{
    public class CampaignManager : ICampaignManager
    {
        private readonly IRepository<Campaign> _repository;
        private readonly IProductManager _productManager;
        private readonly ITimeManager _timeManager;
        private readonly IMapper _mapper;

        public CampaignManager(IRepositoryFactory repositoryFactory,
            IProductManager productManager, 
            ITimeManager timeManager, 
            IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Campaign>();
            _productManager = productManager;
            _timeManager = timeManager;
            _mapper = mapper;
        }

        public CampaignDto CreateCampaign(CreateCampaignDto campaign)
        {
            if (string.IsNullOrWhiteSpace(campaign.Name))
                throw new Exception("Campaign name is not valid.");
            if (campaign.Name.Length > Constraints.NAME_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of campaign name should be {Constraints.NAME_COLUMN_MAX_LENGTH}.");
            if (campaign.Duration <= 0)
                throw new Exception("Duration is invalid.");
            if (campaign.PriceManipulationLimit <= 0 || campaign.PriceManipulationLimit >= 100)
                throw new Exception("Price manipulation limit is invalid.");
            if (campaign.TargetSalesCount < 0)
                throw new Exception("Target sales count is invalid.");
            var product = _productManager.GetProductInfo(campaign.ProductCode);
            var existingCampaign = _repository.GetByCondition(x => x.Name == campaign.Name).FirstOrDefault();
            if (existingCampaign != null)
                throw new Exception($"The campaign with {campaign.Name} name has already been created.");

            var entity = _mapper.Map<Campaign>(campaign);
            entity.ProductId = product.Id;
            entity.CreationTime = _timeManager.GetTimeValue();

            _repository.Create(entity);
            return _mapper.Map<CampaignDto>(entity);
        }

        public CampaignDto GetCampaignInfo(string campaignName)
        {
            if (string.IsNullOrWhiteSpace(campaignName))
                throw new Exception("Campaign name is not valid.");
            if (campaignName.Length > Constraints.NAME_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of campaign name should be {Constraints.NAME_COLUMN_MAX_LENGTH}.");

            var campaign = _repository.GetByCondition(x => x.Name == campaignName).FirstOrDefault();
            if (campaign == null)
                throw new Exception("Campaign not found.");
            return _mapper.Map<CampaignDto>(campaign);
        }
    }
}
