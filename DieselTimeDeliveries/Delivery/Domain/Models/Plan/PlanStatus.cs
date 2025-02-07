using SharedKernel;

namespace Delivery.Domain.Models;

public class PlanStatus : ValueObject
{
    private PlanStatus(PlanStatusEnum planStatusEnum)
    {
        PlanStatusEnum = planStatusEnum;
    }

    public PlanStatusEnum PlanStatusEnum { get; }

    public static PlanStatus Create(PlanStatusEnum planStatusEnum)
    {
        return new PlanStatus(planStatusEnum);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlanStatusEnum;
    }
}