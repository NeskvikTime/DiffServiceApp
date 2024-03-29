﻿using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Contracts.Exceptions;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Domain.Models;
using MediatR;

namespace DiffServiceApp.Application.DiffCouple.Update;
sealed class UpdateDiffCommandHandler(IDiffCouplesRepository _diffCouplesRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<UpdateDiffCommand, DiffPayloadCouple>
{
    public async Task<DiffPayloadCouple> Handle(UpdateDiffCommand request, CancellationToken cancellationToken)
    {
        DiffPayloadCouple? diifCouple;

        if (!await _diffCouplesRepository.DiffCoupleExistsAsync(request.Id, cancellationToken))
        {
            diifCouple = CreateDiffPayloadCouple(request.Id, request.Side, request.Data);

            await _diffCouplesRepository.CreateDiffCoupleAsync(diifCouple, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return diifCouple;
        }

        diifCouple = await _diffCouplesRepository.GetDiffCoupleAsync(request.Id, cancellationToken);

        if (diifCouple is null)
        {
            throw new NotFoundException($"Diff with Id: '{diifCouple}' not found.");
        }

        UpdateDiffPayloadCouple(diifCouple, request.Side, request.Data);

        await _diffCouplesRepository.UpdateDiffPayloadAsync(diifCouple, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return diifCouple;
    }

    private void UpdateDiffPayloadCouple(DiffPayloadCouple diffPayloadCouple, string requestSide, string data)
    {
        byte[] dataToAssign = Convert.FromBase64String(data);

        if (requestSide == DiffDirection.Left)
        {
            diffPayloadCouple.LeftPayloadValue = dataToAssign;
            return;
        }

        // Right side to assign
        diffPayloadCouple.RightPayloadValue = dataToAssign;
    }

    private DiffPayloadCouple CreateDiffPayloadCouple(int coupleId, string requestSide, string data)
    {
        byte[] dataToAssign = Convert.FromBase64String(data);

        if (requestSide == DiffDirection.Left)
        {
            return new DiffPayloadCouple(coupleId, leftPayloadValue: dataToAssign);
        }

        // Right side to assign
        return new DiffPayloadCouple(coupleId, rightPayloadValue: dataToAssign);
    }
}
