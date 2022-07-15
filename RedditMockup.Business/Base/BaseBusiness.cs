using AutoMapper;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Base;


public class BaseBusiness<T, DTO> : IBaseBusiness<DTO>
    where T : BaseEntity
    where DTO : IBaseDto
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<T> _repository;

    private readonly IMapper _mapper;

    public BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<T> repository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken = new())
    {
        var baseEntity = _mapper.Map<T>(dto);
        var baseEntityInstance = await _repository.CreateAsync(baseEntity, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<DTO>(baseEntityInstance);

        return new SamanSalamatResponse
        {
            Data = result,
            IsSuccess = true,
            Message = "Entity Saved"
        };

    }

    public async Task<SamanSalamatResponse<List<DTO>>?> LoadAllAsync(SieveModel model, CancellationToken cancellationToken = new())
    {
        var data = await _repository.LoadAllAsync(model, null, cancellationToken);
        var result = _mapper.Map<List<DTO>>(data);

        return new SamanSalamatResponse<List<DTO>>
        {
            Data = result,
            Message = "Data Loaded",
            IsSuccess = true
        };
    }

    public async Task<SamanSalamatResponse?> UpdateAsync(DTO dto, CancellationToken cancellationToken = new())
    {
        var baseEntity = _mapper.Map<T>(dto);
        await _repository.UpdateAsync(baseEntity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse
        {
            IsSuccess = true,
            Message = "Entity Updated"
        };
    }

    public async Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken = new())
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return new SamanSalamatResponse
        {
            IsSuccess = true,
            Message = "Entity Deleted"
        };
    }
}