using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Campaign;
using Model.Order;
using Model.Shared;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Managers
{
    public class CampaignManager : ICampaignManager
    {
        private readonly IRepository<Campaign> _repository;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public CampaignManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Campaign>();
            _repositoryFactory = repositoryFactory;
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
            var existingCampaign = _repository.GetByCondition(x => x.Name == campaign.Name).FirstOrDefault();
            if (existingCampaign != null)
                throw new Exception($"The campaign with {campaign.Name} name has already been created.");

            var entity = _mapper.Map<Campaign>(campaign);

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

        public List<OrderDto> GetCampaignOrders(CampaignDto campaign)
        {
            var orderRepository = _repositoryFactory.GetRepository<Order>();
            var campaignEndTime = campaign.CreationTime + campaign.Duration;
            var result = orderRepository.GetByCondition(x => x.ProductId == campaign.ProductId
                && x.CreationTime >= campaign.CreationTime
                && x.CreationTime <= campaignEndTime
                && x.RealCreationTime >= campaign.RealCreationTime).ToList();
            return _mapper.Map<List<OrderDto>>(result);
        }
    }
}
