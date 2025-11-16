using FluentValidation;
using ProjectDefense.Application.Interfaces;

namespace ProjectDefense.Application.Validators.Shared
{
    public static class StudentBannedValidator
    {
        public static IRuleBuilderOptions<T, string> MustNotBeBanned<T>(this IRuleBuilder<T, string> ruleBuilder, IStudentBlockRepository studentBlockRepository)
        {
            return ruleBuilder.MustAsync(async (studentId, cancellation) =>
            {
                if (string.IsNullOrEmpty(studentId))
                {
                    return true; 
                }
                var isBanned = await studentBlockRepository.IsStudentBannedAsync(studentId);
                return !isBanned;
            }).WithMessage("This student is blocked and cannot perform this action.");
        }
    }
}