using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for application management operations
    /// </summary>
    public class ApplicationBll(IBaseDal<Domain.Entities.Application, Guid, ApplicationFilter> baseDal, IConstraintBll constraintBll) : BaseBll<MobCentra.Domain.Entities.Application, Guid, ApplicationFilter>(baseDal), IApplicationBll
    {
        /// <summary>
        /// Retrieves applications with filtering by keyword (name in Arabic or Other), company, and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching applications</returns>
        public override async Task<PageResult<Domain.Entities.Application>> GetAllAsync(ApplicationFilter searchParameters)
        {
            // Build search expression with keyword, company, and active status filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Domain.Entities.Application, bool>(a =>
                ((searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword)) || (searchParameters.Keyword.IsNullOrEmpty() || a.NameOt.Contains(searchParameters?.Keyword)))
                && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }
            return await base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new application after validating constraints and uploading application file if provided
        /// </summary>
        /// <param name="entity">The application entity to add</param>
        public override async Task AddAsync(Domain.Entities.Application entity)
        {
            // Check if company has reached the maximum number of applications limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfApplications);

            var (isValid, errorMessage) = ValidateApkSignature(entity.File);
            if (!isValid)
                 throw new Exception(errorMessage);
            // Upload application file if provided
            if (!entity.File.IsNullOrEmpty())
                entity.File = await entity.File.UplodaFiles(name:Guid.NewGuid().ToString());
            await base.AddAsync(entity);
        }
        private static (bool isValid, string errorMessage) ValidateApkSignature(string base64)
        {
            const byte ZipFirst = 0x50;  // 'P'
            const byte ZipSecond = 0x4B; // 'K'
            // Standard ZIP local file header, end of central dir, or spanned archive
            static bool IsValidZipHeader(byte b2, byte b3) =>
                (b2 == 0x03 && b3 == 0x04) || (b2 == 0x05 && b3 == 0x06) || (b2 == 0x07 && b3 == 0x08);

            string data = base64.Trim();
            if (data.Length == 0) return (false, "File content is empty.");
            // Strip optional data URL prefix (e.g. data:application/vnd.android.package-archive;base64,)
            int commaIndex = data.IndexOf(',');
            if (commaIndex >= 0 && data.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                data = data[(commaIndex + 1)..].Trim();
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(data);
            }
            catch (FormatException)
            {
                return (false, "Invalid base64 file content.");
            }
            if (bytes.Length < 4)
                return (false, "File is not a valid APK. Content is too short.");
            if (bytes[0] != ZipFirst || bytes[1] != ZipSecond)
                return (false, "File is not a valid APK. Only APK (Android package) format is allowed.");
            if (!IsValidZipHeader(bytes[2], bytes[3]))
                return (false, "File is not a valid APK. Invalid file signature.");
            return (true, null!);
        }
        /// <summary>
        /// Updates an existing application, handling file upload if file has changed
        /// </summary>
        /// <param name="entity">The application entity with updated information</param>
        public override async Task UpdateAsync(Domain.Entities.Application entity)
        {
            // Get existing application to compare file
            var app = await GetByIdAsync(entity.Id);

            // Upload new file if file has changed and is not empty
            if (app.File != entity.File && !entity.File.IsNullOrEmpty())
                entity.File = await app.File.UplodaFiles();
            else
                entity.File = app.File;
            await base.UpdateAsync(entity);
        }
    }
}
