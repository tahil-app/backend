using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class GroupScheduleReport : BaseReport, IReport
{
    private readonly IGroupRepository _groupRepository;
    private readonly IApplicationContext _applicationContext;

    public GroupScheduleReport(LocalizedStrings localized, IGroupRepository groupRepository, IApplicationContext applicationContext)
        : base(localized)
    {
        _groupRepository = groupRepository;
        _applicationContext = applicationContext;
    }

    public ReportType Type => ReportType.GroupSchedule;

    public async Task<byte[]> GenerateAsync<T>(T dataSet)
    {
        int groupId;

        if (dataSet is int id)
            groupId = id;

        else if (dataSet is GroupDto group)
            groupId = group.Id;

        else
            throw new ArgumentException("Expected int (groupId) or GroupDto data", nameof(dataSet));

        return await GenerateGroupScheduleAsync(groupId);
    }

    public async Task<byte[]> GenerateGroupScheduleAsync(int groupId)
    {
        var group = await _groupRepository.GetAsync(r => r.Id == groupId && r.TenantId == _applicationContext.TenantId);

        if (group == null || group.Id == 0)
        {
            throw new ArgumentException($"Group with ID {groupId} not found", nameof(groupId));
        }

        // Get group schedules from database
        var groupSchedules = await _groupRepository.GetGroupSchedulesAsync(groupId, _applicationContext.TenantId);

        var content = new Action<IContainer>(container =>
        {
            container.Column(column =>
            {
                // Group Information Section
                column.Item().PaddingBottom(0).Column(groupInfo =>
                {
                    GenerateKeyValue(groupInfo.Item(), $"{Localized.Group} : ", group.Name ?? "");
                });

                // Weekly Schedule - Organized by Days
                column.Item().PaddingTop(15).Column(scheduleColumn =>
                {
                    var schedulesByDay = groupSchedules.GroupBy(s => s.Day).OrderBy(g => g.Key);
                    foreach (var dayGroup in schedulesByDay)
                    {
                        // Day Header
                        GenerateTextHeader(scheduleColumn.Item(), dayGroup.Key);

                        // Day Schedule Table
                        scheduleColumn.Item().PaddingVertical(10).Element(container =>
                            GenerateScheduleTable(container, dayGroup.ToList()));

                        scheduleColumn.Item().Height(10);
                    }

                });

            });
        });

        return GenerateReport(Localized.Schedules, "", content);
    }

}