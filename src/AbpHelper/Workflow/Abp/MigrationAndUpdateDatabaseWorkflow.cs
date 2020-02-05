﻿using AbpHelper.Extensions;
using AbpHelper.Models;
using AbpHelper.Steps;

namespace AbpHelper.Workflow.Abp
{
    public static class MigrationAndUpdateDatabaseWorkflow
    {
        public static WorkflowBuilder AddMigrationAndUpdateDatabaseWorkflow(this WorkflowBuilder builder)
        {
            return builder
                    /* Add migration */
                    .AddStep<FileFinderStep>(
                        step => step.SearchFileName = "*.EntityFrameworkCore.DbMigrations.csproj",
                        step => step.ResultParameterName = "MigrationProjectFile"
                    )
                    .AddStep<FileFinderStep>(
                        step => step.SearchFileName = "*.Web.csproj",
                        step => step.ResultParameterName = "WebProjectFile"
                    )
                    .AddStep<RunCommandStep>(
                        step =>
                        {
                            var entityInfo = step.Get<EntityInfo>();
                            var migrationProjectFile = step.GetParameter<string>("MigrationProjectFile");
                            var webProjectFile = step.GetParameter<string>("WebProjectFile");
                            step.Command = $"dotnet ef migrations add Add{entityInfo.ClassName} -p \"{migrationProjectFile}\" -s \"{webProjectFile}\"";
                        })
                ;
        }
    }
}