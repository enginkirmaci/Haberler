using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Haberler.Common
{
    public class BackgroundTaskHelper
    {
        static public async void UnRegisterBackgroundTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
                if (task.Value.Name == Settings.Application.TaskName)
                    task.Value.Unregister(true);

            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Clear();

            var secondaryTiles = await SecondaryTile.FindAllForPackageAsync();
            foreach (var item in secondaryTiles)
            {
                var secondaryTileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(item.TileId);

                secondaryTileUpdater.Clear();
            }
        }

        static public async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                    if (task.Value.Name == Settings.Application.TaskName)
                        task.Value.Unregister(true);

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = Settings.Application.TaskName;
                taskBuilder.TaskEntryPoint = Settings.Application.TaskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(Settings.News.RefreshInterval, false));
                var registration = taskBuilder.Register();
            }
        }
    }
}