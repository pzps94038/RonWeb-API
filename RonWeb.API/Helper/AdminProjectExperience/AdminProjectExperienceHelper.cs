using System.IO;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using RonWeb.API.Enum;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.ProjectExperience;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.AdminProjectExperience
{
    public class AdminProjectExperienceHelper : IAdminProjectExperienceHelper
    {
        private readonly RonWebDbContext _db;

        public AdminProjectExperienceHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<GetByIdProjectExperienceResponse> GetAsync(long id)
        {
            var projectExperience = await _db.ProjectExperience
                .Where(a => a.ProjectExperienceId == id)
                .SingleOrDefaultAsync();

            if (projectExperience == null)
            {
                throw new NotFoundException();
            }

            // 專案角色查詢
            var roleCodeQuery = _db.Code.Where(a => a.CodeTypeId == CodeEnum.ProjectRole.Description());
            var projectRoles = await _db.ProjectRole
                .Where(a => a.ProjectExperienceId == projectExperience.ProjectExperienceId)
                .Join(roleCodeQuery, a => a.RoleId, b => b.CodeId, (a, b) => new SelectItem<string>(a.RoleId, b.CodeName))
                .ToListAsync();

            // 技術工具查詢
            var technologyToolCodeQuery = _db.Code.Where(a => a.CodeTypeId == CodeEnum.TechnologyTool.Description());
            var technologyTools = await _db.TechnologyTool
                .Where(a => a.ProjectExperienceId == projectExperience.ProjectExperienceId)
                .Join(technologyToolCodeQuery, a => a.TechnologyToolId, b => b.CodeId, (a, b) => new SelectItem<string>(a.TechnologyToolId, b.CodeName))
                .ToListAsync();

            return new GetByIdProjectExperienceResponse()
            {
                ProjectExperienceId = projectExperience.ProjectExperienceId,
                Name = projectExperience.Name,
                Description = projectExperience.Description,
                Contributions = projectExperience.Contributions,
                ProjectRoles = projectRoles,
                TechnologyTools = technologyTools,
                CreateDate = projectExperience.CreateDate,
                CreateBy = projectExperience.CreateBy,
                UpdateDate = projectExperience.UpdateDate,
                UpdateBy = projectExperience.UpdateBy,
            };
        }

        public async Task<GetProjectExperienceResponse> GetListAsync(int? page)
        {
            var curPage = page.GetValueOrDefault(1);
            var pageSize = 10;
            var skip = (curPage - 1) * pageSize;

            // 獲取分頁Id
            var idList = await _db.ProjectExperience
                .OrderByDescending(a => a.CreateDate)
                .Select(a => new { a.ProjectExperienceId, a.CreateDate })
                .Skip(skip)
                .Take(pageSize)
                .Select(a => a.ProjectExperienceId)
                .ToListAsync();

            // 獲取所有的總數
            var total = await _db.ProjectExperience.CountAsync();

            // 獲取專案角色
            var roleCodeQuery = _db.Code.Where(a => a.CodeTypeId == CodeEnum.ProjectRole.Description());
            var projectRoleList = await _db.ProjectRole
                .Where(a => idList.Contains(a.ProjectExperienceId))
                .Join(roleCodeQuery, a => a.RoleId, b => b.CodeId, (a, b) => new
                {
                    a.ProjectExperienceId,
                    Options = new SelectItem<string>(a.RoleId, b.CodeName)
                })
                .ToListAsync();

            // 獲取技術工具
            var technologyToolCodeQuery = _db.Code.Where(a => a.CodeTypeId == CodeEnum.TechnologyTool.Description());
            var technologyToolList = await _db.TechnologyTool
                .Where(a => idList.Contains(a.ProjectExperienceId))
                .Join(technologyToolCodeQuery, a => a.TechnologyToolId, b => b.CodeId, (a, b) => new
                {
                    a.ProjectExperienceId,
                    Options = new SelectItem<string>(a.TechnologyToolId, b.CodeName)
                })
                .ToListAsync();

            // 建立結果列表
            var projectExperiences = await _db.ProjectExperience
                .Where(a => idList.Contains(a.ProjectExperienceId))
                .Select(a => new ProjectExperienceItem
                {
                    ProjectExperienceId = a.ProjectExperienceId,
                    Name = a.Name,
                    Description = a.Description,
                    Contributions = a.Contributions,
                    ProjectRoles = projectRoleList
                        .Where(role => role.ProjectExperienceId == a.ProjectExperienceId)
                        .Select(role => role.Options)
                        .ToList(),
                    TechnologyTools = technologyToolList
                        .Where(tool => tool.ProjectExperienceId == a.ProjectExperienceId)
                        .Select(tool => tool.Options)
                        .ToList(),
                    CreateDate = a.CreateDate
                })
                .OrderByDescending(a => a.CreateDate)
                .ToListAsync();

            // 返回結果
            var data = new GetProjectExperienceResponse
            {
                Total = total,
                ProjectExperiences = projectExperiences
            };

            return data;
        }

        public async Task UpdateAsync(long id, UpdateProjectExperienceRequest data)
        {
            var sanitizer = new HtmlSanitizer();
            // 自定義規則
            sanitizer.AllowedSchemes.Add("mailto"); // 添加對 連結 屬性的支持
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("alt"); // 添加對 alt 屬性的支持
            var projectExperience = await _db.ProjectExperience.SingleOrDefaultAsync(a => a.ProjectExperienceId == id);
            if (projectExperience == null)
            {
                throw new NotFoundException();
            }
            var executionStrategy = _db.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 專案經歷
                        projectExperience.Name = data.Name;
                        projectExperience.Description = sanitizer.Sanitize(data.Description);
                        projectExperience.Contributions = sanitizer.Sanitize(data.Contributions);
                        projectExperience.UpdateDate = DateTime.Now;
                        projectExperience.UpdateBy = data.UserId;
                        // 專案角色
                        var existProjectRoles = await _db.ProjectRole
                            .Where(a => a.ProjectExperienceId == projectExperience.ProjectExperienceId)
                            .ToListAsync();
                        if (existProjectRoles.Any())
                        {
                            _db.ProjectRole.RemoveRange(existProjectRoles);
                        }
                        var projectRoles = data.ProjectRoles.Select(a => new ProjectRole()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            RoleId = a.Value,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        }).ToList();
                        if (projectRoles.Any())
                        {
                            await _db.ProjectRole.AddRangeAsync(projectRoles);
                        }
                        // 專案描述相關檔
                        var descriptionFiles = data.DescriptionFiles.Select(a => new ProjectExperienceImage()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        })
                        .ToList();
                        if (descriptionFiles.Any())
                        {
                            await _db.ProjectExperienceImage.AddRangeAsync(descriptionFiles);
                        }
                        // 專案貢獻相關檔
                        var contributionsFiles = data.ContributionsFiles.Select(a => new ProjectExperienceImage()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        })
                        .ToList();
                        if (contributionsFiles.Any())
                        {
                            await _db.ProjectExperienceImage.AddRangeAsync(contributionsFiles);
                        }

                        await _db.SaveChangesAsync();
                        await tc.CommitAsync();
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public async Task DeleteAsync(long id)
        {
            var projectExperience = await _db.ProjectExperience.SingleOrDefaultAsync(a => a.ProjectExperienceId == id);
            if (projectExperience == null)
            {
                throw new NotFoundException();
            }
            var executionStrategy = _db.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _db.ProjectExperience.Remove(projectExperience);

                        // 移除專案角色
                        var existRoles = await _db.ProjectRole.Where(a => a.ProjectExperienceId == projectExperience.ProjectExperienceId).ToListAsync();
                        if (existRoles.Any())
                        {
                            _db.ProjectRole.RemoveRange(existRoles);
                        }

                        // 移除專案技術
                        var existTechnologyTools = await _db.TechnologyTool.Where(a => a.ProjectExperienceId == projectExperience.ProjectExperienceId).ToListAsync();
                        if (existTechnologyTools.Any())
                        {
                            _db.TechnologyTool.RemoveRange(existTechnologyTools);
                        }

                        // 移除專案經歷圖片
                        var images = await _db.ProjectExperienceImage.Where(a => a.ProjectExperienceId == id).ToListAsync();
                        if (images.Any())
                        {
                            _db.ProjectExperienceImage.RemoveRange(images);
                        }
                        await _db.SaveChangesAsync();
                        await tc.CommitAsync();

                        // 移除雲端圖片
                        var storageBucket = Environment.GetEnvironmentVariable(EnvVarEnum.STORAGE_BUCKET.Description())!;
                        var storageTool = new FireBaseStorageTool(storageBucket);
                        foreach (var image in images)
                        {
                            await storageTool.Delete(image.Path);
                        }
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public async Task CreateAsync(CreateProjectExperienceRequest data)
        {
            var executionStrategy = _db.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tc = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var sanitizer = new HtmlSanitizer();
                        sanitizer.AllowedSchemes.Add("mailto");
                        sanitizer.AllowedAttributes.Add("class");
                        sanitizer.AllowedAttributes.Add("alt");

                        var projectExperience = new ProjectExperience()
                        {
                            Name = data.Name,
                            Description = sanitizer.Sanitize(data.Description),
                            Contributions = sanitizer.Sanitize(data.Contributions),
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        };
                        await _db.ProjectExperience.AddAsync(projectExperience);
                        await _db.SaveChangesAsync();

                        // 增加專案角色
                        var projectRoles = data.ProjectRoles.Select(a => new ProjectRole()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            RoleId = a.Value,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });
                        if (projectRoles.Any())
                        {
                            await _db.ProjectRole.AddRangeAsync(projectRoles);
                        }

                        // 增加使用技術
                        var technologyTools = data.TechnologyTools.Select(a => new TechnologyTool()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            TechnologyToolId = a.Value,
                            CreateBy = data.UserId,
                            CreateDate = DateTime.Now
                        });
                        if (technologyTools.Any())
                        {
                            await _db.TechnologyTool.AddRangeAsync(technologyTools);
                        }

                        // 專案描述相關檔
                        var descriptionFiles = data.DescriptionFiles.Select(a => new ProjectExperienceImage()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        })
                        .ToList();
                        if (descriptionFiles.Any())
                        {
                            await _db.ProjectExperienceImage.AddRangeAsync(descriptionFiles);
                        }
                        // 專案貢獻相關檔
                        var contributionsFiles = data.ContributionsFiles.Select(a => new ProjectExperienceImage()
                        {
                            ProjectExperienceId = projectExperience.ProjectExperienceId,
                            FileName = a.FileName,
                            Path = a.Path,
                            Url = a.Url,
                            CreateDate = DateTime.Now,
                            CreateBy = data.UserId
                        })
                        .ToList();
                        if (contributionsFiles.Any())
                        {
                            await _db.ProjectExperienceImage.AddRangeAsync(contributionsFiles);
                        }

                        await _db.SaveChangesAsync();
                        await tc.CommitAsync();
                    }
                    catch
                    {
                        await tc.RollbackAsync();
                        throw;
                    }
                }
            });
        }

    }
}

