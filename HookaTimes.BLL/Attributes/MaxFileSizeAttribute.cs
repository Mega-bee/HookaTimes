using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HookaTimes.BLL.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile formFile = value as IFormFile;
            if (formFile != null)
            {
                if (formFile.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maximum file size {_maxFileSize} exceeded!");
                }
            }
            return ValidationResult.Success;
        }

    }
}
