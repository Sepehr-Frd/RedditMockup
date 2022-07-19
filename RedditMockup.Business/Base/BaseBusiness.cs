using System.Collections;
using AutoMapper;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Base;


public class BaseBusiness<T, DTO> : IBaseBusiness<T, DTO>
    where T : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<T> _repository;

    private readonly IMapper _mapper;

    protected BaseBusiness(IUnitOfWork unitOfWork, IBaseRepository<T> repository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _mapper = mapper;
    }

    protected async Task<SamanSalamatResponse?> CreateAsync(T t, CancellationToken cancellationToken = new())
    {

        var entity = await _repository.CreateAsync(t, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var response = _mapper.Map<DTO>(entity);

        return new SamanSalamatResponse
        {
            Data = response,
            IsSuccess = true,
            Message = "Entity Saved"
        };

    }

    protected async Task<SamanSalamatResponse?> UpdateAsync(T t, CancellationToken cancellationToken = new())
    {

        await _repository.UpdateAsync(t, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse
        {
            IsSuccess = true,
            Message = "Entity Updated"
        };
    }

    protected async Task<SamanSalamatResponse?> DeleteAsync(T t, CancellationToken cancellationToken = new())
    {   
        await _repository.DeleteAsync(t, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse
        {
            IsSuccess = true,
            Message = "Entity Deleted"
        };
    }

    public async Task<SamanSalamatResponse<IEnumerable>?> LoadAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = new())
    {
        var data = await _repository.LoadAllAsync(sieveModel, null, cancellationToken);

        var result = _mapper.Map<List<DTO>>(data);

        return new SamanSalamatResponse<IEnumerable>
        {
            Data = result,
            Message = "Data Loaded",
            IsSuccess = true
        };
    }

    Task<SamanSalamatResponse?> IBaseBusiness<T, DTO>.CreateAsync(T t, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<SamanSalamatResponse?> UpdateAsync(int id, DTO dto, CancellationToken cancellationToken = new())
    {
    }

    Task<SamanSalamatResponse?> IBaseBusiness<T, DTO>.DeleteAsync(T t, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<SamanSalamatResponse?> IBaseBusiness<T, DTO>.CreateAsync(DTO dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<SamanSalamatResponse?> IBaseBusiness<T, DTO>.UpdateAsync(int id, DTO dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<SamanSalamatResponse?> IBaseBusiness<T, DTO>.DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}